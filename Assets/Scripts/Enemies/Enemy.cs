using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	// For spawning
	private readonly float X_POS_MAX = 19;
	private readonly float Y_POS_MAX = 11;

	// Set in editor
	[SerializeField]
	protected bool isInteractable;
	[SerializeField]
	protected int health;

	// Move variables
	protected Player player;
	[SerializeField]
	protected float speed = 1;
	[SerializeField]
	protected Rigidbody2D rb;
	protected Vector2 unitVel;

	[SerializeField] //TODO: DELETE LATER
	bool isHovered;
	public bool isSelected;

	protected bool isActivated = false;
    protected bool isZoneActivated = false;
    protected GameController gameRef;

	[SerializeField]
	protected List<Collider2D> collisions;

    // Borders and particles
    [SerializeField]
    protected SpriteRenderer[] borderRenderers;

    [SerializeField]
    protected ParticleSystem deathParticles;

    public void Start()
    {
        if (borderRenderers.Length != 0)
		{
			for (int i = 0; i < borderRenderers.Length; i++)
			{
				borderRenderers[i].enabled = false;
            }
        }
    }

    public virtual void Setup(Player player, GameController gameRef) {
		SetSpawnPoint();
		this.player = player;
		this.gameRef = gameRef;
    }

		protected void SetSpawnPoint() {
		int side = Random.Range(0, 4);
		switch(side) {
			case 0:
				transform.position = new Vector2(X_POS_MAX, Random.Range(-Y_POS_MAX, Y_POS_MAX));
				break;
			case 1:
				transform.position = new Vector2(-X_POS_MAX, Random.Range(-Y_POS_MAX, Y_POS_MAX));
				break;
			case 2:
				transform.position = new Vector2(Random.Range(-X_POS_MAX, X_POS_MAX), Y_POS_MAX);
				break;
			case 3:
				transform.position = new Vector2(Random.Range(-X_POS_MAX, X_POS_MAX), -Y_POS_MAX);
				break;
		}
	}

	void Update() {
		if (health <= 0) {
			if (deathParticles != null)
			{
                Instantiate(deathParticles, transform.position, Quaternion.identity);
            }
            Kill();
		}

		if (isInteractable && isSelected && Input.GetKeyDown("space")) {
            isActivated = true;
            PlayerActivate();
        }

		// Monkey deselect/select
		if (Input.GetMouseButtonDown(0)) {
            isSelected = isHovered;
		}
		DefaultBehaviour();

		// Borders
		if (borderRenderers.Length != 0)
		{
            if (isSelected)
            {
                if (isActivated || isZoneActivated)
                {
                    for (int i = 0; i < borderRenderers.Length; i++)
                    {
                        borderRenderers[i].enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < borderRenderers.Length; i++)
                    {
                        borderRenderers[i].enabled = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < borderRenderers.Length; i++)
                {
                    borderRenderers[i].enabled = false;
                }
            }
		}
    }

	public abstract void DefaultBehaviour();

	public abstract void PlayerActivate();

	public abstract void ZoneActivate();

	public void SetZoneActivated()
	{
		isZoneActivated = true;
	}

	void Kill() {
        gameRef.onEnemyKill(this);
    }
	public virtual void SetTarget(Vector3 target) {
		// set direction
		Vector2 deltaPos = target - transform.position;
		unitVel = deltaPos / deltaPos.magnitude;

		Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * deltaPos;
		Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
		transform.rotation = targetRotation;
	}

	void OnTriggerEnter2D(Collider2D collision) {
		collisions.Add(collision);
		CheckCollisions();
	}

	void OnTriggerExit2D(Collider2D collision) {
		collisions.Remove(collision);
	}

	public virtual void CheckCollisions() {
		foreach (Collider2D collision in collisions) {
			if (collision.tag == "Scrambling")
			{
				isInteractable = false;
				isSelected = false;
			} else if (collision.tag == "Danger" || collision.tag == "Player") {
				health -= 1;
				Debug.Log(collision.gameObject);
			} else if (isInteractable && collision.gameObject.tag == "Selection") {
				isSelected = true;
			} else if (collision.gameObject.tag == "Death") {
				health = 0;
			} else {
				Debug.Log(collision);
			}
		}
	}

	public void OnMouseEnter() {
		isHovered = true;
	}

	public void OnMouseExit() {
		isHovered = false;
	}
}