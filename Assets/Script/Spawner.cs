/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.Events;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Spawner : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared" ) ] public Pool_Ball pool_ball;
    [ BoxGroup( "Shared" ) ] public SharedReferenceNotifier notifier_ballTarget_transform;
    [ BoxGroup( "Shared" ) ] public UnityEvent onSingleBallSpawn;

	private RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void BallSpawnEventResponse( BallSpawnEvent spawnEvent ) 
    {
		var currency  = spawnEvent.currency;
		var direction = spawnEvent.direction;
		var power     = spawnEvent.power;
		var count     = spawnEvent.spawnCount;
		var health    = spawnEvent.health;
		var color     = spawnEvent.color;

		var sequence = recycledSequence.Recycle();

		for( var i = 0; i < count; i++ )
		{
			sequence.AppendCallback( onSingleBallSpawn.Invoke );
			sequence.AppendCallback( () => SpawnBall( currency, direction, power, health, color ) );
			sequence.Append( transform
								.DOPunchScale( GameSettings.Instance.ball_spawn_punchScale_strength, GameSettings.Instance.ball_spawn_delay, 15 )
								.SetEase( Ease.InOutElastic ) );
			sequence.AppendInterval( GameSettings.Instance.ball_spawn_delay );
		}
	}

	public void LevelFinishedResponse()
	{
		recycledSequence.Kill();
	}
#endregion

#region Implementation
	private void SpawnBall( bool currency, float direction, float power, int health, Color color )
	{
		var targetTransform = notifier_ballTarget_transform.sharedValue as Transform;
		var targetTransform_position = targetTransform.position;

		var offset = GameSettings.Instance.bar_width / 2f * Vector3.right;
		var target = Vector3.Lerp( targetTransform_position - offset, targetTransform_position + offset, Random.Range( 0f, 1f ) );

		var ball = pool_ball.GetEntity();
		ball.Spawn( target, currency, transform.position, direction, power, health, color );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
