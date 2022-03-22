/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using FFStudio;

public class KeyboardManager : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared" ) ] public SharedStringNotifier keyboard_input;
    [ BoxGroup( "Shared" ) ] public StringGameEvent keyboard_submit;
    [ BoxGroup( "Shared" ) ] public SharedFloatNotifier keyboard_height;

    // Private \\
    private TouchScreenKeyboard keyboard;
	private int keyboard_input_lenght;

	// Delegates
	private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
    {
		updateMethod = ExtensionMethods.EmptyMethod;
		TouchScreenKeyboard.hideInput = true;
	}

    private void Update()
    {
		updateMethod();

		keyboard_height.SharedValue = TouchScreenKeyboard.area.height;
	}
#endregion

#region API
    public void OpenAndCreate()
    {
		keyboard = TouchScreenKeyboard.Open( null, TouchScreenKeyboardType.Default, false, false, false, false, null, GameSettings.Instance.keyboard_max_characterLimit );
		updateMethod = OnUpdate_KeyboardOpen;

		ClearKeyboardInput();
	}

    public void Close()
    {
		keyboard.active = false;
		updateMethod    = ExtensionMethods.EmptyMethod;

		ClearKeyboardInput();
	}
#endregion

#region Implementation
    private void OnUpdate_KeyboardOpen()
    {
		var input  = keyboard.text;
		var length = input.Length;

		if( length != keyboard_input_lenght )
        {
			keyboard_input.SharedValue = input;
			keyboard_input_lenght = length;
		}

        if( keyboard.status == TouchScreenKeyboard.Status.Done )
        {
			keyboard_submit.Raise( keyboard.text );
			ClearKeyboardInput();

			keyboard = TouchScreenKeyboard.Open( null, TouchScreenKeyboardType.Default, false, false, false, false, null, 100 );
		}
	}

    private void ClearKeyboardInput()
    {
		keyboard.text = string.Empty;
		keyboard_input.SetValue_NotifyAlways( string.Empty );

		keyboard_input_lenght = 0;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}