using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
	[Tooltip("Low-intensity waves that constantly spawn stuff")]
	public List<IWave> passiveWaves;
	public List<IWave> inUsePassiveWaves;

	[Tooltip("Special waves that happen infrequently")]
	public List<IWave> activeWaves;
	public List<IWave> inUseActiveWaves;


	public GameController gc;

	public float timeStart;

	public float timeBetweenPassiveWaves;
	public float timeBetweenActiveWaves;

	public int passiveWeightSum;
	public int activeWeightSum;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine("CheckWaves");
		StartCoroutine("PassiveWave");
		StartCoroutine("ActiveWave");

		timeStart = Time.time;
	}

	IEnumerator CheckWaves() {
		while (true) {
			float deltaTime = Time.time - timeStart;

			// Remove expired waves
			for (int i = inUsePassiveWaves.Count - 1; i >= 0; i--) {
				IWave currentWave = inUsePassiveWaves[i];
				if (deltaTime > currentWave.maxTime) {
					passiveWeightSum -= currentWave.weight;
					inUsePassiveWaves.RemoveAt(i);
					print(deltaTime);
				}
			}
			for (int i = inUseActiveWaves.Count - 1; i >= 0; i--) {
				IWave currentWave = inUseActiveWaves[i];
				if (deltaTime > currentWave.maxTime) {
					activeWeightSum -= currentWave.weight;
					inUseActiveWaves.RemoveAt(i);
					print(deltaTime);
				}
			}

			// Check for new waves
			for (int i = passiveWaves.Count - 1; i >= 0; i--) {
				IWave currentWave = passiveWaves[i];
				if (deltaTime > currentWave.minTime && (deltaTime < currentWave.maxTime || currentWave.maxTime == 0) && !inUsePassiveWaves.Contains(currentWave)) {
					passiveWeightSum += currentWave.weight;
					inUsePassiveWaves.Add(currentWave);
				}
			}
			for (int i = activeWaves.Count - 1; i >= 0; i--) {
				IWave currentWave = activeWaves[i];
				if (deltaTime > currentWave.minTime && (deltaTime < currentWave.maxTime || currentWave.maxTime == 0) && !inUseActiveWaves.Contains(currentWave)) {
					activeWeightSum += currentWave.weight;
					inUseActiveWaves.Add(currentWave);
				}
			}

			yield return new WaitForSeconds(1f);
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
			foreach (IWave wave in inUsePassiveWaves)
			{
				if (r < tmp + wave.weight)
				{
					wave.Spawn(gc);
					return;
				}
				tmp += wave.weight;
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
			foreach (IWave wave in inUseActiveWaves)
			{
				if (r < tmp + wave.weight)
				{
					wave.Spawn(gc);
					return;
				}
				tmp += wave.weight;
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
