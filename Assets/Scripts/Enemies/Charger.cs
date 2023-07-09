using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy {
	private bool isCharging;

	private bool isTurning;
	private float turnStartTime;
	[SerializeField]
	float turnSpeed;

	private Vector3 velocity = Vector3.zero;
	private Quaternion targetRotation;


	public override void Setup(Player player, GameController gameRef) {
		base.Setup(player, gameRef);
		isCharging = false;

		SetTarget(player.transform.position);
	}

	public override void SetTarget(Vector3 target) {
		// set direction
		Vector2 deltaPos = target - transform.position;
		unitVel = deltaPos / deltaPos.magnitude;

		Vector3 VectorToTarget = target - transform.position;
		Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * VectorToTarget;
		targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
	}

	public override void DefaultBehaviour() {

		// Stop for big turn
		if (isTurning && Time.time - turnStartTime > turnSpeed * 1.5f) {
			isCharging = true;
			isTurning = false;
			ZoneActivate();
		}

		// Rotate
		transform.rotation = MyTools.SmoothDampQuaternion(transform.rotation, targetRotation, ref velocity, turnSpeed);

		if (!isTurning) {
			// Move
			rb.AddForce(unitVel * speed * 1.2f);
			if (!isCharging && rb.velocity.magnitude >= speed) {
				rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
			}
		}
	}

	public override void PlayerActivate() {
        isInteractable = false;
		isTurning = true;
		turnStartTime = Time.time;
		rb.velocity = Vector2.zero;
		rb.totalForce = Vector2.zero;
		SetTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
	}

    public override void CheckCollisions()
    {
        foreach (Collider2D collision in collisions)
        {
            if (collision.tag == "Scrambling")
            {
                isSelected = false;
                isScrambled = true;
            }
            else if (collision.tag == "Danger" || collision.tag == "Player" || (collision.tag == "Bullet" && !isCharging))
            {
                health -= 1;
                Debug.Log(collision.gameObject);
            }
            else if (!isScrambled && isInteractable && collision.gameObject.tag == "Selection")
            {
                isSelected = true;
            }
            else if (collision.gameObject.tag == "Death")
            {
                health = 0;
            }
            else
            {
                Debug.Log(collision);
            }
        }
    }

    public override void ZoneActivate() {
		speed = 5;
		isInteractable = false;
		transform.GetChild(0).gameObject.SetActive(true);
		CheckCollisions();
	}
}