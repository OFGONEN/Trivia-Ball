/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
        [ Header( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedListener;
        public EventListenerDelegateResponse levelRevealedListener;
        public EventListenerDelegateResponse levelStartedListener;

        [ Header( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;
        public GameEvent event_player_answer_wrong;
        public BallSpawnEvent event_ballSpawn_player;
        public BallSpawnEvent event_ballSpawn_enemy;

        [ Header( "Level Releated" ) ]
        public SharedFloatNotifier levelProgress;

        // Private
        private Dictionary< int, string > player_answers = new Dictionary< int, string >( 64 );
#endregion

#region UnityAPI
        private void OnEnable()
        {
            levelLoadedListener.OnEnable();
            levelRevealedListener.OnEnable();
            levelStartedListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedListener.OnDisable();
            levelRevealedListener.OnDisable();
            levelStartedListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedListener.response   = LevelLoadedResponse;
            levelRevealedListener.response = LevelRevealedResponse;
            levelStartedListener.response  = LevelStartedResponse;
        }
#endregion

#region API
        public void KeyboardSubmitResponse( StringGameEvent gameEvent )
        {
			var answer = gameEvent.eventValue;
			var answer_hash = answer.GetHashCode();

			if( CurrentLevelData.Instance.levelData.CheckIfCorrectAnswer( answer_hash ) && !player_answers.ContainsKey( answer_hash ) )
            {
				player_answers.Add( answer_hash, answer );

                // If player answerd all questions, start over
                if( player_answers.Keys.Count == CurrentLevelData.Instance.levelData.question_answers.Length )
					player_answers.Clear();

                // Raise ball spawn event for player
				event_ballSpawn_player.Raise( GameSettings.Instance.ball_player_direction,
					answer.Length,
					GameSettings.Instance.ball_player_color
				);
			}
            else
				event_player_answer_wrong.Raise();
		}
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
			levelProgress.SetValue_NotifyAlways( 0 );

			var levelData = CurrentLevelData.Instance.levelData;

            // Set Active Scene
			if( levelData.scene_overrideAsActiveScene )
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 1 ) );
            else
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 0 ) );
		}

        private void LevelRevealedResponse()
        {

        }

        private void LevelStartedResponse()
        {

        }
#endregion
    }
}