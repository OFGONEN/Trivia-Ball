/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

namespace FFEditor 
{
[ CreateAssetMenu( fileName = "QuestionExtract", menuName = "FFEditor/QuestionExtract" ) ]
    public class QuestionExtract : ScriptableObject
	{
		public TextAsset question_text;
		public List< QuestionData > question_datas = new List< QuestionData >();

		[ Button() ]
        public void ExtractData()
        {
			question_datas.Clear();

			var text = question_text.text;

			var index = 1;
			var hasQuestion = true;

			while( hasQuestion  )
            {
				var data = new QuestionData();
				data.question_answers = new List< string >();

				var inline_question = text.IndexOf( '\n', index );
				data.question = text.Substring( index , inline_question - index );

				index = inline_question + 1;

				var sameQuestion = true;

				while( sameQuestion )
                {
					var inline_answer = text.IndexOf( '\n', index );

                    if( inline_answer < 0 )
						return;
					data.question_answers.Add( text.Substring( index, inline_answer - index ) );

                    if( text.Length - 1 > inline_answer + 1 && text[ inline_answer + 1 ] == '#' )
						sameQuestion = false;
                    else
						index = inline_answer + 1;
				}

				question_datas.Add( data );

				var nextQuestion = text.IndexOf( '#', index );

                if( nextQuestion < 0 )
					return;
                else
					index = nextQuestion + 1;

				LogInfo();
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

            FFLogger.Log( $"Most Long Answer: {mostLongAnswer}\nLetter Count: {mostLongAnswer_Count}" );
            FFLogger.Log( $"\nQuestion of {mostLongAnswerQuestion} Index: {mostLongAnswerQuestion_Index}" );
		}
	}

    [ System.Serializable ]
	public struct QuestionData
	{
		public string question;
		public List< string > question_answers;
	}
}