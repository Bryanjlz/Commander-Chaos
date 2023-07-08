using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	// For spawning
	private readonly float X_POS_MAX = 19;
	private readonly float Y_POS_MAX = 11;

	// Set in editor
	[SerializeField]
	protected bool isInteractable;
	[SerializeField]
	protected int health;

	// Move variables
	protected Player player;
	[SerializeField]
	protected float speed = 1;
	[SerializeField]
	protected Rigidbody2D rb;
	protected Vector2 unitVel;

	[SerializeField] //TODO: DELETE LATER
	bool isHovered;
	public bool isSelected;

	protected GameController gameRef;

	[SerializeField]
	protected List<Collider2D> collisions;

	public virtual void Setup(Player player, GameController gameRef) {
		SetSpawnPoint();
		this.player = player;
		this.gameRef = gameRef;
	}

		protected void SetSpawnPoint() {
		int side = Random.Range(0, 4);
		switch(side) {
			case 0:
				transform.position = new Vector2(X_POS_MAX, Random.Range(-Y_POS_MAX, Y_POS_MAX));
				break;
			case 1:
				transform.position = new Vector2(-X_POS_MAX, Random.Range(-Y_POS_MAX, Y_POS_MAX));
				break;
			case 2:
				transform.position = new Vector2(Random.Range(-X_POS_MAX, X_POS_MAX), Y_POS_MAX);
				break;
			case 3:
				transform.position = new Vector2(Random.Range(-X_POS_MAX, X_POS_MAX), -Y_POS_MAX);
				break;
		}
	}

	void Update() {
		if (health <= 0) {
			Kill();
		}

		if (isInteractable && isSelected && Input.GetKeyDown("space")) {
			PlayerActivate();
		}

		// Monkey deselect/select
		if (Input.GetMouseButtonDown(0)) {
			isSelected = isHovered;
		}
		DefaultBehaviour();
	}

	public abstract void DefaultBehaviour();

	public abstract void PlayerActivate();

	public abstract void ZoneActivate();

	void Kill() {
		gameRef.onEnemyKill(this);
	}
	public virtual void SetTarget(Vector3 target) {
		// set direction
		Vector2 deltaPos = target - transform.position;
		unitVel = deltaPos / deltaPos.magnitude;

		// rotate to face target
		float angle = Mathf.Atan(deltaPos.y / deltaPos.x);
		transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		collisions.Add(collision);
		CheckCollisions();
	}

	void OnTriggerExit2D(Collider2D collision) {
		collisions.Remove(collision);
	}

	public virtual void CheckCollisions() {
		foreach (Collider2D collision in collisions) {
			if (collision.gameObject.tag == "Danger" || collision.gameObject.tag == "Player") {
				health -= 1;
			} else if (isInteractable && collision.gameObject.tag == "Selection") {
				isSelected = true;
			} else if (collision.gameObject.tag == "Death") {
				health = 0;
			} else {
				Debug.Log(collision);
			}
		}
	}

	public void OnMouseEnter() {
		isHovered = true;
	}

	public void OnMouseExit() {
		isHovered = false;
	}
}