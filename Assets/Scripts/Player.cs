using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public GameController gameRef;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
	}

	void Kill()
	{
		gameRef.onPlayerKill();
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		this.Kill();
	}
}
