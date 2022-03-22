/* Created by and for usage of FF Studios (2021). */
using UnityEngine;
using Sirenix.OdinInspector;

namespace FFStudio
{
	/* This class holds references to ScriptableObject assets. These ScriptableObjects are singletons, so they need to load before a Scene does.
	 * Using this class ensures at least one script from a scene holds a reference to these important ScriptableObjects. */
	public class AssetManager : MonoBehaviour
	{
#region Fields
		public GameSettings gameSettings;
		public CurrentLevelData currentLevelData;


		[ BoxGroup( "Pool" ) ] public Pool_UIPopUpText pool_UIPopUpText;
		[ BoxGroup( "Pool" ) ] public Pool_Ball pool_ball;

		private void Awake()
		{
			pool_UIPopUpText.InitPool( transform, true );
			pool_ball.InitPool( transform, true );
		}
#endregion
	}
}