using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : Enemy {
	public Player player;

	[SerializeField]
	float speed = 1;
	private float angle;

	private Rigidbody2D rb;
	private Vector2 unitVel;

	private GameObject bulletPrefab;

	public void Setup(Player player, GameController gameRef) {
		SetSpawnPoint();
		health = 1;
		isInteractable = true;
		this.player = player;
		this.gameRef = gameRef;
	}

	public override void DefaultBehaviour() {
		SetTarget(player.transform.position);

		// Move
		rb.AddForce(unitVel * speed);
		if (rb.velocity.magnitude >= speed) {
			rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
		}
	}

	public void SetTarget(Vector3 target) {
		// set direction
		Vector2 deltaPos = target - transform.position;
		unitVel = deltaPos / deltaPos.magnitude;

		// rotate to face target
		angle = Mathf.Atan(deltaPos.y / deltaPos.x);
		transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle);
	}

	public override void PlayerActivate() {
		SetTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
		ZoneActivate();
	}

	public override void ZoneActivate() {
	}

	public override void CheckCollisions() {
		base.CheckCollisions();
	}
}