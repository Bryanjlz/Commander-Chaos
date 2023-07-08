using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	[SerializeField]
	protected int health = 1;

	[SerializeField] //TODO: DELETE LATER
	bool isSelected;

	[SerializeField] //TODO: DELETE LATER
	bool isInteractable;

	public GameController gameRef;

	void Update() {
		if (health <= 0) {
			Kill();
		}

		if (isInteractable && isSelected && Input.GetKeyDown("space")) {
			PlayerActivate();
		}
		DefaultBehaviour();
	}

	public abstract void DefaultBehaviour();

	public abstract void PlayerActivate();

	public abstract void ZoneActivate();

	void Kill() {
		gameRef.onEnemyKill(this);
	}

	public virtual void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Danger" || collision.gameObject.tag == "Player") {
			health -= 1;
		}
	}
}