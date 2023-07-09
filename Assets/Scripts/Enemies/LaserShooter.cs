using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : Enemy {

	[SerializeField]
	Laser laser;
	[SerializeField]
	[Tooltip("How far from center will laser stop")]
	float laserRange;
	[SerializeField]
	float laserChargeTime;
	[SerializeField]
	float laserSustainTime;
	private bool startLaser;
	private float laserStartTime;
	private Vector3 laserTarget;

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
		Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
		transform.rotation = targetRotation;
	}

	public override void DefaultBehaviour() {
		// Set Target to middle
		SetTarget(player.transform.position);

		if (!startLaser) {
			// Move
			rb.AddForce(unitVel * speed);
			if (rb.velocity.magnitude >= speed) {
				rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
			}

			// Start shoot sequence if in range
			print(Vector2.Distance(player.transform.position, transform.position));
			if (Vector2.Distance(player.transform.position, transform.position) <= laserRange) {
				StartLaser();
			}
		} else if (Time.time - laserStartTime > laserChargeTime + laserSustainTime + 2f) {
			startLaser = false;
			speed = 1;
		}
	}

	private void StartLaser() {
        isInteractable = true;
        startLaser = true;
		speed = 0;
		rb.velocity = Vector2.zero;
		laserStartTime = Time.time;
		laser.StartLaser(laserStartTime);
	}

	public override void PlayerActivate() {
		laserTarget = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
	}

	public override void ZoneActivate() {
	}
}