/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Ball : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared" ) ] public Pool_Ball pool_ball;

    // Private Fields \\
    private int ball_health;
    private int ball_health_current;
    private float ball_direction;
    private float ball_power;
    private Color ball_color;
    
    // Components
    private Rigidbody ball_rigidbody;
    private Collider ball_collider;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
        ball_rigidbody = GetComponent< Rigidbody >();
        ball_collider  = GetComponent< Collider >();
    }
#endregion

#region API
    public void Spawn( Vector3 position, float direction, int health, float power, Color color )
    {
		transform.position    = position;
		transform.eulerAngles = Vector3.forward * direction;

		gameObject.layer        = GameSettings.Instance.ball_spawn_layer;
		ball_collider.isTrigger = GameSettings.Instance.ball_spawn_trigger;

		ball_health         = health;
		ball_health_current = health;
		ball_power          = power;
		ball_direction      = direction;
		ball_color          = color;
	}

    public void OnCollision_Bar( Bar bar )
    {
		ball_health_current -= 1;

		var newColor = Color.Lerp( ball_color,
			GameSettings.Instance.ball_health_color,
			Mathf.InverseLerp( ball_health, 0, ball_health_current ) 
        );

		bar.Push( ball_direction * ball_power );

        if( ball_health_current <= 0 )
            DeSpawn();
	}
#endregion

#region Implementation
    private void DeSpawn()
    {
		ball_rigidbody.velocity = Vector3.zero;
		ball_rigidbody.angularVelocity = Vector3.zero;

		pool_ball.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
