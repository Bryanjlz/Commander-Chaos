using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTracker : MonoBehaviour {
	internal static LevelTracker self;

	public float timeStart;
	public int score;
	[SerializeField]
	public int scorePerSec;

	void OnEnable() {
		if (self != null) {
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		self = this;
	}

	public void BeginLevel() {
		StartCoroutine("TimeScore");
	}

	public void DoneLevel() {
		StopCoroutine("TimeScore");
	}

	public IEnumerator TimeScore() {
		while (true) {
			yield return new WaitForSeconds(1);
			Debug.Log("Active");
			score += scorePerSec;
		}
	}
}
