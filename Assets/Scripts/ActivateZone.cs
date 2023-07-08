using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateZone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Enemy") {
			collision.gameObject.GetComponent<Enemy>().SetZoneActivated();
            collision.gameObject.GetComponent<Enemy>().ZoneActivate();
		}
	}
}
