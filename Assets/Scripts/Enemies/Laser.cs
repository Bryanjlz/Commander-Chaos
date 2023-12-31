using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	[SerializeField]
	Color laserOn;
	[SerializeField]
	Color laserFlash;
	[SerializeField]
	Color laserIndicator;
	[SerializeField]
	Collider2D collider;
	[SerializeField]
	SpriteRenderer sprite;

	float laserSustainTime;
	float laserChargeTime;
	float laserStartTime;

	LaserState state = LaserState.NONE;

	public void Setup(float laserSustainTime, float laserChargeTime) {
		this.laserSustainTime = laserSustainTime;
		this.laserChargeTime = laserChargeTime;
	}

	public void StartLaser(float laserStartTime) {
		this.laserStartTime = laserStartTime;
		StartCoroutine(Sounds());
        state = LaserState.CHARGING;
	}

	public void FireLaserEarly() {
		laserStartTime = Time.time;
		sprite.color = Color.white;
		state = LaserState.FIRING;
	}

	// Update is called once per frame
	void Update() {
		float timeSinceLaserStart = Time.time - laserStartTime;
		float test = Mathf.Floor(timeSinceLaserStart) * 2;
		switch (state) {
			case LaserState.CHARGING:
                sprite.enabled = true;
                if ((int)(timeSinceLaserStart * 2) % 2 == 0) {
					sprite.color = laserFlash;
                } else {
					sprite.color = laserIndicator;
				}
				if (timeSinceLaserStart >= laserChargeTime) {
					laserStartTime = Time.time;
					sprite.color = Color.white;
					state = LaserState.FIRING;
				}
				break;
			case LaserState.FIRING:
				collider.enabled = true;
				sprite.color = Color.Lerp(Color.white, laserOn, timeSinceLaserStart / 0.1f);
				if (timeSinceLaserStart >= laserSustainTime) {
					laserStartTime = Time.time;
					state = LaserState.STOPPING;
				}
				break;
			case LaserState.STOPPING:
				collider.enabled = false;
				sprite.color = Color.Lerp(laserOn, Color.clear, timeSinceLaserStart / 0.1f);
				if (timeSinceLaserStart >= 0.1f) {
					collider.enabled = false;
					sprite.enabled = false;
					state = LaserState.NONE;
				}
				break;
		}
	}

    IEnumerator Sounds()
    {
        FindObjectOfType<AudioManager>().Play("laserwarn");
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().Play("laserwarn");
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().Play("laserwarn");
    }

    private enum LaserState {
		CHARGING,
		FIRING,
		STOPPING,
		NONE
	}
}
