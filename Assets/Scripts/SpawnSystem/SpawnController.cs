using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
	[Tooltip("Set all of the below in the editor :)")]
	public List<IWave> waves;

	public GameController gc;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine("aaa");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void SpawnWave(IWave wave)
	{
		wave.Spawn(gc);
	}

	IEnumerator aaa()
	{
		while (true)
		{
			SpawnWave(waves[0]);
			yield return new WaitForSeconds(1);
			SpawnWave(waves[1]);
			yield return new WaitForSeconds(1);
		}
	}
}
