using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : Enemy {
	[SerializeField]
	private GameObject bulletPrefab;
	[SerializeField]
	float shootRange;
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

		if (Vector2.Distance(transform.position, player.transform.position) < shootRange) {
			ZoneActivate();
		}
	}

    public override void PlayerActivate() {
		SetTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
		ZoneActivate();
	}

	public override void ZoneActivate() {
		SetTarget(Vector3.zero);
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
}