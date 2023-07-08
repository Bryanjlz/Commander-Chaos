using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	// References to everything
	public Player player;
	public List<Enemy> enemies;

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
		// Popup a loss screen or something
		// For now, crash the game
		int x = 0;
		x /= 0;
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
