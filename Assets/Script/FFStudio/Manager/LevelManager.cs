/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
        [ TitleGroup( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedListener;
        public EventListenerDelegateResponse levelRevealedListener;
        public EventListenerDelegateResponse levelStartedListener;
        public MultipleEventListenerDelegateResponse levelFinishedListener;

        [ TitleGroup( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;
        public GameEvent event_player_answer_wrong;
        public BallSpawnEvent event_ballSpawn_player;
        public BallSpawnEvent event_ballSpawn_enemy;

        [ TitleGroup( "Shared" ) ]
        public SharedFloatNotifier levelProgress;
		public SharedStringNotifier notifier_enemy_input;
		public SharedIntNotifier notifier_currency;

		// Private
		private Dictionary< int, string > player_answers = new Dictionary< int, string >( 64 );
        private Tween ai_answer_tween;
#endregion

#region UnityAPI
        private void OnEnable()
        {
            levelLoadedListener.OnEnable();
            levelRevealedListener.OnEnable();
            levelStartedListener.OnEnable();
			levelFinishedListener.OnEnable();
		}

        private void OnDisable()
        {
            levelLoadedListener.OnDisable();
            levelRevealedListener.OnDisable();
            levelStartedListener.OnDisable();
			levelFinishedListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedListener.response   = LevelLoadedResponse;
            levelRevealedListener.response = LevelRevealedResponse;
            levelStartedListener.response  = LevelStartedResponse;
            levelFinishedListener.response = LevelFinishedResponse;
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
				event_ballSpawn_player.Raise( 
                    true,
                    GameSettings.Instance.ball_player_direction,
				    Mathf.Max( 1, answer.Length / GameSettings.Instance.ball_spawn_letterCount ),
                    PlayerPrefs.GetFloat( "power", GameSettings.Instance.ball_default_power ),
                    PlayerPrefs.GetInt( "health", GameSettings.Instance.ball_default_health ),
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
			player_answers.Clear();

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
			// ai_answer_tween.Recycle( DOVirtual.DelayedCall( CurrentLevelData.Instance.levelData.ai_answer_rate.ReturnRandom(), AIAnswer  ) );
			ai_answer_tween = DOVirtual.DelayedCall( CurrentLevelData.Instance.levelData.ai_answer_rate.ReturnRandom(), AIAnswer );
		}

        private void LevelFinishedResponse()
        {
			ai_answer_tween.Kill();
			PlayerPrefs.SetInt( "currency", notifier_currency.SharedValue );
		}

        private void AIAnswer()
        {
			var levelData    = CurrentLevelData.Instance.levelData;
			var randomAnswer = levelData.question_answers.ReturnRandom();

			event_ballSpawn_enemy.Raise( 
                false,
                GameSettings.Instance.ball_enemy_direction,
				Mathf.Max( 1, randomAnswer.Length / GameSettings.Instance.ball_spawn_letterCount ),
                levelData.ai_ball_power,
                levelData.ai_ball_health,
				GameSettings.Instance.ball_enemy_color
			);

			notifier_enemy_input.SetValue_NotifyAlways( randomAnswer );
			// ai_answer_tween.Recycle( DOVirtual.DelayedCall( CurrentLevelData.Instance.levelData.ai_answer_rate.ReturnRandom(), AIAnswer  ) );
			ai_answer_tween = DOVirtual.DelayedCall( CurrentLevelData.Instance.levelData.ai_answer_rate.ReturnRandom(), AIAnswer );
		}
#endregion
    }
}