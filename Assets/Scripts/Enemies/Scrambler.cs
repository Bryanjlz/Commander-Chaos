using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrambler : Enemy
{

	[SerializeField]
	GameObject zoneOfScrambling;

    // Start is called before the first frame update
    public override void Setup(Player player, GameController gameRef)
	{
		base.Setup(player, gameRef);
	}

	public override void DefaultBehaviour()
	{
		SetTarget(player.transform.position);

		// Move
		rb.AddForce(unitVel * speed);
		if (rb.velocity.magnitude >= speed) {
			rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
		}

		// Efficiency moment
		zoneOfScrambling.transform.position = transform.position;
	}

	public override void ZoneActivate()
	{

	}

	public override void PlayerActivate()
	{

	}
}
