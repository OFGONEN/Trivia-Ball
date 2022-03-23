﻿/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
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
        [ BoxGroup( "Remote Config" ) ] public bool useRemoteConfig_GameSettings;
        [ BoxGroup( "Remote Config" ) ] public bool useRemoteConfig_Components;

        public int maxLevelCount;
        [ BoxGroup( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ BoxGroup( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ BoxGroup( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ BoxGroup( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
		[ BoxGroup( "UI Settings" ), Tooltip( "Joy Stick"                                        ) ] public float ui_Entity_JoyStick_Gap;
		[ BoxGroup( "UI Settings" ), Tooltip( "Pop Up Text relative float height"                ) ] public float ui_PopUp_height;
		[ BoxGroup( "UI Settings" ), Tooltip( "Pop Up Text float duration"                       ) ] public float ui_PopUp_duration;
        [ BoxGroup( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;

        [ BoxGroup( "Bar" ) ] public float bar_movement_drag;
        [ BoxGroup( "Bar" ) ] public float bar_width;

		[ BoxGroup( "Ball" ), Layer ] public int ball_spawn_layer;
		[ BoxGroup( "Ball" ) ] public bool ball_spawn_trigger;
		[ BoxGroup( "Ball" ) ] public Color ball_targetColor;
		[ BoxGroup( "Ball" ) ] public float ball_launch_power;
		[ BoxGroup( "Ball" ) ] public float ball_launch_power_torque;
		[ BoxGroup( "Ball" ) ] public float ball_player_direction;
		[ BoxGroup( "Ball" ) ] public Color ball_player_color;
		[ BoxGroup( "Ball" ) ] public float ball_enemy_direction;
		[ BoxGroup( "Ball" ) ] public Color ball_enemy_color;

		[ BoxGroup( "AI" ) ] public float ai_answer_cooldown;

		[ BoxGroup( "Keyboard" ) ] public int keyboard_max_characterLimit;

        [ BoxGroup( "Debug" ) ] public float debug_ui_text_float_height;
        [ BoxGroup( "Debug" ) ] public float debug_ui_text_float_duration;
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
