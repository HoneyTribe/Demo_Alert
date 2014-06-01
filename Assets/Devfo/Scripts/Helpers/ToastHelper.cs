using UnityEngine;
using System.Collections;

public class ToastHelper {

	private AndroidJavaObject curActivity = null;
	private AndroidJavaObject tHandler = null;
	
	public ToastHelper ()
	{	
		AndroidJNI.AttachCurrentThread ();
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		curActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");	
		tHandler = curActivity.Call<AndroidJavaObject> ("getToast");
	}
	
	//shows message in an Android native Toast, with Toast.LENGTH_SHORT
	public void showToastShort(string message)
	{
		AndroidJNI.AttachCurrentThread ();
		tHandler.Call ("showToastShort", message);
	}
	
	//shows message in an Android native Toast, with Toast.LENGTH_LONG
	public void showToastLong(string message)
	{
		AndroidJNI.AttachCurrentThread ();
		tHandler.Call ("showToastLong", message);
	}
			
}
