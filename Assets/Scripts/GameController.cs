using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	// References to everything
	public Player player;
	public List<Enemy> enemies;
	public GameObject gameOverScreen;

	public int enemiesLeft;

	void Start() {
		if (enemies == null) {
			enemies = new List<Enemy>();
		}
		// set everything's gameref to this, just in case
		player.gameRef = this;
		foreach (Enemy e in enemies) {
			e.gameRef = this;
		}
	}

	void Update() {
	}

	public void onPlayerKill() {
		gameOverScreen.SetActive(true);
	}

	public void onEnemyKill(Enemy e) {
		enemies.Remove(e);
		Destroy(e.gameObject);
		--enemiesLeft;
		if (enemiesLeft == 0) {
			// Win the game or level if we're doing that
		}
	}
}
