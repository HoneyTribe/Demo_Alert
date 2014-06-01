using UnityEngine;
using System.Collections;

public class SMSHelper
{

	private AndroidJavaObject curActivity = null;
	private AndroidJavaObject handler = null;
		
	public SMSHelper ()
	{	
		AndroidJNI.AttachCurrentThread ();
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		curActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		handler = curActivity.Call<AndroidJavaObject> ("getSms");
	}
	
	//launch the device's default text messaging application in a new screen(Activity).
	//param must be in format: blank/blank, number,blank, number/body. CANNOT pass in body only.
	public void LaunchSMSActivity ()
	{			
		string[] str = new string[] {"",""};
		launch(str);
	}
	
	public void LaunchSMSActivity (string number, string msg)
	{	
		string[] str = new string[] {number, msg};
		launch(str);
	}
	
	public void LaunchSMSActivity (string number)
	{	
		string[] str = new string[] {number,""};
		launch(str);
	}
	
	private void launch (string[] param)
	{
		AndroidJNI.AttachCurrentThread ();
		handler.Call ("launchSmsActivity", param);
	}
}
