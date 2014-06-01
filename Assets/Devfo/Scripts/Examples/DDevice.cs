using UnityEngine;
using System.Collections;

public class DDevice : MonoBehaviour
{

	private const string UNITY_EDITOR = "non-Android device";
	private string deviceId = "Push for Device Id";
	private string deviceNo = "Push for Phone#";
	private string appVerName = "Push for App Name";
	private string appVerNo = "Push for App Ver#";
	private string appConn = "Check connectivity";
	private string appISO = "Get ISO country code";
	private string appMem = "Get memory stats";
	static DeviceHelper helperScript = null;
	
	//Call Helper screen on awake. Any earlier will cause error because the
	//screen(Android Activity) needs to be attached first--before the helper
	//screen links to "com.unity3d.player.UnityPlayer"
	void Awake ()
	{
		// Unity Editor throws JNI error. Can only test on Android device or emulator.
		#if UNITY_ANDROID && !UNITY_EDITOR
			helperScript = new DeviceHelper();
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
		
		GUI.Label (new Rect (20, 10, 450, 35), "Device Information", myLabelStyle);

		if (GUI.Button (new Rect (15, 100, 450, 75), deviceId, myButtonStyle)) {
			if (helperScript != null) {
				deviceId = helperScript.GetDeviceId ();
			} else {
				deviceId = UNITY_EDITOR;
			}
		}
		
		if (GUI.Button (new Rect (15, 200, 450, 75), deviceNo, myButtonStyle)) {
			if (helperScript != null) {
				deviceNo = helperScript.GetDevicePhoneNumber ();
			} else {
				deviceNo = UNITY_EDITOR;
			}
		}
		
		if (GUI.Button (new Rect (15, 300, 450, 75), appVerName, myButtonStyle)) {
			if (helperScript != null) {
				appVerName = helperScript.GetVersionName ();
			} else {
				appVerName = UNITY_EDITOR;
			}
		}
		
		if (GUI.Button (new Rect (15, 400, 450, 75), appVerNo, myButtonStyle)) {
			if (helperScript != null) {
				appVerNo = "Version: " + helperScript.GetVersionCode ();
			} else {
				appVerNo = UNITY_EDITOR;
			}
		}
		
		if (GUI.Button (new Rect (15, 500, 450, 75), appConn, myButtonStyle)) {
			if (helperScript != null) {
				appConn = "Connected: " + helperScript.HasConnection ();
			} else {
				appConn = UNITY_EDITOR;
			}
		}
		
		if (GUI.Button (new Rect (15, 600, 450, 75), appISO, myButtonStyle)) {
			if (helperScript != null) {
				appISO = helperScript.GetCountryCode ();
			} else {
				appISO = UNITY_EDITOR;
			}
		}
		
		if (GUI.Button (new Rect (15, 700, 450, 200), appMem, myButtonStyle)) {
			if (helperScript != null) {
				appMem = helperScript.GetVmHeapStats ();
			} else {
				appMem = UNITY_EDITOR;
			}
		}
		
		
	}
}
