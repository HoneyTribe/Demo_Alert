using UnityEngine;
using System.Collections;

public class DialogHelper {

	private AndroidJavaObject curActivity = null;
	private AndroidJavaObject dHandler = null;
	private string[] emptyString = new string[]{};
	
	public DialogHelper ()
	{	
		AndroidJNI.AttachCurrentThread ();
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		curActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");	
		dHandler = curActivity.Call<AndroidJavaObject> ("getDialog");
	}
	
	//ShowNativeDialog(string message, string button1, string button2)
	//button[] { "text", "callback", "callbackObject"}
	// text: Text to show on button
	// callback: callback function name. Function to reside inside callbackObject
	// callbackObject: Object on Hierarchy that contains C# class that holds callback.
	
	//Shows message in an Android native dialog box.
	//No user buttons on dialog. 
	//Click outside the dialog box will close dialog.
	//Back button will close dialog.
	public void ShowNativeDialog(string message)
	{
		ShowNativeDialog (message, emptyString, emptyString, emptyString);
	}
	
	//Show dialog box with "OK" button that dismisses dialog. No callbacks.
	public void ShowNativeDialogOK(string message)
	{
		ShowNativeDialog (message, new string[]{"OK","dismiss"}, emptyString, emptyString);
	}
	
	//Show dialog box with 1 button
	public void ShowNativeDialog(string message, string[] button1)
	{
		ShowNativeDialog (message, button1, emptyString, emptyString);
	}
	
	//Show dialog box with 2 buttons
	public void ShowNativeDialog(string message, string[] button1, string[] button2)
	{
		ShowNativeDialog (message, button1, button2, emptyString);
	}
	
	//Show dialog box with 3 buttons
	public void ShowNativeDialog (string message, string[] button1, string[] button2, string[] button3)
	{
		if (button1.Length == 2 && !button1[1].Equals("dismiss")) 
			Debug.LogWarning("ShowNativeDialog: Incorrect paramater size of: "+button1.Length);
		AndroidJNI.AttachCurrentThread ();
		dHandler.Call ("showDialog", message, button1, button2, button3);
	}
	
	
}
