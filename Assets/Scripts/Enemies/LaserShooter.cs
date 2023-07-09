using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : Enemy {

	[SerializeField]
	float turnSpeed;
	[SerializeField]
	float movePerLaser;
	[SerializeField]
	Laser laser;
	[SerializeField]
	[Tooltip("How far from center will laser stop")]
	float laserRange;
	[SerializeField]
	float laserChargeTime;
	[SerializeField]
	float laserSustainTime;
	[SerializeField]
	float laserCooldownTime;
	private LaserState state = LaserState.NONE;
	private float laserStateStartTime;
	private Vector3 laserTarget;
	private Vector3 velocity = Vector3.zero;
	private Quaternion targetRotation;

	private bool sounded = false;

	public override void Setup(Player player, GameController gameRef) {
        isInteractable = false;
        base.Setup(player, gameRef);
		laser.Setup(laserSustainTime, laserChargeTime);
		laserTarget = player.transform.position;
	}

	public override void SetTarget(Vector3 target) {
		// set direction
		Vector2 deltaPos = target - transform.position;
		unitVel = deltaPos / deltaPos.magnitude;

		Vector3 VectorToTarget = laserTarget - transform.position;
		Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * VectorToTarget;
		targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
	}

	public override void DefaultBehaviour() {
		// Set Target
		SetTarget(player.transform.position);

		// Rotate
		transform.rotation = MyTools.SmoothDampQuaternion(transform.rotation, targetRotation, ref velocity, turnSpeed);

		switch(state) {
			case LaserState.NONE:
				// Move
				rb.AddForce(unitVel * speed);
				if (rb.velocity.magnitude >= speed) {
					rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
				}

				// Start shoot sequence if in range
				if (Vector2.Distance(player.transform.position, transform.position) <= laserRange) {
					StartLaser();
				}
				break;
			case LaserState.CHARGING:
				
				// Done Charging
				if (Time.time - laserStateStartTime > laserChargeTime) {
					state = LaserState.FIRING;
                    laserStateStartTime = Time.time;
                    rb.velocity = rb.velocity / 3;
                }
				break;
			case LaserState.FIRING:
                if (!sounded)
				{
                    FindObjectOfType<AudioManager>().Play("Laser");
                    sounded = true;
				}
                    
                speed = 0;
                // Done Firing
                turnSpeed = 1.5f;
                if (Time.time - laserStateStartTime > laserSustainTime) {
					state = LaserState.COOLDOWN;
					laserStateStartTime = Time.time;
					laserTarget = player.transform.position;
				}
				break;
			case LaserState.COOLDOWN:
                sounded = false;
                // Done cooldown
                turnSpeed = 0.5f;
				if (Time.time - laserStateStartTime > laserCooldownTime) {
					state = LaserState.NONE;
					laserStateStartTime = Time.time;
				}
				break;
		}
	}

	private void StartLaser() {
        isInteractable = true;
        state = LaserState.CHARGING;
		speed = 0;
		rb.velocity = Vector2.zero;
		laserStateStartTime = Time.time;
		laser.StartLaser(laserStateStartTime);
	}

    protected override void Kill()
    {
        FindObjectOfType<AudioManager>().Stop("Laser");
        int randomNum = Random.Range(1, 9);
        FindObjectOfType<AudioManager>().Play("death" + randomNum);
        gameRef.onEnemyKill(this);
    }

    public override void PlayerActivate() {
		laserTarget = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
	}

	public override void ZoneActivate() {
	}

	private enum LaserState {
		CHARGING,
		FIRING,
		COOLDOWN,
		NONE
	}
}