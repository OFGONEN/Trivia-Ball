/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using FFStudio;
using Sirenix.OdinInspector;

namespace FFEditor 
{
[ CreateAssetMenu( fileName = "QuestionExtract", menuName = "FFEditor/QuestionExtract" ) ]
    public class QuestionExtract : ScriptableObject
	{
		public TextAsset question_text;
		public List< QuestionData > question_datas = new List< QuestionData >();
		public LevelData levelData;

		[ Button() ]
		private void ExtractToLevelData( int questionIndex )
		{
			EditorUtility.SetDirty( levelData );

			questionIndex = Mathf.Clamp( questionIndex, 0, question_datas.Count - 1 );

			levelData.question = question_datas[ questionIndex ].question;
			levelData.question_answers = question_datas[ questionIndex ].question_answers.ToArray();

			for( var i = 0; i < levelData.question_answers.Length; i++ )
			{
				levelData.question_answers[ i ] = levelData.question_answers[ i ].RemoveChar( ' ' ).ToLower();
			}

			AssetDatabase.SaveAssets();
		}

		[ Button() ]
        public void ExtractData()
        {
			question_datas.Clear();

			var text = question_text.text;
			int index = 0;

			var stringBuilder = new StringBuilder( 128 );

			QuestionData currentQuestionData = new QuestionData();

			while( true )
			{
				var nextInline = text.IndexOf( '\n', index );

				if( nextInline <= index )
					return;

				var subString = text.Substring( index, nextInline - index );
				if( subString[ 0 ] == '#' ) //Question Line
				{
					currentQuestionData = new QuestionData();
					currentQuestionData.question = subString.Remove( 0, 1 );
					currentQuestionData.question_answers = new List< string >();
					question_datas.Add( currentQuestionData );
				}
				else
					currentQuestionData.question_answers.Add( subString );

				index = nextInline + 1;

				if( index >= text.Length )
					return;
			}
		}

        [ Button() ]
        public void LogInfo()
        {
            //TOTAL ANSWER
			var totalAnswerCount = 0;

            for( var i = 0; i < question_datas.Count; i++ )
            {
				totalAnswerCount += question_datas[ i ].question_answers.Count;
			}

            FFLogger.Log( $"Total Question: {question_datas.Count}\nTotal Answer Count: {totalAnswerCount}" );

            // MOST LONG QUESTION
			var mostLongQuestion = string.Empty;
			var mostLongQuestion_Count = 0;
			var mostLongQuestion_Index = 0;

            for( var i = 0; i < question_datas.Count; i++ )
            {
                if( question_datas[ i ].question.Length > mostLongQuestion_Count )
                {
					mostLongQuestion = question_datas[ i ].question;
					mostLongQuestion_Count = mostLongQuestion.Length;
			        mostLongQuestion_Index = i;
				}
			}

            FFLogger.Log( $"Most Long Question: {mostLongQuestion}\nLetter Count: {mostLongQuestion_Count}" );

            // MOST LONG ANSWER

			var mostLongAnswer = string.Empty;
			var mostLongAnswerQuestion = string.Empty;
			var mostLongAnswer_Count = 0;
			var mostLongAnswer_Index = 0;
			var mostLongAnswerQuestion_Index = 0;

            for( var x = 0; x < question_datas.Count; x++ )
            {
				var data = question_datas[ x ];

				for( var y = 0; y < data.question_answers.Count; y++ )
                {
					if( data.question_answers[ y ].Length > mostLongAnswer_Index )
					{
						mostLongAnswer = data.question_answers[ y ];
			            mostLongAnswerQuestion = data.question;
						mostLongAnswer_Count = mostLongAnswer.Length;
						mostLongAnswer_Index = y;
			            mostLongAnswerQuestion_Index = x;
					}
				}

			}

            FFLogger.Log( $"Question of {mostLongAnswerQuestion} Index: {mostLongAnswerQuestion_Index}" );
            FFLogger.Log( $"Most Long Answer: {mostLongAnswer}\nLetter Count: {mostLongAnswer_Count}" );
		}

		[ Button( "SetLevelData" ) ]
		public void UpdateLevelData()
		{
			for( var i = 0; i < question_datas.Count; i++ )
			{
				var levelData = Resources.Load< LevelData >( "level_data_" + ( i + 1 ) );
				if( levelData )
				{
					this.levelData = levelData;
					ExtractToLevelData( i );
				}
				else
					break;
			}

			AssetDatabase.SaveAssets();
		}
	}

    [ System.Serializable ]
	public struct QuestionData
	{
		public string question;
		public List< string > question_answers;
	}
}