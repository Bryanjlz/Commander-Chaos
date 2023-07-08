using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject {
	[Tooltip("In Seconds")]
	public float levelLength;

	public List<int> numEnemies;
}
