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
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void DoScale()
    {
		rectTransform.localScale = startSize;
		rectTransform.DOScale( endSize, duration ).OnComplete( onComplete.Invoke );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
