using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	[SerializeField]
	private float speed;
	private Vector2 unitVel = Vector2.up;

	private GameObject parent;
	private bool hit = false;

	public void Setup(GameObject parent, Vector2 unitVel) {
		this.parent = parent;
		this.unitVel = unitVel;
		transform.position = (Vector2)parent.transform.position + unitVel * 0.25f;
	}

	void Update() {
		transform.position += (Vector3)unitVel * speed * Time.deltaTime;
		if (hit) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		// TODO: should bullets destroy other bullets?
		if (parent == null || collision.gameObject != parent) {
			hit = true;
		}
	}
}
