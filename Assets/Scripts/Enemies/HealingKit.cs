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
		int times = 0;
		while (!foundTarget)
		{
			targetPos = new Vector2(Random.Range(-10, 10), Random.Range(-4, 4));
			Vector2 deltaVec = targetPos - (Vector2)transform.position;
			RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(3, 3), 0f, deltaVec, deltaVec.magnitude, LayerMask.GetMask("Player"));
			if (!hit)
			{
				foundTarget = true;
			}
			++times;
			if (times >= 1000)
			{
				Destroy(this);
				break;
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

    public override void BorderChange()
    {
        if (!isInteractable || isScrambled)
        {
			for (int i = 0; i < borderRenderers.Length; i++)
			{
				spriteRenderers[i].color = new Color(0.65f, 0.89f, 0.64f);
			}
		}
        else if (isSelected)
        {
            for (int i = 0; i < borderRenderers.Length; i++)
            {
                spriteRenderers[i].color = originalColor;
                borderRenderers[i].color = Color.white;
                borderRenderers[i].enabled = true;
            }
        }
    }

    public override void CheckCollisions()
    {
        foreach (Collider2D collision in collisions)
        {
            if (collision.tag == "Scrambling")
            {
                isSelected = false;
                isScrambled = true;
            }
            else if (collision.tag == "Danger" || collision.tag == "Bullet")
            {
                DeafKill();
            }
            else if (collision.tag == "Player")
            {
                health = 0;
            }
            else if (!isScrambled && isInteractable && collision.gameObject.tag == "Selection")
            {
                isSelected = true;
            }
            else if (!isSelected && !isScrambled && isInteractable && collision.gameObject.tag == "Selection")
            {
                isSelected = true;
                int randomNum = Random.Range(1, 4);
                FindObjectOfType<AudioManager>().Play("s" + randomNum);
            }
            else if (collision.gameObject.tag == "Death")
            {
                // health = 0;
                DeafKill();
            }
            else
            {
                Debug.Log(collision);
            }
        }
    }

    protected override void Kill()
    {
        FindObjectOfType<AudioManager>().Play("heal");
        gameRef.onEnemyKill(this);
    }

    public override void ZoneActivate()
	{

	}

	public override void PlayerActivate()
	{
        FindObjectOfType<AudioManager>().Play("healprep");
        speed = 5.0f;
		isInteractable = false;
		SetTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)));
		ZoneActivate();
	}
}
