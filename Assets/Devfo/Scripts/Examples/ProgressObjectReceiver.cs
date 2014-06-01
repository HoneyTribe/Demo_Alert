using UnityEngine;
using System.Collections;

public class ProgressObjectReceiver : MonoBehaviour {
	
	//This script is attached to a Scene object.
	
	
	//callback function called by: ShowNativeProgressDialog(string title, string message, bool cancelable, int spinnerStyle, string[] cancelCallback)
	//function located in ProgressDialogHelper.cs
	public void OnCancelDialogFunction(string message)
	{
		Debug.Log("OnCancelDialogFunction: " + message);
	}
}
