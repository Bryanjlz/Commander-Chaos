using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	// References to everything
	public Player player;
	public LevelData level;
	public List<Enemy> currentEnemies;
	public List<(float time, EnemyType enemyType)> enemyTimeline;
	public List<GameObject> enemyPrefabs;
	public GameObject gameOverScreen;

	public int enemiesLeft;
	public float timeStart;

	void Start() {
		if (currentEnemies == null) {
			currentEnemies = new List<Enemy>();
		}
		enemyTimeline = new List<(float time, EnemyType enemyType)>();

		// set everything's gameref to this, just in case
		player.gameRef = this;

		timeStart = Time.time;
		LevelTracker.self.timeStart = timeStart;

		LevelTracker.self.BeginLevel();
	}

	// Turns Level Data into a timeline
	public void ParseLevelData() {
		List<int> numEnemies = new List<int>(level.numEnemies);

		int totalNumEnemies = 0;
		foreach (int i in numEnemies) {
			totalNumEnemies += i;
		}
		enemiesLeft = totalNumEnemies;

		float baseTimeSpacing = level.levelLength / (totalNumEnemies + 1);
		float currentTime;

		while (totalNumEnemies != 0) {
			// Get random enemy type that remains
			int enemyType = Random.Range(0, numEnemies.Count);
			while (numEnemies[enemyType] == 0) {
				if (enemyType == numEnemies.Count - 1) {
					enemyType = 0;
				} else {
					enemyType++;
				}
			}
			totalNumEnemies--;
			numEnemies[enemyType]--;

			// Get timing
			currentTime = baseTimeSpacing * totalNumEnemies + Random.Range(-baseTimeSpacing * 1.5f, baseTimeSpacing * 1.5f);
			if (currentTime < 0) {
				currentTime = 0;
			}
			if (currentTime > level.levelLength) {
				currentTime = level.levelLength;
			}

			// Add to timeline making sure it's in order
			bool foundTimingSpot = false;
			for (int i = 0; i < enemyTimeline.Count && !foundTimingSpot; i++) {
				if (currentTime < enemyTimeline[i].time) {
					enemyTimeline.Insert(i, (currentTime, (EnemyType)enemyType));
					foundTimingSpot = true;
				}
			}
			if (!foundTimingSpot) {
				enemyTimeline.Add((currentTime, (EnemyType)enemyType));
			}
		}
	}

	public void Update() {
		if (enemyTimeline.Count != 0 && Time.time - timeStart > enemyTimeline[0].time) {
			SpawnEnemy(enemyTimeline[0].enemyType);
			enemyTimeline.RemoveAt(0);
		}
	}

	public void SpawnEnemy(EnemyType enemyType, float x = 0, float y = 0) {
		GameObject enemyObj = Instantiate(enemyPrefabs[(int)enemyType]);

		Enemy enemy = enemyObj.GetComponent<Enemy>();
		if (x != 0 && y != 0)
		{
			enemy.SetForcedSpawn(new Vector2(x, y));
		}
		switch (enemyType)
		{
			// Add cases for special Setup calls
		}

		AddEnemy(enemy);
	}

	public void AddEnemy(Enemy enemy, float x = 0, float y = 0)
	{
		enemy.Setup(player, this);
		if (x != 0 && y != 0)
		{
			enemy.SetForcedSpawn(new Vector2(x, y));
		}
		currentEnemies.Add(enemy);
	}

	public void onPlayerKill() {
		LevelTracker.self.DoneLevel();
		gameOverScreen.SetActive(true);
	}

	public void onEnemyKill(Enemy e) {
		currentEnemies.Remove(e);
		Destroy(e.gameObject);
		--enemiesLeft;
		if (enemiesLeft == 0) {
			print("hurrahh");
			// Win the game or level if we're doing that
		}
	}
}
