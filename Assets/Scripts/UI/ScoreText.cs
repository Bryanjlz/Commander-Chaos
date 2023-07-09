using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
	[SerializeField]
	TMP_Text text;

	// Update is called once per frame
	void Update() {
		text.text = "SCORE: " + LevelTracker.self.score;
	}
}
