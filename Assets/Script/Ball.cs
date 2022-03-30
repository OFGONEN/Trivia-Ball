/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.Events;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Ball : MonoBehaviour
{
#region Fields
[ TitleGroup( "Shared" ) ]
    public SharedIntNotifier notifier_currency;
    public Pool_Ball pool_ball;
    public Pool_UIPopUpText pool_popUpText;

[ TitleGroup( "Setup" ) ] 
	public Transform transform_gfx;
	public TrailRenderer trailRenderer;
	public UnityEvent onDespawn_forPlayer;
	public UnityEvent onDespawn_forEnemy;

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

	private RecycledTween tween_punchScale = new RecycledTween();

	private Vector3 ball_start_Size;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnDisable()
	{
		tween_punchScale.Kill();
		trailRenderer.enabled = false;
	}

    private void Awake()
    {
		ball_rigidbody    = GetComponent< Rigidbody >();
        ball_collider     = GetComponent< Collider >();
        ball_color_setter = GetComponentInChildren< ColorSetter >();

		ball_start_Size = transform_gfx.localScale;

		trailRenderer.enabled = false;
	}
#endregion

#region API
    public void Spawn( bool currency, Vector3 position, float direction, float power, int health, Color color )
    {
		gameObject.SetActive( true );
		trailRenderer.Clear();
		trailRenderer.enabled = true;
		transform_gfx.localScale = ball_start_Size;

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
        // ball_rigidbody.AddTorque( Random.onUnitSphere * GameSettings.Instance.ball_launch_power_torque, ForceMode.Impulse );
	}

	public void OnCollision( Collision collision )
	{
		tween_punchScale.Recycle(
			transform_gfx.DOPunchScale( GameSettings.Instance.ball_punchScale,
			GameSettings.Instance.ball_punchScale_duration )
			.SetEase( GameSettings.Instance.ball_punchScale_ease )
		);
	}

    public bool OnCollision_Bar( Bar bar )
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
		{
			DeSpawn();
			if( ball_currency )
				onDespawn_forPlayer.Invoke();
			else
				onDespawn_forEnemy.Invoke();
		}

		return ball_currency;
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