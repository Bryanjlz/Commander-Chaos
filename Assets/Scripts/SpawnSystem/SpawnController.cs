using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
	[Tooltip("Low-intensity waves that constantly spawn stuff")]
	public List<IWave> passiveWaves;

	[Tooltip("Special waves that happen infrequently")]
	public List<IWave> activeWaves;


	public GameController gc;

	public float timeStart;

	public float timeBetweenPassiveWaves;
	public float timeBetweenActiveWaves;

	public int passiveWeightSum;
	public int activeWeightSum;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine("PassiveWave");
		StartCoroutine("ActiveWave");

		timeStart = Time.time;

		passiveWeightSum = 0;
		foreach (IWave wave in passiveWaves)
		{
			passiveWeightSum += wave.weight;
		}
		activeWeightSum = 0;
		foreach (IWave wave in activeWaves)
		{
			activeWeightSum += wave.weight;
		}
	}

	void SpawnWave(IWave wave)
	{
		wave.Spawn(gc);
	}

	void SelectPassiveWave()
	{
		int times = 0;
		while (times < 1000)
		{
			float r = Random.Range(0, passiveWeightSum);
			int tmp = 0;
			foreach (IWave wave in passiveWaves)
			{
				if (r < tmp + wave.weight)
				{
					wave.Spawn(gc);
					return;
				}
				tmp++;
			}
			++times;
		}
	}

	void SelectActiveWave()
	{
		int times = 0;
		while (times < 1000)
		{
			float r = Random.Range(0, activeWeightSum);
			int tmp = 0;
			foreach (IWave wave in activeWaves)
			{
				if (r < tmp + wave.weight)
				{
					wave.Spawn(gc);
					return;
				}
				tmp++;
			}
			++times;
		}
	}

	IEnumerator PassiveWave()
	{
		while (true)
		{
			yield return new WaitForSeconds(timeBetweenPassiveWaves);
			Debug.Log("Passive");
			SelectPassiveWave();
		}
	}

	IEnumerator ActiveWave()
	{
		while (true)
		{
			yield return new WaitForSeconds(timeBetweenActiveWaves);
			Debug.Log("Active");
			SelectActiveWave();
		}
	}
}
