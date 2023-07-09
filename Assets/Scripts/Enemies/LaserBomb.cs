using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LaserBomb : Enemy {

	[SerializeField]
	Collider2D collider;
	[SerializeField]
	List<Laser> lasers;
	[SerializeField]
	float laserChargeTime;
	[SerializeField]
	float laserSustainTime;
	private float laserStateStartTime;
	private LaserState state = LaserState.NONE;
	private Vector2 target;

	public override void Setup(Player player, GameController gameRef) {
		base.Setup(player, gameRef);
		foreach (Laser laser in lasers) {
			laser.Setup(laserSustainTime, laserChargeTime);
		}
		target = FindTarget();

		SetTarget(target);

		// random spin
		rb.angularVelocity = Random.Range(30f, 45f) * Mathf.Pow(-1, Random.Range(0, 1));
	}

	protected override void Kill() {
		gameRef.onEnemyKill(this);
	}

	private Vector2 FindTarget() {
		bool foundTarget = false;
		Vector2 targetPos = Vector2.zero;
		while (!foundTarget) {
			targetPos = new Vector2(Random.Range(-10, 10), Random.Range(-4, 4));
			Vector2 deltaVec = targetPos - (Vector2)transform.position;
			RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(3, 3), 0f, deltaVec, deltaVec.magnitude, LayerMask.GetMask("Player"));
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
		switch (state) {
			case LaserState.NONE:
				// Move
				rb.AddForce(unitVel * speed);
				if (rb.velocity.magnitude >= speed) {
					rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
				}

				// Start bomb
				if (Vector2.Distance(transform.position, target) < 1f) {
					isInteractable = true;
					StartLaser();
				}
				break;
			case LaserState.CHARGING:
				// Done Charging
				if (Time.time - laserStateStartTime > laserChargeTime) {
					StartFiring();
				}
				break;
			case LaserState.FIRING:
				// Done Firing
				if (Time.time - laserStateStartTime > laserSustainTime) {
					state = LaserState.STOPPING;
					laserStateStartTime = Time.time;
				}
				break;
			case LaserState.STOPPING:
				// Done cooldown
				if (Time.time - laserStateStartTime > 0.2f) {
					health = 0;
				}
				break;
		}
	}

	private void StartLaser() {
		state = LaserState.CHARGING;
		laserStateStartTime = Time.time;
		speed = 0;
		rb.velocity = Vector2.zero;
		rb.totalForce = Vector2.zero;
		foreach (Laser laser in lasers) {
			laser.StartLaser(laserStateStartTime);
		}
	}

	private void StartFiring() {
		state = LaserState.FIRING;
		laserStateStartTime = Time.time;
		rb.angularVelocity = 15;

		// Screenshake
		float shakeStrength = MyTools.negativeCube(Mathf.Clamp(transform.position.magnitude / 8f, 0, 0.9f)) * 1.5f;
		impulseSource.m_DefaultVelocity = transform.position / transform.position.magnitude * shakeStrength;
		impulseSource.GenerateImpulseWithForce(1f);
	}

	public override void PlayerActivate() {
		StartFiring();

		foreach (Laser laser in lasers) {
			laser.FireLaserEarly();
		}
	}

	public override void ZoneActivate() {
	}

	private enum LaserState {
		CHARGING,
		FIRING,
		STOPPING,
		NONE
	}
}