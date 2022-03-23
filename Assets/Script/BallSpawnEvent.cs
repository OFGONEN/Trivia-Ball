/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "event_ballSpawn_", menuName = "FF/Event/Ball Spawn" ) ]
public class BallSpawnEvent : GameEvent
{
	public float direction;
	public int letterCount;
	public Color color;

	public void Raise( float direction, int letterCount, Color color )
	{
		this.direction   = direction;
		this.letterCount = letterCount;
		this.color       = color;
	}
}
