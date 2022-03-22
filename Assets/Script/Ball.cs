/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Ball : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Spawn" ), Layer ] public int ball_spawn_layer;
    [ BoxGroup( "Spawn" ) ] public bool ball_spawn_trigger;

    // Private Fields \\
    private int ball_health;
    private float ball_power;
    
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

		gameObject.layer        = ball_spawn_layer;
		ball_collider.isTrigger = ball_spawn_trigger;

		ball_health = health;
		ball_power  = power;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
