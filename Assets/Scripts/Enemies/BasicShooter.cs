using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : Enemy {
	[SerializeField]
	private GameObject bulletPrefab;
	List<GameObject> myBullets;

	public override void Setup(Player player, GameController gameRef) {
		base.Setup(player, gameRef);
		myBullets = new List<GameObject>();
	}

	public override void DefaultBehaviour() {
		SetTarget(player.transform.position);

		// Move
		rb.AddForce(unitVel * speed);
		if (rb.velocity.magnitude >= speed) {
			rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
		}
	}

    public override void PlayerActivate() {
		SetTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
		ZoneActivate();
	}

	public override void ZoneActivate() {
        isInteractable = false;
        GameObject bullet = Instantiate(bulletPrefab);

        Vector2 leftVec = Quaternion.Euler(0f, 0f, -10) * unitVel;
        GameObject bullet2 = Instantiate(bulletPrefab);

        Vector2 rightVec = Quaternion.Euler(0f, 0f, 10) * unitVel;
        GameObject bullet3 = Instantiate(bulletPrefab);

        bullet.GetComponent<Bullet>().Setup(gameObject, unitVel, bullet2, bullet3);
        myBullets.Add(bullet);
        bullet2.GetComponent<Bullet>().Setup(gameObject, leftVec, bullet, bullet3);
        myBullets.Add(bullet2);
        bullet3.GetComponent<Bullet>().Setup(gameObject, rightVec, bullet, bullet2);
        myBullets.Add(bullet3);

        health = 0;
	}

	public override void CheckCollisions() {
		foreach (Collider2D collision in collisions) {
			// don't get hit by own bullet as it spawns
			if (myBullets.Contains(collision.gameObject)) {
				continue;
			}

            // usual collision stuff
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
}