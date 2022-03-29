/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;

namespace FFStudio
{
    [ CreateAssetMenu( fileName = "event_", menuName = "FF/Event/StringGameEvent" ) ]
    public class StringGameEvent : GameEvent
    {
        public string eventValue;

        public void Raise( string value )
        {
			eventValue = value;
			Raise();
		}

        [ Button() ]
        public void RaiseAsKeyboard()
        {
			eventValue = eventValue.RemoveChar( ' ' ).ToLower();
			Raise();
		}
    }
}