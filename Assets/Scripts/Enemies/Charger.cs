using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy {
	public Player player;

	[SerializeField]
	float speed = 1;

	[SerializeField] // DEBUG, DELETE LATER
	float angle;

	public void Setup(Player player, GameController gameRef) {
		SetSpawnPoint();
		health = 1;
		isInteractable = true;
		this.player = player;
		this.gameRef = gameRef;
	}

	public override void DefaultBehaviour() {
		// move
		Vector3 deltaPos = player.transform.position - transform.position;
		transform.position += deltaPos / deltaPos.magnitude * speed * Time.deltaTime;

		// rotate to face player
		angle = Mathf.Atan(deltaPos.y / deltaPos.x);
		transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle);
	}

	public override void PlayerActivate() {
		print("CHARRGEEE");
	}

	public override void ZoneActivate() {
	}

	public override void OnTriggerEnter2D(Collider2D collision) {
		base.OnTriggerEnter2D(collision);
		print("yooo");
	}
}