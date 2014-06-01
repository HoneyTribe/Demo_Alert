using UnityEngine;
using System.Collections;

public class DPackageMgr : MonoBehaviour {

	// Use this for initialization
	private const string UNITY_EDITOR = "non-Android device";
	private string startApp = "Launch app: ";
	private string safeMode = "Was phone booted in SafeMode?";
	private string getPInfo = "Get package info: ";
	private string getInstl = "Get installed Apps";
	private string rmvIcon = "Remove THIS app from menu";
	private string hasPerm = " Has permission android.permission.INTERNET ?";
	private string installURL = "com.android.vending";
	
	static PackageManagerHelper managerHelper = null;
	static ToastHelper toastHelper = null;
	static DialogHelper dialogHelper = null;
	
	
	//Call Helper screen on awake. Any earlier will cause error because the
	//screen(Android Activity) needs to be attached first--before the helper
	//screen links to "com.unity3d.player.UnityPlayer"
	void Awake ()
	{
		// Unity Editor throws JNI error. Can only test on Android device or emulator.
		#if UNITY_ANDROID && !UNITY_EDITOR
			managerHelper = new PackageManagerHelper();
			toastHelper = new ToastHelper();
			dialogHelper = new DialogHelper();
		
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
	
	//startApplication
	//isSafeMode
	//removeAppIcon	
	//getInstalledApplications	
	//getPackageInfo
	//hasPermission
	
	void OnGUI ()
	{
		GUIStyle myButtonStyle = new GUIStyle (GUI.skin.button);
		myButtonStyle.fontSize = 20;
		myButtonStyle.wordWrap = true;
		
		GUIStyle myLabelStyle = new GUIStyle (GUI.skin.label);
		myLabelStyle.fontSize = 30;
		
		GUIStyle myFieldStyle = new GUIStyle (GUI.skin.textField);
		myLabelStyle.fontSize = 20;
		
		GUI.Label (new Rect (20, 10, 450, 40), "Package Manager Example", myLabelStyle);		
		GUI.Label (new Rect (20, 63, 200, 35), "use this PKG for examples:");
		GUI.TextField(new Rect (185, 60, 200, 40), installURL, myFieldStyle);
		
		//Start another app/package from within this app. You can use this for opening the 
		//Google play, Google Mail, Text messaging, or whatever app is installed on the phone.
		if (GUI.Button (new Rect (15, 110, 450, 75), startApp + installURL, myButtonStyle)) {
			if (managerHelper != null) {
				managerHelper.StartApplication (installURL);
			} else {
				startApp = UNITY_EDITOR;
			}
		}	
		
		//Was the phone booted in safe mode
		if (GUI.Button (new Rect (15, 220, 450, 75), safeMode, myButtonStyle)) {
			if (managerHelper != null) {
				safeMode = "Safe Mode: " + managerHelper.IsSafeMode();
			} else {
				safeMode = UNITY_EDITOR;
			}
		}
		
		GUI.Label (new Rect (20, 310, 450, 50), "Once clicked, the ICON for THIS app will no longer be " +
			"visible in the app main menu. You will need to uninstall/reinstall this demo. If ICON is still " +
			"visible, reboot device.");
		
		//Remove the app icon from the phone. (This demo build, whatever you call it on the Bundle Identifier)
		// This also removes the ability for the app to start the MAIN activity. Once you try this, you MUST uninstall
		// and reinstall this demo app. If you try a "build & run" from Unity, it will not start the activity because
		// the Manifest permissions were removed.
		if (GUI.Button (new Rect (15, 360, 450, 75), rmvIcon, myButtonStyle)) {
			if (managerHelper != null) {
				managerHelper.RemoveAppIcon();
			} else {
				rmvIcon = UNITY_EDITOR;
			}
		}
		
		//Get all installed apps on device, and show response in a native Dialog.
		if (GUI.Button (new Rect (15, 460, 450, 75), getInstl, myButtonStyle)) {
			if (managerHelper != null) {
				string response = managerHelper.GetInstalledApplications(false);
				dialogHelper.ShowNativeDialogOK(response);
			} else {
				getInstl = UNITY_EDITOR;
			}
		}
		
		//Get package information and show JSON response in a native Dialog.
		if (GUI.Button (new Rect (15, 560, 450, 75), getPInfo + installURL, myButtonStyle)) {
			if (managerHelper != null) {
				string response = managerHelper.GetPackageInfo(installURL);
				dialogHelper.ShowNativeDialogOK(response);		
			} else {
				getPInfo = UNITY_EDITOR;
			}
		}
		
		// does the app have the permission, and show response in native Toast.
		if (GUI.Button (new Rect (15, 660, 450, 150), installURL + hasPerm, myButtonStyle)) {
			if (managerHelper != null) {
				bool response = managerHelper.HasPermission("android.permission.INTERNET", installURL);
				toastHelper.showToastShort("RESPONSE: " + response);
				
			} else {
				hasPerm = UNITY_EDITOR;
			}
		}
		
	}
	
}
