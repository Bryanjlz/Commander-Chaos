using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public GameController gameRef;

	public GameObject healthBar;

	public int maxHealth = 3;
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

	void BecomeInvulnerable()
	{
		isInvulnerable = true;
		invulnTimer = maxInvulnTime;
		if (health > 0)
		{
			invulnFlash = StartCoroutine("Flash");
			GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
		}
	}

	void BecomeVulnerable()
	{
		isInvulnerable = false;
		GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		StopCoroutine(invulnFlash);
	}

	IEnumerator Flash()
	{
		WaitForSeconds delay = new WaitForSeconds(0.12f);
		while (true)
		{
			GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
			yield return delay;
			GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			yield return delay;
		}
	}
}
