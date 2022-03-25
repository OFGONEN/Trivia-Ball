/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

public abstract class IncrementalButton : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ) ] public SharedIntNotifier notifier_currency;
    [ BoxGroup( "Setup" ) ] public RectTransform rectTransform;
    [ BoxGroup( "Setup" ) ] public Button button;
    [ BoxGroup( "Setup" ) ] public TextMeshProUGUI text_value;
    [ BoxGroup( "Setup" ) ] public TextMeshProUGUI text_cost;
    [ BoxGroup( "Setup" ) ] public Color text_color_positive;
    [ BoxGroup( "Setup" ) ] public Color text_color_negative;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		button.interactable = false;
	}
#endregion

#region API
    public Tween GoUp()
    {
		UpdateVisual();
		button.interactable = false;

		return rectTransform.DOAnchorPosY( 
			-rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y,
			 GameSettings.Instance.ui_Entity_Move_TweenDuration 
		).OnComplete( CheckCost );
	}

    public Tween GoDown()
    {
		button.interactable = false;

		return rectTransform.DOAnchorPosY( 
			-( rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y ),
			 GameSettings.Instance.ui_Entity_Move_TweenDuration 
		);
    }


	public abstract bool CanAfford();
#endregion

#region Implementation
	protected void CheckCost()
    {
		button.interactable = CanAfford();
	}

	protected abstract void UpdateVisual();
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnValidate()
	{
		text_color_positive = text_color_positive.SetAlpha( 1 );
		text_color_negative = text_color_negative.SetAlpha( 1 );
	}
#endif
#endregion
}