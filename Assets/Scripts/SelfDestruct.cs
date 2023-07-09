using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

	public float lifespan;
	[SerializeField]
	private float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
		timeAlive = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
		timeAlive += Time.deltaTime;
        if (timeAlive >= lifespan)
		{
			Destroy(this.gameObject);
		}
    }
}
