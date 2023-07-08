using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	[SerializeField]
	private float speed = 1;
	private Vector2 unitVel = Vector2.up;

	private bool hit = false;

	void Update() {
		transform.position += (Vector3)unitVel * speed * Time.deltaTime;
		if (hit) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		hit = true;
	}
}
