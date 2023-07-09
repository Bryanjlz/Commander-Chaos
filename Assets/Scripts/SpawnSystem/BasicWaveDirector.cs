using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Janky copy paste I know
public class BasicWaveDirector: WaveDirector<BasicWaveData>
{
	GameController gc;
	bool flag;
	float waveLen = 10.0f;

	public override void SpawnWith(GameController gc, BasicWaveData wave) {
		Player player = gc.player;
		this.gc = gc;
		this.waveLen = wave.levelLength;
		float timeStart = Time.time;
		List<Enemy> currentEnemies = gc.currentEnemies;
		List<(float time, EnemyType enemyType)> enemyTimeline = gc.enemyTimeline;
		timeStart = Time.time;

		if (enemyTimeline == null || currentEnemies == null)
		{
			return;
		}


		List<int> numEnemies = new List<int>(wave.numEnemies);

		int totalNumEnemies = 0;
		foreach (int i in numEnemies)
		{
			totalNumEnemies += i;
		}
		gc.enemiesLeft = totalNumEnemies;

		float baseTimeSpacing = wave.levelLength / (totalNumEnemies + 1);
		float currentTime;

		while (totalNumEnemies != 0)
		{
			// Get random enemy type that remains
			int enemyType = Random.Range(0, numEnemies.Count);
			while (numEnemies[enemyType] == 0)
			{
				if (enemyType == numEnemies.Count - 1)
				{
					enemyType = 0;
				} else
				{
					enemyType++;
				}
			}
			totalNumEnemies--;
			numEnemies[enemyType]--;

			// Get timing
			currentTime = baseTimeSpacing * totalNumEnemies + Random.Range(-baseTimeSpacing * 1.5f, baseTimeSpacing * 1.5f);
			if (currentTime < 0)
			{
				currentTime = 0;
			}
			if (currentTime > wave.levelLength)
			{
				currentTime = wave.levelLength;
			}

			// Add to timeline making sure it's in order
			bool foundTimingSpot = false;
			for (int i = 0; i < enemyTimeline.Count && !foundTimingSpot; i++)
			{
				if (currentTime < enemyTimeline[i].time)
				{
					enemyTimeline.Insert(i, (currentTime, (EnemyType)enemyType));
					foundTimingSpot = true;
				}
			}
			if (!foundTimingSpot)
			{
				enemyTimeline.Add((currentTime, (EnemyType)enemyType));
			}
		}

	}
}
