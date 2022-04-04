/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UIScale : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ) ] public RectTransform rectTransform;
    [ BoxGroup( "Setup" ) ] public Vector3 startSize;
    [ BoxGroup( "Setup" ) ] public Vector3 endSize;
    [ BoxGroup( "Setup" ) ] public float duration;
    [ BoxGroup( "Setup" ) ] public UnityEvent onComplete;

    private RecycledTween scaleTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void DoScale()
    {
		scaleTween.Kill();
		rectTransform.localScale = startSize;
		scaleTween.Recycle( rectTransform.DOScale( endSize, duration ), onComplete.Invoke );
	}

    public void KillScale()
    {
		scaleTween.Kill();
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
