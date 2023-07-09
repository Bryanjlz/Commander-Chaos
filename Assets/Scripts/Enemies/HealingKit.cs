using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingKit : Enemy
{
	private Vector2 startPos;
	private Vector2 target;

	// Start is called before the first frame update
	public override void Setup(Player player, GameController gameRef)
	{
		base.Setup(player, gameRef);
		target = FindTarget();

		SetTarget(target);

		isInteractable = true;
	}

	private Vector2 FindTarget()
	{
		bool foundTarget = false;
		Vector2 targetPos = Vector2.zero;
		while (!foundTarget)
		{
			targetPos = new Vector2(Random.Range(-10, 10), Random.Range(-4, 4));
			Vector2 deltaVec = targetPos - (Vector2)transform.position;
			RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(3, 3), 0f, deltaVec, deltaVec.magnitude, LayerMask.GetMask("Player"));
			if (!hit)
			{
				foundTarget = true;
			}
		}
		return targetPos;
	}

	public override void DefaultBehaviour()
	{
		rb.AddForce(unitVel * speed);
		if (rb.velocity.magnitude >= speed)
		{
			rb.velocity = rb.velocity / rb.velocity.magnitude * speed;
		}
	}

	public override void ZoneActivate()
	{

	}

	public override void PlayerActivate()
	{
		speed = 5.0f;
		isInteractable = false;
		SetTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
		ZoneActivate();
	}
}
