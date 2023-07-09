using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData", order = 2)]
public class ScriptableObjectWaveData: WaveData<ScriptableObjectWaveData>
{

}


public abstract class WaveData<T> : IWave where T : IWave
{
	[Tooltip("Script to manage the wave's spawns")]
	public GameObject director;

	public override void Spawn(GameController gc)
	{
		director.GetComponent<WaveDirector<T>>().SpawnWith(gc, this as T);
	}
}

public abstract class IWave: ScriptableObject
{

	[Tooltip("Relative probability that this wave will be chosen")]
	public int weight;

	[Tooltip("Minimum time between instances of this wave")]
	public float cooldown;

	[Tooltip("Minimum time in game before this wave can be chosen")]
	public float minTime;

	[Tooltip("Maximum time in game time before this wave stops being chosen")]
	public float maxTime;

	public abstract void Spawn(GameController gc);
}
