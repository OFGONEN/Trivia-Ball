/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Spawner : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared" ) ] public Pool_Ball pool_ball;
    [ BoxGroup( "Shared" ) ] public SharedReferenceNotifier notifier_bar_transform;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void SpawnBall( BallSpawnEvent spawnEvent ) 
    {
		//todo handle spawnEvent.letterCount

		var ball = pool_ball.GetEntity();
		ball.Spawn( transform.position, spawnEvent.direction, 3, 1, spawnEvent.color );

		var position_bar = ( notifier_bar_transform.sharedValue as Transform ).position;

		var offset = GameSettings.Instance.bar_width / 2f * Vector3.right;
		var target = Vector3.Lerp( position_bar - offset, position_bar + offset, Random.Range( 0f, 1f ) );

		ball.transform.LookAtAxis( target, Vector3.up );

		ball.Launch();
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
