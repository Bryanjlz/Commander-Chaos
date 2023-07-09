using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShower : WaveDirector<ScriptableObjectWaveData>
{
	// For spawning
	private readonly float X_POS_MAX = 19;
	private readonly float Y_POS_MAX = 11;

	public override void SpawnWith(GameController gc, ScriptableObjectWaveData wave)
	{
		for (int i = 0; i < 5; ++i)
		{
			gc.SpawnEnemy(EnemyType.ASTEROID, X_POS_MAX, Random.Range(-Y_POS_MAX, Y_POS_MAX));
		}
	}

	public void Update()
	{
		Destroy(this);
	}
}
