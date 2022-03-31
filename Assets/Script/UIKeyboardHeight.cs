/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class UIKeyboardHeight : MonoBehaviour
{
#region Fields
    private RectTransform rectTransform;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
        rectTransform = GetComponent< RectTransform >();
    }

    private void Update()
    {
		rectTransform.position = rectTransform.position.SetY( TouchScreenKeyboard.area.height );
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
