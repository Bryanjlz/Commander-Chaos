using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicWaveData", menuName = "ScriptableObjects/WaveData (Basic)", order = 2)]
public class BasicWaveData : WaveData<BasicWaveData>
{
	[Header("Random wave spawner (like Bryan's original implementation)")]

	[Tooltip("In Seconds")]
	public float levelLength;

	public List<int> numEnemies;
}
