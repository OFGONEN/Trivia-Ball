/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Spawner : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared" ) ] public Pool_Ball pool_ball;
    [ BoxGroup( "Shared" ) ] public SharedReferenceNotifier notifier_bar_transform;

	private RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void BallSpawnEventResponse( BallSpawnEvent spawnEvent ) 
    {
		var direction = spawnEvent.direction;
		var power     = spawnEvent.power;
		var count     = spawnEvent.spawnCount;
		var color     = spawnEvent.color;

		var sequence = recycledSequence.Recycle();

		for( var i = 0; i < count; i++ )
		{
			sequence.AppendCallback( () => SpawnBall( direction, power, color ) );
			sequence.AppendInterval( GameSettings.Instance.ball_spawn_delay );
		}
	}

	public void LevelFinishedResponse()
	{
		recycledSequence.Kill();
	}
#endregion

#region Implementation
	private void SpawnBall( float direction, float power, Color color )
	{
		var ball = pool_ball.GetEntity();
		ball.Spawn( transform.position, direction, power, color );

		var position_bar = ( notifier_bar_transform.sharedValue as Transform ).position;

		var offset = GameSettings.Instance.bar_width / 2f * Vector3.right;
		var target = Vector3.Lerp( position_bar - offset, position_bar + offset, Random.Range( 0f, 1f ) );

		ball.transform.LookAtAxis( target, Vector3.up );
		ball.Launch();
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
