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

	[SerializeField] //TODO: DELETE LATER
	bool isHovered;

	public GameController gameRef;

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