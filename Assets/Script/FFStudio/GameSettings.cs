﻿/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
#region Singleton Related
        private static GameSettings instance;

        private delegate GameSettings ReturnGameSettings();
        private static ReturnGameSettings returnInstance = LoadInstance;

		public static GameSettings Instance => returnInstance();
#endregion
        
#region Fields
        [ HideInInspector ] public int maxLevelCount;

	// Level Design
		[ BoxGroup( "Dsgn Ball" ) ] public int ball_spawn_letterCount;
		[ BoxGroup( "Dsgn Ball" ) ] public Vector3 ball_spawn_offset;
		[ BoxGroup( "Dsgn Ball" ) ] public Color ball_targetColor;
		[ BoxGroup( "Dsgn Ball" ) ] public float ball_launch_power;
		// [ BoxGroup( "Dsgn Ball" ) ] public float ball_launch_power_torque;
		[ BoxGroup( "Dsgn Ball" ) ] public Color ball_player_color;
		[ BoxGroup( "Dsgn Ball" ) ] public Color ball_enemy_color;
		[ BoxGroup( "Dsgn Ball" ) ] public float ball_default_power;
		[ BoxGroup( "Dsgn Ball" ) ] public int ball_default_health;
	// Game Settings
		[ BoxGroup( "Game" ), MinMaxSlider( 3, 10 ) ] public Vector2Int ball_currency;
		[ BoxGroup( "Game" ), MinMaxSlider( 0.25f, 4f ) ] public Vector2 ball_currency_textSize;
        [ BoxGroup( "Game" ) ] public float bar_width;
        [ BoxGroup( "Game" ) ] public float board_length;
        [ BoxGroup( "Game" ) ] public float board_reposition_duration;
        [ BoxGroup( "Game" ) ] public Ease board_reposition_ease;
		[ BoxGroup( "Game" ) ] public float ai_answer_cooldown;
		[ BoxGroup( "Game" ) ] public int keyboard_max_characterLimit;

		[ BoxGroup( "UI Flash Color" ) ] public Color ui_flashColor;
		[ BoxGroup( "UI Flash Color" ) ] public float ui_flashColor_duration_in;
		[ BoxGroup( "UI Flash Color" ) ] public float ui_flashColor_duration_out;
		[ BoxGroup( "UI Flash Color" ) ] public Ease ui_flashColor_ease_in;
		[ BoxGroup( "UI Flash Color" ) ] public Ease ui_flashColor_ease_out;

		[ FoldoutGroup( "Ball" ), Layer ] public int ball_spawn_layer;
		[ FoldoutGroup( "Ball" )        ] public bool ball_spawn_trigger;
		[ FoldoutGroup( "Ball" )        ] public float ball_spawn_delay = 0.2f;
		[ FoldoutGroup( "Ball" )        ] public Vector3 ball_spawn_punchScale_strength = Vector3.one;
		[ FoldoutGroup( "Ball" )        ] public float ball_player_direction;
		[ FoldoutGroup( "Ball" )        ] public float ball_enemy_direction;
		[ FoldoutGroup( "Ball" )        ] public Vector3 ball_punchScale;
		[ FoldoutGroup( "Ball" )        ] public float ball_punchScale_duration;
		[ FoldoutGroup( "Ball" )        ] public Ease ball_punchScale_ease;
		[ FoldoutGroup( "Ball" )        ] public float ball_jump_power;
		[ FoldoutGroup( "Ball" )        ] public float ball_jump_duration;
		[ FoldoutGroup( "Ball" )        ] public Ease ball_jump_ease;

        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_height;
        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_duration;

        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_wait_duration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Joy Stick"                                        ) ] public float ui_Entity_JoyStick_Gap;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text relative float height"                ) ] public float ui_PopUp_height;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text float duration"                       ) ] public float ui_PopUp_duration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;

        [ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_GameSettings;
        [ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_Components;
#endregion

#region Implementation
        private static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "game_settings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		private static GameSettings ReturnInstance()
        {
            return instance;
        }
#endregion


#region EditoyrOnly
#if UNITY_EDITOR
		private void OnValidate()
		{
			ball_targetColor.SetAlpha( 1 );
			ball_player_color.SetAlpha( 1 );
			ball_enemy_color.SetAlpha( 1 );
		}
#endif
#endregion
    }
}
