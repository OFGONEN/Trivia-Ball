/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using FFStudio;
using TMPro;

public class Test_Keyboard : MonoBehaviour
{
#region Fields
	private TouchScreenKeyboard keyboard;

    public TextMeshProUGUI keyboard_text_input;
    public TextMeshProUGUI keyboard_text_area;
    public TextMeshProUGUI keyboard_text_status;

	private UnityMessage updateMethod;
#endregion
	private void Awake()
    {
		updateMethod = ExtensionMethods.EmptyMethod;
		TouchScreenKeyboard.hideInput = true;
	}

    private void Update()
    {
		updateMethod();
	}

    public void OpenAndCreate()
    {
		keyboard = TouchScreenKeyboard.Open( null, TouchScreenKeyboardType.Default, false, false, false, false, null, 100 );
		updateMethod = OnUpdate_KeyboardOpen;
	}

    public void Close()
    {
		keyboard.active = false;
		updateMethod = ExtensionMethods.EmptyMethod;
	}

    private void OnUpdate_KeyboardOpen()
    {
		keyboard_text_status.text = $"Status: {keyboard.status.ToString()}";
		keyboard_text_area.text = $"Alive: {keyboard.active}\n{TouchScreenKeyboard.area}";
		keyboard_text_input.text = keyboard.text;

        if( keyboard.status == TouchScreenKeyboard.Status.Done )
        {
			keyboard.text = string.Empty;
		    keyboard = TouchScreenKeyboard.Open( null, TouchScreenKeyboardType.Default, false, false, false, false, null, 100 );
		}
	}
}