using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Enemy {

	public override void Setup(Player player, GameController gameRef) {
		base.Setup(player, gameRef);
		SetTarget(player.transform.position);
		speed = Random.Range(0.5f, 1.5f);
		rb.velocity = unitVel * speed;
		// random spin
		rb.angularVelocity = Random.Range(15f, 60f) * Mathf.Pow(-1,Random.Range(0,1));
	}

	public override void DefaultBehaviour() {
		// It's a rock, why would it do things
	}

	public override void SetTarget(Vector3 target) {
		// set direction
		Vector2 deltaPos = target - transform.position;
		unitVel = deltaPos / deltaPos.magnitude;
	}

	public override void PlayerActivate() {
		// It's a rock, why would it do things
	}

	public override void ZoneActivate() {
		// It's a rock, why would it do things
	}
}