using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy {
	public Player player;

	[SerializeField]
	float speed = 1;
	private float angle;

	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField] // DEBUG, DELETE LATER
	private Vector2 unitVel;

	bool isCharging;

	public void Setup(Player player, GameController gameRef) {
		SetSpawnPoint();
		health = 1;
		isInteractable = true;
		isCharging = false;
		this.player = player;
		this.gameRef = gameRef;
	}

	public override void DefaultBehaviour() {
		// Set Target to middle
		if (!isCharging) {
			SetTarget(player.transform.position);
		}
		rb.AddForce(unitVel * speed);
		if (rb.velocity.magnitude >= speed) {
			rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
		}
	}

	public void SetTarget (Vector3 target) {
		// set direction
		Vector2 deltaPos = target - transform.position;
		unitVel = deltaPos / deltaPos.magnitude;

		// rotate to face target
		angle = Mathf.Atan(deltaPos.y / deltaPos.x);
		transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle);
	}

	public override void PlayerActivate() {
		isCharging = true;
		SetTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
		ZoneActivate();
	}

	public override void ZoneActivate() {
		speed = 5;
		rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
		isInteractable = false;
		transform.tag = "Danger";
		gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		CheckCollisions();
	}

	public override void CheckCollisions() {
		base.CheckCollisions();
		print("yooo");
	}
}