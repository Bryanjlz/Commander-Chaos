using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrambler : Enemy
{

	[SerializeField]
	GameObject zoneOfScrambling;


	float fieldSize;
	[SerializeField]
	float maxFieldSize;

    // Start is called before the first frame update
    public override void Setup(Player player, GameController gameRef)
	{
        FindObjectOfType<AudioManager>().Play("Scrambler");
        base.Setup(player, gameRef);
		SetTarget(player.transform.position);

		fieldSize = 0.0f;
		zoneOfScrambling.GetComponent<SpriteRenderer>().size = Vector2.zero;
		zoneOfScrambling.GetComponent<CircleCollider2D>().radius = 0.0f;
	}

	public override void DefaultBehaviour()
	{

		// Move
		rb.AddForce(unitVel * speed);
		if (rb.velocity.magnitude >= speed) {
			rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
		}

		if (fieldSize < maxFieldSize)
		{
			fieldSize += Time.deltaTime;
			zoneOfScrambling.GetComponent<SpriteRenderer>().size = new Vector2(2 * fieldSize, 2 * fieldSize);
			// It's "Lenient"
			zoneOfScrambling.GetComponent<CircleCollider2D>().radius = fieldSize * 0.8f;
		}

		// Efficiency moment
		zoneOfScrambling.transform.position = transform.position;
	}

	public void FixedUpdate()
	{

		rb.AddTorque(10.0f);
		if (Mathf.Abs(rb.angularVelocity) >= 120.0f)
		{
			rb.angularVelocity = rb.angularVelocity / Mathf.Abs(rb.angularVelocity);
		}
	}

    protected override void Kill()
    {
        FindObjectOfType<AudioManager>().Stop("Scrambler");
        int randomNum = Random.Range(1, 9);
        FindObjectOfType<AudioManager>().Play("death" + randomNum);
        gameRef.onEnemyKill(this);
    }

    public override void ZoneActivate()
	{

	}

	public override void PlayerActivate()
	{

	}
}
