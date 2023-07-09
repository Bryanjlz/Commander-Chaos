using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShower : WaveDirector<ScriptableObjectWaveData>
{
	public override void SpawnWith(GameController gc, ScriptableObjectWaveData wave)
	{
		for (int i = 0; i < 10; ++i)
		{
			gc.SpawnEnemy(EnemyType.ASTEROID);
		}
	}

	public void Update()
	{
		Destroy(this);
	}
}
