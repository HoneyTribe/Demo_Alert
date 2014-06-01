using UnityEngine;
using System.Collections;

public class DSMS : MonoBehaviour
{
	
	private const string UNITY_EDITOR = "non-android device";
	private string appSMS = "Launch SMS Activity \n No contact or subject";
	private string appSMS2 = "Launch SMS Activity \n Phone#: 555-5555";
	private string appSMS3 = "Launch SMS Activity \n Phone#: 555-5555 \n subject: HELLO";
	static SMSHelper helperScript = null;
	
	//Call Helper screen on awake. Any earlier will cause error because the
	//screen(Android Activity) needs to be attached first--before the helper
	//screen links to "com.unity3d.player.UnityPlayer"
	void Awake ()
	{
		// Unity Editor throws JNI error. Can only test on Android device or emulator.
		#if UNITY_ANDROID && !UNITY_EDITOR
			helperScript = new SMSHelper();
		#endif
	}
	
	void Start ()
	{
		AndroidJNI.AttachCurrentThread ();
	}
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			Application.LoadLevel(0);
        }
		
    }
	
	void OnGUI ()
	{
		GUIStyle myButtonStyle = new GUIStyle (GUI.skin.button);
		myButtonStyle.fontSize = 20;
		myButtonStyle.wordWrap = true;
		
		GUIStyle myLabelStyle = new GUIStyle (GUI.skin.label);
		myLabelStyle.fontSize = 30;
		
		GUI.Label (new Rect (20, 10, 450, 35), "SMS Utilities", myLabelStyle);
		
		
		//Launch default SMS screen -- blank.
		if (GUI.Button (new Rect (15, 100, 450, 100), appSMS, myButtonStyle)) {
			if (helperScript != null) {
				helperScript.LaunchSMSActivity();
			} else {
				appSMS = UNITY_EDITOR;
			}
		}
		
		//Launch default SMS screen and populate with phone# only
		if (GUI.Button (new Rect (15, 250, 450, 100), appSMS2, myButtonStyle)) {
			if (helperScript != null) {
				helperScript.LaunchSMSActivity("5555555");
			} else {
				appSMS2 = UNITY_EDITOR;
			}
		}
		
		//Launch default SMS screen and populate with phone# and message
		if (GUI.Button (new Rect (15, 400, 450, 100), appSMS3, myButtonStyle)) {
			if (helperScript != null) {
				helperScript.LaunchSMSActivity("5555555", "HELLO");
			} else {
				appSMS2 = UNITY_EDITOR;
			}
		}
		
		
	}
}
