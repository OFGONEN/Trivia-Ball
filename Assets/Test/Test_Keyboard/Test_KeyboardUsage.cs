/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;
using TMPro;

public class Test_KeyboardUsage : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared" ) ] public EventListenerDelegateResponse listener_keyboard_submit;
    [ BoxGroup( "Shared" ) ] public SharedStringNotifier notifier_keyboard_input;
    [ BoxGroup( "Shared" ) ] public SharedFloat shared_keyboard_height;

    [ BoxGroup( "Setup" ) ] public TextMeshProUGUI text_input;
    [ BoxGroup( "Setup" ) ] public TextMeshProUGUI text_submit;
    [ BoxGroup( "Setup" ) ] public RectTransform uiImage;

#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		listener_keyboard_submit.OnEnable();
		notifier_keyboard_input.Subscribe( OnKeyboardInputResponse );
	}

    private void OnDisable()
    {
		listener_keyboard_submit.OnDisable();
		notifier_keyboard_input.Subscribe( OnKeyboardInputResponse );
	}

    private void Awake()
    {
		listener_keyboard_submit.response = KeyboardSubmitResponse;
	}

    private void Update()
    {
		uiImage.position = Vector3.up * shared_keyboard_height.sharedValue;
	}
#endregion

#region API
#endregion

#region Implementation
    private void KeyboardSubmitResponse()
    {
		text_submit.text = ( listener_keyboard_submit.gameEvent as StringGameEvent ).eventValue;
	}

    private void OnKeyboardInputResponse()
    {
		text_input.text = notifier_keyboard_input.sharedValue;
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}