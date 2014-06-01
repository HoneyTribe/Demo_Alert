using UnityEngine;
using System.Collections;

public class ProgressDialogHelper {

	private AndroidJavaObject curActivity = null;
	private AndroidJavaObject dpHandler = null;
	
	public ProgressDialogHelper ()
	{	
		AndroidJNI.AttachCurrentThread ();
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		curActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");	
		dpHandler = curActivity.Call<AndroidJavaObject> ("getProgressDialog");
	}
	
	//show progress dialog. Non-cancelable -- need to send HideNativeProgressDialog(), or 
	// use this function with StartCoroutine()
	public void ShowSpinnerDialog(string title, string message)
	{
		AndroidJNI.AttachCurrentThread ();
		dpHandler.Call ("ShowSpinnerDialog", title, message);
	}
	
	//set cancelable to true will dismiss progress dialog if user hits the back button
	// need to send HideNativeProgressDialog(), or 
	// use this function with StartCoroutine()
	public void ShowSpinnerDialog(string title, string message, bool cancelable)
	{
		AndroidJNI.AttachCurrentThread ();
		dpHandler.Call ("ShowSpinnerDialog", title, message, cancelable);
	}
	
	//add a callback method
	//cancelCallback fires when dialog is cancelled (not dismissed). Normally cancelable is set to TRUE with callback.
	// cancelCallback requires: callback function, Scene callback object
	//ie: string[] cancelCallback = new string[]{"onCancelDialogFunction", "ProgressObjectReceiver"}
	public void ShowSpinnerDialog(string title, string message, bool cancelable, string[] cancelCallback)
	{
		AndroidJNI.AttachCurrentThread ();
		dpHandler.Call ("ShowSpinnerDialog", title, message, cancelable, cancelCallback);
	}
	
	public void ShowProgressDialog(string title, string message, bool cancelable, string[] cancelCallback, int maxBarValue, bool autoClose) {
		AndroidJNI.AttachCurrentThread ();
		dpHandler.Call ("ShowProgressDialog", title, message, cancelable, cancelCallback, maxBarValue, autoClose);
	}
	
	//update the progress bar. 
	// If autoClose was set to true, the dialog box will close once progress=maxBarValue
	// If autoclose was set to false, progress can show greater than maxBarValue. You must also close it yourself by calling DismissProgressDialog()
	// CAUTION: this call can be sluggish, especially if calling it once a second. Try to avoid a fast-pace bar. For instance, update it for every 10% complete.
	public void UpdateProgressDialog(int progress)
	{
		AndroidJNI.AttachCurrentThread ();
		dpHandler.Call ("UpdateProgressDialog", progress);
	}
	
	public void DismissProgressDialog()
	{
		AndroidJNI.AttachCurrentThread ();
		dpHandler.Call ("DismissProgressDialog");
	}
	
}
