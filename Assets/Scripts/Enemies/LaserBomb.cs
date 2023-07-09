using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBomb : Enemy {

	[SerializeField]
	Collider2D collider;
	[SerializeField]
	List<Laser> lasers;
	[SerializeField]
	float laserChargeTime;
	[SerializeField]
	float laserSustainTime;
	private float laserStartTime;

	private bool startLaser;
	private Vector2 startPos;
	private Vector2 target;

	public override void Setup(Player player, GameController gameRef) {
		base.Setup(player, gameRef);
		foreach (Laser laser in lasers) {
			laser.Setup(laserSustainTime, laserChargeTime);
		}
		startPos = gameObject.transform.position;
		target = FindTarget();

		SetTarget(target);
		laserStartTime = Time.time;

		// random spin
		rb.angularVelocity = Random.Range(15f, 60f) * Mathf.Pow(-1, Random.Range(0, 1));
	}

	private Vector2 FindTarget() {
		bool foundTarget = false;
		Vector2 targetPos = Vector2.zero;
		while (!foundTarget) {
			targetPos = new Vector2(Random.Range(-10, 10), Random.Range(-4, 4));
			Vector2 deltaVec = targetPos - (Vector2)transform.position;
			RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(2, 1), 0f, deltaVec, deltaVec.magnitude, LayerMask.GetMask("Player"));
			if (!hit) {
				foundTarget = true;
			}
		}
		return targetPos;
	}

	public override void SetTarget(Vector3 target) {
		// set direction
		Vector2 deltaPos = target - transform.position;
		unitVel = deltaPos / deltaPos.magnitude;
	}

	public override void DefaultBehaviour() {
		// Move
		if (!startLaser) {
			rb.AddForce(unitVel * speed);
			if (rb.velocity.magnitude >= speed) {
				rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
			}
			
			// Start bomb
			if (Vector2.Distance(transform.position, target) < 1f) {
				StartLaser();
			}
		}

		// Kill bomb after detonation
		if (startLaser && Time.time - laserStartTime > laserChargeTime + laserSustainTime + 0.5f) {
			health = 0;
		}
	}

	private void StartLaser() {
		startLaser = true;
		speed = 0;
		rb.velocity = Vector2.zero;
		laserStartTime = Time.time;
		foreach (Laser laser in lasers) {
			laser.StartLaser(laserStartTime);
		}
	}

	public override void PlayerActivate() {
	}

	public override void ZoneActivate() {
	}
}