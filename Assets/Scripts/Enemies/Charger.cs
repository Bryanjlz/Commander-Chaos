using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy {
	private bool isCharging;

	public override void Setup(Player player, GameController gameRef) {
		base.Setup(player, gameRef);
		isCharging = false;
	}

	public override void DefaultBehaviour() {
		// Set Target to middle
		if (!isCharging) {
			SetTarget(player.transform.position);
		}

		// Move
		rb.AddForce(unitVel * speed);
		if (rb.velocity.magnitude >= speed) {
			rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
		}
	}

	public override void PlayerActivate() {
		isCharging = true;
		SetTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
		ZoneActivate();
	}

	public override void ZoneActivate() {
		speed = 5;
		rb.velocity = unitVel * speed;
		isInteractable = false;
		transform.tag = "Danger";
		gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		CheckCollisions();
	}
}