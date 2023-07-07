using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	[SerializeField]
	int health;

	[SerializeField]
	bool isSelected;

	void Update() {
		if (health == 0) {
			Kill();
		}

		if (isSelected && Input.GetKeyDown("space")) {
			Activate();
		}
		DefaultBehaviour();
	}
	public abstract void DefaultBehaviour();

	public abstract void Activate();

	public void Kill() {
		Destroy(gameObject);
	}
}