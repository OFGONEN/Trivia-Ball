/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
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
			}
		}
	}

    [ System.Serializable ]
	public struct QuestionData
	{
		public string question;
		public List< string > question_answers;
	}
}