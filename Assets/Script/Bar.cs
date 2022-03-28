/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Bar : MonoBehaviour
{
#region Fields

// Private Fields \\
    [ ShowInInspector, ReadOnly ] private float movement_force;
    [ ShowInInspector, ReadOnly ] private float movement_drag;


// Delegate
    private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		updateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
    public void StartMovement()
    {
		updateMethod = Movement;
		movement_drag = CurrentLevelData.Instance.levelData.bar_movement_drag;
	}

    public void StopMovement()
    {
		updateMethod = ExtensionMethods.EmptyMethod;
	}

    public void BallCollided( Collision collision )
    {
		var ball = collision.gameObject.GetComponentInParent< Ball >();

		if( ball ) //todo remove this if bot balls are removed from project
			ball.OnCollision_Bar( this );
	}

    public void Push( float force )
    {
		movement_force += force;
	}
#endregion

#region Implementation
    private void Movement()
    {
		var step = Time.deltaTime * movement_drag;

        if( step >= Mathf.Abs( movement_force ) )
			step = movement_force;

		movement_force = movement_force + step * Mathf.Sign( movement_force ) * -1f;

		transform.position += Vector3.forward * movement_force * Time.deltaTime;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
