using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	private readonly float X_POS_MAX = 10;
	private readonly float Y_POS_MAX = 6;

	[SerializeField] //TODO: DEBUG, DELETE LATER
	protected int health = 1;

	[SerializeField] //TODO: DEBUG, DELETE LATER
	protected bool isInteractable;
	[SerializeField] //TODO: DELETE LATER
	bool isHovered;
	public bool isSelected;

	protected GameController gameRef;

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

	public virtual void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Danger" || collision.gameObject.tag == "Player") {
			health -= 1;
		} else if (collision.gameObject.tag == "Selection")
		{
			isSelected = true;
		} else
		{
			Debug.Log(collision);
		}
	}

	public void OnMouseEnter()
	{
		isHovered = true;
	}

	public void OnMouseExit()
	{
		isHovered = false;
	}
}