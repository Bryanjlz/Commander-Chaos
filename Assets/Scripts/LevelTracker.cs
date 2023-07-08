using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTracker : MonoBehaviour {
	internal static LevelTracker self;

	public LevelData currentLevel;

	void OnEnable() {
		if (self != null) {
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		self = this;
	}
}
