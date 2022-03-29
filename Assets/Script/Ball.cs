/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Ball : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared" ) ] public SharedIntNotifier notifier_currency;
    [ BoxGroup( "Shared" ) ] public Pool_Ball pool_ball;
    [ BoxGroup( "Shared" ) ] public Pool_UIPopUpText pool_popUpText;

    // Private Fields \\
    private int ball_health;
    private int ball_health_current;
	private bool ball_currency;
	private float ball_direction;
    private float ball_power;
	private Color ball_color;
	private Color ball_color_current;
    
    // Components
    private Rigidbody ball_rigidbody;
    private Collider ball_collider;
    private ColorSetter ball_color_setter;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
        ball_rigidbody    = GetComponent< Rigidbody >();
        ball_collider     = GetComponent< Collider >();
        ball_color_setter = GetComponentInChildren< ColorSetter >();
	}
#endregion

#region API
    public void Spawn( bool currency, Vector3 position, float direction, float power, int health, Color color )
    {
		gameObject.SetActive( true );

		transform.position    = position;
		transform.eulerAngles = Vector3.forward * direction;

		gameObject.layer        = GameSettings.Instance.ball_spawn_layer;
		ball_collider.isTrigger = GameSettings.Instance.ball_spawn_trigger;

		ball_color_setter.SetColor( color );

		ball_currency       = currency;
		ball_health         = health;
		ball_health_current = health;
		ball_power          = power;
		ball_direction      = direction;
		ball_color_current  = color;
		ball_color          = color;
	}

    public void Launch()
    {
		ball_rigidbody.AddForce( transform.forward * GameSettings.Instance.ball_launch_power, ForceMode.Impulse);
        ball_rigidbody.AddTorque( Random.onUnitSphere * GameSettings.Instance.ball_launch_power_torque, ForceMode.Impulse );
	}

    public void OnCollision_Bar( Bar bar )
    {
		if( ball_currency )
		{
			var increase = Mathf.RoundToInt( GameSettings.Instance.ball_currency.ReturnRandom() * CurrentLevelData.Instance.levelData.currency_cofactor );
			notifier_currency.SharedValue += increase;

			var popUp = pool_popUpText.GetEntity();
			popUp.Spawn( transform.position, $"+{increase}", GameSettings.Instance.ball_currency_textSize.ReturnRandom(), ball_color.SetAlpha( 1 ) );
		}

		ball_health_current -= 1;

		var newColor = Color.Lerp( ball_color_current,
			GameSettings.Instance.ball_targetColor,
			Mathf.InverseLerp( ball_health, 0, ball_health_current ) 
        );

		ball_color_setter.SetColor( newColor );

		bar.Push( ball_direction * ball_power );

        if( ball_health_current <= 0 )
            DeSpawn();
	}

    public void LevelCompleteResponse()
    {
        ball_rigidbody.velocity        = Vector3.zero;
        ball_rigidbody.angularVelocity = Vector3.zero;

		pool_ball.ReturnEntity( this );
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