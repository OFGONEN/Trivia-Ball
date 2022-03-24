/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "event_ballSpawn_", menuName = "FF/Event/Ball Spawn" ) ]
public class BallSpawnEvent : GameEvent
{
	public float direction;
	public int spawnCount;
	public float power;
	public int health;
	public Color color;

	public void Raise( float direction, int spawnCount, float power, int health, Color color )
	{
		this.direction  = direction;
		this.spawnCount = spawnCount;
		this.power      = power;
		this.health     = health;
		this.color      = color;

		Raise();
	}
}
