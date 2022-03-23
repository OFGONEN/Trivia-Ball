/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace FFStudio
{
	[ CreateAssetMenu( fileName = "LevelData", menuName = "FF/Data/LevelData" ) ]
	public class LevelData : ScriptableObject
    {
		[ BoxGroup( "Setup" ), ValueDropdown( "SceneList" ), LabelText( "Scene Index" ) ] public int scene_index;
        [ BoxGroup( "Setup" ), LabelText( "Override As Active Scene" ) ] public bool scene_overrideAsActiveScene;

        [ BoxGroup( "Level Dsgn" ) ] public string question;
        [ BoxGroup( "Level Dsgn" ), HideInInspector ] public string[] question_answers;

        [ BoxGroup( "AI" ), MinMaxSlider( 0, 10f ) ] public Vector2 ai_answer_rate;

		private Dictionary< int, string > question_answers_dictionary;

		public void InitAnswerDictionay()
		{
			if( question_answers_dictionary == null )
			{
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
		[ ShowInInspector ] private List< string > question_answers_editor = new List< string >();

		private static IEnumerable SceneList()
        {
			var list = new ValueDropdownList< int >();

			var scene_count = SceneManager.sceneCountInBuildSettings;

			for( var i = 0; i < scene_count; i++ )
				list.Add( Path.GetFileNameWithoutExtension( SceneUtility.GetScenePathByBuildIndex( i ) ) + $" ({i})", i );

			return list;
		}

		private void OnValidate()
		{
			question_answers = new string[ question_answers_editor.Count ];

			for( var i = 0; i < question_answers_editor.Count; i++ )
			{
				question_answers[ i ] = question_answers_editor[ i ].RemoveChar( ' ' );
			}
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
