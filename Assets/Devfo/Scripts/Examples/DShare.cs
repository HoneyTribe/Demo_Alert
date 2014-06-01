using UnityEngine;
using System.Collections;

public class DShare : MonoBehaviour {
	
	private const string UNITY_EDITOR = "non-Android device";
	private string shareText = "Share Text with another app";
	private string shareImage = "Share Image with another app";
	private string shareGeneric = "Generic share intent";
	static ShareHelper helperScript = null;
	
	
	//Call Helper screen on awake. Any earlier will cause error because the
	//screen(Android Activity) needs to be attached first--before the helper
	//screen links to "com.unity3d.player.UnityPlayer"
	void Awake()
	{
		// Unity Editor throws JNI error. Can only test on Android device or emulator.
		#if UNITY_ANDROID && !UNITY_EDITOR
			helperScript = new ShareHelper();
		#endif
	}
	
	void Start()
	{
		AndroidJNI.AttachCurrentThread();
	}
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			Application.LoadLevel(0);
        }
		
    }
	
	void OnGUI()
	{
		GUIStyle myButtonStyle = new GUIStyle (GUI.skin.button);
		myButtonStyle.fontSize = 20;
		myButtonStyle.wordWrap = true;
		
		GUIStyle myLabelStyle = new GUIStyle (GUI.skin.label);
		myLabelStyle.fontSize = 30;
		
		
		GUI.Label (new Rect (20, 10, 450, 40), "Share Examples", myLabelStyle);	
		
		GUI.Label (new Rect (20, 100, 450, 50), "Opens a Share Intent. For this example, we will pass" +
			"in the subject line of 'Test Share Subject', the body 'Test Share Body', and 'Choose app' for the dialog " +
			"box header");
		
		if (GUI.Button (new Rect (15, 150, 300, 75), shareText, myButtonStyle)) {
			if (helperScript != null) {
				
				helperScript.ShareText("Test Share Body", "Test Share Subject", "Choose app");
			} else {
				shareText = UNITY_EDITOR; 
			}
		}
		
		GUI.Label (new Rect (20, 270, 450, 50), "Opens a Share Intent. For this example, we will pass" +
			"in an image located at: sdcard/images/myImage.jpg. -- NOTE: This image does not exist on your device." +
			 "You will need supply a valid image path.");
		if (GUI.Button (new Rect (15, 320, 300, 75), shareImage, myButtonStyle)) {
			if (helperScript != null) {
				
				string path = "file:///sdcard/images/myImage.jpg";
				helperScript.ShareImage(path, "Choose app");
			} else {
				shareText = UNITY_EDITOR;
			}
		}
		
		GUI.Label (new Rect (20, 440, 450, 50), "Opens a Share Intent with a supplied 'generic' MIME type Flag. For this example," +
			" we will use google.note");
		if (GUI.Button (new Rect (15, 475, 300, 75), shareGeneric, myButtonStyle)) {
			if (helperScript != null) {
				
				// A full list of MIME types can be seen here: http://www.webmaster-toolkit.com/mime-types.shtml
				string mimeType = "vnd.android.cursor.dir/vnd.google.note";
				helperScript.ShareGeneric(mimeType, "test body", "test subject", "Choose");
			} else {
				shareText = UNITY_EDITOR;
			}
		}
	}
	
	
}
