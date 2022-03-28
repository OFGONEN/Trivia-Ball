/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace FFStudio
{
	[ CreateAssetMenu( fileName = "LevelData", menuName = "FF/Data/LevelData" ) ]
	public class LevelData : ScriptableObject
    {
		[ BoxGroup( "Setup" ), ValueDropdown( "SceneList" ), LabelText( "Scene Index" ) ] public int scene_index;
        [ BoxGroup( "Setup" ), LabelText( "Override As Active Scene" ) ] public bool scene_overrideAsActiveScene;

        [ BoxGroup( "Level Dsgn" ) ] public bool showIncremantal;
        [ BoxGroup( "Level Dsgn" ) ] public float currency_cofactor = 1f;
        [ BoxGroup( "Level Dsgn" ) ] public string question;
        [ BoxGroup( "Level Dsgn" ) ] public string[] question_answers;

        [ BoxGroup( "AI" ), MinMaxSlider( 0, 10f ) ] public Vector2 ai_answer_rate;
        [ BoxGroup( "AI" ), Range( 0, 5 ) ]  public float ai_ball_power = 1f;
        [ BoxGroup( "AI" ), Range( 0, 10 ) ] public int ai_ball_health  = 2;
        [ BoxGroup( "Bar" ) ] public float bar_movement_drag = 1f;

		private Dictionary< int, string > question_answers_dictionary;

		public void InitAnswerDictionay()
		{
			if( question_answers_dictionary == null )
			{
				question_answers_dictionary = new Dictionary< int, string >( question_answers.Length );

				for( var i = 0; i < question_answers.Length; i++ )
				{
					question_answers_dictionary.Add( question_answers[ i ].GetHashCode(), question_answers[ i ] );
				}
			}
		}

		public bool CheckIfCorrectAnswer( int hashCode )
		{
			return question_answers_dictionary.ContainsKey( hashCode );
		}

#if UNITY_EDITOR
		private static IEnumerable SceneList()
        {
			var list = new ValueDropdownList< int >();

			var scene_count = SceneManager.sceneCountInBuildSettings;

			for( var i = 0; i < scene_count; i++ )
				list.Add( Path.GetFileNameWithoutExtension( SceneUtility.GetScenePathByBuildIndex( i ) ) + $" ({i})", i );

			return list;
		}

		[ Button() ]
		private void LogAnswers()
		{
			for( var i = 0; i < question_answers.Length; i++ )
			{
				FFLogger.Log( question_answers[ i ] , this );
			}
		}
#endif
    }
}
