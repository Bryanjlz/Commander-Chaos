using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDirector<WaveType>: MonoBehaviour
{
	public virtual void SpawnWith(GameController gc, WaveType wave) { }
}
