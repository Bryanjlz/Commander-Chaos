using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public GameController gameRef;

	public GameObject healthBar;
	[SerializeField]
	GameObject sprites;

	public int maxHealth = 3;
	[SerializeField]
	private int health = 3;

	[SerializeField]
	private float maxInvulnTime = 1.0f;
	[SerializeField]
	private float invulnTimer = 0.0f;
	[SerializeField]
	private bool isInvulnerable = false;

	private Coroutine invulnFlash;

	// Start is called before the first frame update
	void Start()
	{
		health = maxHealth;
	}

	// Update is called once per frame
	void Update()
	{
		if (invulnTimer > 0.0f)
		{
			invulnTimer -= Time.deltaTime;
			if (invulnTimer <= 0.0f)
			{
				BecomeVulnerable();
			}
		}
	}

	void Kill()
	{
		Time.timeScale = 0;
		gameRef.onPlayerKill();
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Health" && health < maxHealth)
		{
			++health;
			return;
		}

		if (!isInvulnerable && 
			collision.tag != "Selection" && collision.tag != "Zone" && collision.tag != "Scrambling")
		{
			if (health > 0 && invulnTimer <= 0.0f)
			{
				--health;
				BecomeInvulnerable();
			}

			UpdateHealthBar();	

			if (health == 0)
			{
				Kill();
			}
		}
	}

	void UpdateHealthBar()
	{
		Vector3 origHealthPos = healthBar.transform.position;
		float healthRatio = 1.0f * health / maxHealth;
		healthBar.transform.position =
			new Vector3(
				-0.5f * (1.0f - healthRatio),
				origHealthPos.y,
				origHealthPos.z
		);

		healthBar.GetComponent<SpriteRenderer>().size = new Vector2(
			healthRatio, 1.0f
		);
	}

	void UpdateColorAlpha(float alpha) {
		foreach (SpriteRenderer sr in sprites.GetComponentsInChildren<SpriteRenderer>()) {
			Color currentColor = sr.color;
			sr.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
		}
	}

	void BecomeInvulnerable()
	{
		isInvulnerable = true;
		invulnTimer = maxInvulnTime;
		if (health > 0)
		{
			invulnFlash = StartCoroutine("Flash");
			UpdateColorAlpha(0.5f);
		}
	}

	void BecomeVulnerable()
	{
		isInvulnerable = false;
		UpdateColorAlpha(1.0f);
		StopCoroutine(invulnFlash);
	}



	IEnumerator Flash()
	{
		WaitForSeconds delay = new WaitForSeconds(0.12f);
		while (true)
		{
			UpdateColorAlpha(0.5f);
			yield return delay;
			UpdateColorAlpha(1.0f);
			yield return delay;
		}
	}
}
