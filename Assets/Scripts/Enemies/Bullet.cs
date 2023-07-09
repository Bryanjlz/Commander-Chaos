using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	[SerializeField]
	private float speed;
	private Vector2 unitVel = Vector2.up;

	private GameObject parent;
	private bool hit = false;

    [SerializeField]
    protected ParticleSystem deathParticles;

    // sibling bullets
    GameObject sib1;
    GameObject sib2;

    public void Setup(GameObject parent, Vector2 unitVel, GameObject sib1, GameObject sib2) {
		this.parent = parent;
		this.unitVel = unitVel;
		transform.position = (Vector2)parent.transform.position + unitVel * 0.25f;
		this.sib1 = sib1;
        this.sib2 = sib2;
    }

    void Update() {
		transform.position += (Vector3)unitVel * speed * Time.deltaTime;
		if (hit) {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
        // TODO: should bullets destroy other bullets?
        if (collision.gameObject == sib1 || collision.gameObject == sib2)
        {
			return;
        }

        // don't hit zones or selection or scrambler field
        if (collision.tag == "Zone" || collision.tag == "Selection" || collision.tag == "Scrambling") {
			return;
		}
		// don't hit the enemy spawning the bullet
		if ((parent == null || collision.gameObject != parent)) {
			Debug.Break();
			hit = true;
		}
	}
}
