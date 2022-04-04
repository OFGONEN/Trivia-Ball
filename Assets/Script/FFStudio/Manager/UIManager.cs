/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class UIManager : MonoBehaviour
    {
#region Fields
        [ BoxGroup( "UI Elements" ) ] public Image question_image;
        [ BoxGroup( "UI Elements" ) ] public Image flash_image;
        [ BoxGroup( "UI Elements" ) ] public TextMeshProUGUI question_text;
        [ BoxGroup( "UI Elements" ) ] public RectTransform answerBox_player;
        [ BoxGroup( "UI Elements" ) ] public RectTransform answerBox_enemy;
        [ BoxGroup( "UI Elements" ) ] public IncrementalButton incremental_left;
        [ BoxGroup( "UI Elements" ) ] public IncrementalButton incremental_right;
        [ BoxGroup( "UI Elements" ) ] public Button incremental_pass;

        [ FoldoutGroup( "Base - Listeners" ) ] public EventListenerDelegateResponse levelLoadedResponse;
        [ FoldoutGroup( "Base - Listeners" ) ] public EventListenerDelegateResponse levelCompleteResponse;
        [ FoldoutGroup( "Base - Listeners" ) ] public EventListenerDelegateResponse levelFailResponse;
        [ FoldoutGroup( "Base - Listeners" ) ] public EventListenerDelegateResponse tapInputListener;

        [ FoldoutGroup( "Base - UI" ) ] public UI_Patrol_Scale level_loadingBar_Scale;
        [ FoldoutGroup( "Base - UI" ) ] public TextMeshProUGUI level_count_text;
        [ FoldoutGroup( "Base - UI" ) ] public TextMeshProUGUI level_information_text;
        [ FoldoutGroup( "Base - UI" ) ] public UI_Patrol_Scale level_information_text_Scale;
        [ FoldoutGroup( "Base - UI" ) ] public Image loadingScreenImage;
        [ FoldoutGroup( "Base - UI" ) ] public Image foreGroundImage;
        [ FoldoutGroup( "Base - UI" ) ] public RectTransform tutorialObjects;

        [ FoldoutGroup( "Base - Fired Events" ) ] public GameEvent levelRevealedEvent;
        [ FoldoutGroup( "Base - Fired Events" ) ] public GameEvent loadNewLevelEvent;
        [ FoldoutGroup( "Base - Fired Events" ) ] public GameEvent resetLevelEvent;
        [ FoldoutGroup( "Base - Fired Events" ) ] public ElephantLevelEvent elephantLevelEvent;

        private RecycledSequence answerBox_enemy_sequence = new RecycledSequence();
		private RectTransform incremental_pass_transform;
#endregion

#region Unity API
        private void OnEnable()
        {
            levelLoadedResponse.OnEnable();
            levelFailResponse.OnEnable();
            levelCompleteResponse.OnEnable();
            tapInputListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedResponse.OnDisable();
            levelFailResponse.OnDisable();
            levelCompleteResponse.OnDisable();
            tapInputListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedResponse.response   = LevelLoadedResponse;
            levelFailResponse.response     = LevelFailResponse;
            levelCompleteResponse.response = LevelCompleteResponse;
            tapInputListener.response      = ExtensionMethods.EmptyMethod;

			level_information_text.text = "Tap to Start";

			question_image.color = question_image.color.SetAlpha( 0 );
			question_text.color = question_text.color.SetAlpha( 0 );
			question_text.text   = string.Empty;

			incremental_pass_transform = incremental_pass.targetGraphic.rectTransform;
			incremental_pass.interactable = false;
		}
#endregion

#region API
        public void TweenEnemyBox()
        {
			answerBox_enemy_sequence.Kill();

			var sequence = answerBox_enemy_sequence.Recycle();

			sequence.Append( answerBox_enemy.DOAnchorPosX( 0, GameSettings.Instance.ui_Entity_Move_TweenDuration ) );
			sequence.AppendInterval( GameSettings.Instance.ai_answer_cooldown );
			sequence.Append( answerBox_enemy.DOAnchorPosX( answerBox_enemy.sizeDelta.x, GameSettings.Instance.ui_Entity_Move_TweenDuration ) );
		}

        public void IncrementalSelected()
        {
			var passButtonTarget = -( incremental_pass_transform.sizeDelta.y + incremental_pass_transform.anchoredPosition.y );
			incremental_pass.interactable = false;

			var sequence = DOTween.Sequence();

			sequence.Append( incremental_left.GoDown() )
 					.Join( incremental_right.GoDown() )
					.Join( incremental_pass_transform.DOAnchorPosY( passButtonTarget, GameSettings.Instance.ui_Entity_Move_TweenDuration ) )
					.AppendCallback( levelRevealedEvent.Raise )
					.Append( question_image.DOFade( 1, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					.Join( question_text.DOFade( 1, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					.Join( answerBox_player.DOMoveX( 0, GameSettings.Instance.ui_Entity_Move_TweenDuration ) );

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}

		public void FlashForeGround( )
		{
			var color = GameSettings.Instance.ui_flashColor;
			var alpha = color.a;

			flash_image.color = color.SetAlpha( 0 );

			var sequence = DOTween.Sequence();
			sequence.Append( flash_image.DOFade( alpha,
				GameSettings.Instance.ui_flashColor_duration_in ) 
				.SetEase( GameSettings.Instance.ui_flashColor_ease_in ) 
			);

			sequence.Append( flash_image.DOFade( 0, 
			GameSettings.Instance.ui_flashColor_duration_out )
			.SetEase( GameSettings.Instance.ui_flashColor_ease_out )
			);
		}
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
			var levelData = CurrentLevelData.Instance.levelData;
			question_text.text = levelData.question;

			var sequence = DOTween.Sequence()
								.Append( level_loadingBar_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
								.Append( loadingScreenImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) );

			                    if( levelData.showIncremantal )
								    sequence.AppendCallback( () => tapInputListener.response = ShowIncremental );
                                else
								    sequence.AppendCallback( () => tapInputListener.response = StartLevel );

			level_count_text.text = "Level " + CurrentLevelData.Instance.currentLevel_Shown;

            levelLoadedResponse.response = NewLevelLoaded;
        }

        private void LevelCompleteResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;

			level_information_text.text = "Completed \n\n Tap to Continue";

			sequence.Append( question_image.DOFade( 0 , GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    .Join( question_text.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    .Join( answerBox_player.DOMoveX( -answerBox_player.sizeDelta.x, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    .Join( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = LoadNewLevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelCompleted;
            elephantLevelEvent.Raise();
        }

        private void LevelFailResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;
			level_information_text.text = "Level Failed \n\n Tap to Continue";

			sequence.Append( question_image.DOFade( 0 , GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    .Join( answerBox_player.DOMoveX( -answerBox_player.sizeDelta.x, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    .Join( question_text.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    .Join( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    // .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = Resetlevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelFailed;
            elephantLevelEvent.Raise();

		}

        private void NewLevelLoaded()
        {
			var currentLevel = CurrentLevelData.Instance;
			level_count_text.text = "Level " + currentLevel.currentLevel_Shown;
			question_text.text = currentLevel.levelData.question;

			level_information_text.text = "Tap to Start";

			var sequence = DOTween.Sequence();

			// Tween tween = null;

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );

			if( currentLevel.levelData.showIncremantal )
				sequence.AppendCallback( () => tapInputListener.response = ShowIncremental ); 
           else
				sequence.AppendCallback( () => tapInputListener.response = StartLevel );

            // elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            // elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            // elephantLevelEvent.Raise();
        }

		private void StartLevel()
		{
			foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration );
			question_image.DOFade( 1, GameSettings.Instance.ui_Entity_Fade_TweenDuration );
			question_text.DOFade( 1, GameSettings.Instance.ui_Entity_Fade_TweenDuration );

			answerBox_player.DOMoveX( 0, GameSettings.Instance.ui_Entity_Move_TweenDuration );

			level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration );
			level_information_text_Scale.Subscribe_OnComplete( levelRevealedEvent.Raise );

			tutorialObjects.gameObject.SetActive( false );

			tapInputListener.response = ExtensionMethods.EmptyMethod;

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}

        private void ShowIncremental()
        {
			tutorialObjects.gameObject.SetActive( false );
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var passButtonTarget = -incremental_pass_transform.anchoredPosition.y - incremental_pass_transform.sizeDelta.y;
			incremental_pass.interactable = true;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					.Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.Append( incremental_left.GoUp() )
 					.Join( incremental_right.GoUp() )
 					.Join( incremental_pass_transform.DOAnchorPosY( passButtonTarget, GameSettings.Instance.ui_Entity_Move_TweenDuration ) );

			if( !incremental_left.CanAfford() && !incremental_right.CanAfford() )
			{
				incremental_pass.interactable = false;
				sequence.AppendInterval( GameSettings.Instance.ui_Entity_wait_duration );
				sequence.AppendCallback( IncrementalSelected );
			}
		}

		private void LoadNewLevel()
		{
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
			        .AppendCallback( loadNewLevelEvent.Raise );
		}

		private void Resetlevel()
		{
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
			        .AppendCallback( resetLevelEvent.Raise );

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}
#endregion
    }
}