using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class Enemy : MonoBehaviour {
	// For spawning
	private readonly float X_POS_MAX = 19;
	private readonly float Y_POS_MAX = 11;

	// Set in editor
	[SerializeField]
	protected bool isInteractable;
	[SerializeField]
	protected int health;
	[SerializeField]
	protected CinemachineImpulseSource impulseSource;

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
	public bool isScrambled = false;

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

    [SerializeField]
    protected SpriteRenderer[] spriteRenderers;
	protected Color originalColor;

    public void Start()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColor = spriteRenderers[i].color;
        }
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

		// Borders and interactibility
		BorderChange();
    }

	public virtual void BorderChange()
	{
        if (borderRenderers.Length != 0)
        {
            if (!isInteractable || isScrambled)
            {
                for (int i = 0; i < borderRenderers.Length; i++)
                {
                    Color.RGBToHSV(spriteRenderers[i].color, out float H, out float S, out float V);
                    spriteRenderers[i].color = Color.HSVToRGB(H, 0.75f, 0.75f);
                    borderRenderers[i].color = new Color(1f, 0.25f, 0.25f);
                    borderRenderers[i].enabled = true;
                }
            }
            else if (isSelected)
            {
                for (int i = 0; i < borderRenderers.Length; i++)
                {
                    Color.RGBToHSV(spriteRenderers[i].color, out float H, out float S, out float V);
                    spriteRenderers[i].color = originalColor;
                    borderRenderers[i].color = Color.white;
                    borderRenderers[i].enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < borderRenderers.Length; i++)
                {
                    Color.RGBToHSV(spriteRenderers[i].color, out float H, out float S, out float V);
                    spriteRenderers[i].color = originalColor;
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

	protected virtual void Kill() {
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

	public void OnTriggerEnter2D(Collider2D collision) {
		collisions.Add(collision);
		CheckCollisions();
	}

	void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Scrambling")
		{
            isScrambled = false;
        }

        collisions.Remove(collision);
	}

	public virtual void CheckCollisions() {
		foreach (Collider2D collision in collisions) {
			if (collision.tag == "Scrambling")
			{
				isSelected = false;
                isScrambled = true;
            } else if (collision.tag == "Danger" || collision.tag == "Player") {
				health -= 1;
				Debug.Log(collision.gameObject);
			} else if (!isScrambled && isInteractable && collision.gameObject.tag == "Selection") {
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