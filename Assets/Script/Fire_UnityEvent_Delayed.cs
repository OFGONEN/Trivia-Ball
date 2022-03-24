/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

namespace FFStudio
{
	public class Fire_UnityEvent_Delayed : MonoBehaviour
	{
#region Fields
        public EventPairDelayed[] eventPairs;
#endregion

#region Properties
#endregion

#region Unity API
		private void OnEnable()
		{
			for( var i = 0; i < eventPairs.Length; i++ )
				eventPairs[ i ].OnEnable();
		}

		private void OnDisable()
		{
			for( var i = 0; i < eventPairs.Length; i++ )
				eventPairs[ i ].OnDisable();
		}

		private void Awake()
		{
			for( var i = 0; i < eventPairs.Length; i++ )
				eventPairs[ i ].Pair();
		}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
	}
}