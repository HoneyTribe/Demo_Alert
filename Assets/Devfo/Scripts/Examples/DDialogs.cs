using UnityEngine;
using System.Collections;

public class DDialogs : MonoBehaviour {

	private const string UNITY_EDITOR = "non-Android device";
	private const int STYLE_SPINNER = 0;
	private string alert1 = "Alert example 1";
	private string alert2 = "Alert example 2";
	private string alert3 = "Alert example 3";
	private string alert4 = "Alert example 4";
	private string alert5 = "Alert example 5";
	private string spin1 = "Spinner dialog: self cancelling";
	private string spin2 = "Spinner dialog  user cancel with callback";
	private string prog1 = "Progress dialog: static with cancel callback";
	private string prog2 = "Progress dialog: auto increment, auto close";
	private string toast1 = "Toast Long";
	private string toast2 = "Toast Short";
	
	static DialogHelper dialogHelper = null;
	static ProgressDialogHelper progressHelper = null;
	static ToastHelper toastHelper = null;
	
	//Call Helper screen on awake. Any earlier will cause error because the
	//screen(Android Activity) needs to be attached first--before the helper
	//screen links to "com.unity3d.player.UnityPlayer"
	void Awake ()
	{
		// Unity Editor throws JNI error. Can only test on Android device or emulator.
		#if UNITY_ANDROID && !UNITY_EDITOR
			dialogHelper = new DialogHelper();
			progressHelper = new ProgressDialogHelper();
			toastHelper = new ToastHelper();
		
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
			progressHelper = null;
			dialogHelper = null;
			toastHelper = null;
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
		
		
		GUI.Label (new Rect (20, 10, 450, 40), "Dialog Examples", myLabelStyle);		
		
		
		//Show dialog box with "OK" button that dismisses dialog. No callbacks.
		//Same as if you called ShowNativeDialog("text", new string[]{"OK","dismiss"}))
		//ShowNativeDialogOK(string message)
		if (GUI.Button (new Rect (15, 100, 205, 75), alert1, myButtonStyle)) {
			if (dialogHelper != null) {
				dialogHelper.ShowNativeDialogOK("Your message here. Default OK button dismisses " +
					"this dialog.");
			} else {
				alert1 = UNITY_EDITOR;
			}
		}	
		
		//Show dialog box - no user buttons.
		//Click outside the dialog box or back button will close dialog.
		//ShowNativeDialog(string message)
		if (GUI.Button (new Rect (250, 100, 205, 75), alert2, myButtonStyle)) {
			if (dialogHelper != null) {
				dialogHelper.ShowNativeDialog("Your message here. Click outside this box" +
					"or hit the BACK button to dismiss dialog.");
			} else {
				alert2 = UNITY_EDITOR;
			}
		}	
		
		//Show dialog box with 1 button
		//ShowNativeDialog(string message, string[] button1)
		if (GUI.Button (new Rect (15, 200, 205, 75), alert3, myButtonStyle)) {
			if (dialogHelper != null) {
				string[] button1 = new string[]{"my button","OnFunctionYes", "DialogObjectReceiver"};
				dialogHelper.ShowNativeDialog("Your message here. Single button with user defined button text and Unity" +
					"callback", button1);
			} else {
				alert3 = UNITY_EDITOR;
			}
		}	
		
		//Show dialog box with 2 buttons
		//ShowNativeDialog(string message, string[] button1, string[] button2)
		if (GUI.Button (new Rect (250, 200, 205, 75), alert4, myButtonStyle)) {
			if (dialogHelper != null) {
				string[] button1 = new string[]{"Yes","OnFunctionYes", "DialogObjectReceiver"};
				string[] button2 = new string[]{"No","OnFunctionNo", "DialogObjectReceiver"};
				dialogHelper.ShowNativeDialog("Dialog box with two user defined buttons",
					button1,button2);
			} else {
				alert4 = UNITY_EDITOR;
			}
		}
		
		//Show dialog box with 3 buttons
		//ShowNativeDialog (string message, string[] button1, string[] button2, string[] button3)
		if (GUI.Button (new Rect (15, 300, 205, 75), alert5, myButtonStyle)) {
			if (dialogHelper != null) {
				string[] button1 = new string[]{"Yes","OnFunctionYes", "DialogObjectReceiver"};
				string[] button2 = new string[]{"No","OnFunctionNo", "DialogObjectReceiver"};
				string[] button3 = new string[]{"Maybe","OnFunctionMaybe", "DialogObjectReceiver"};
				dialogHelper.ShowNativeDialog("Dialog box with three user defined buttons",
					button1,button2, button3);
			} else {
				alert5 = UNITY_EDITOR;
			}
		}	
		
		//show progress dialog for specified time.
		if (GUI.Button (new Rect (15, 400, 450, 75), spin1, myButtonStyle)) {
			if (progressHelper != null) {
				StartCoroutine(ShowAutoDismissDialog(2));
			} else {
				spin1 = UNITY_EDITOR;
			}
		}	
		
		//show progress dialog until user clicks outside the dialog box, or back button.
		//this will trigger a callback function.
		if (GUI.Button (new Rect (15, 500, 450, 75), spin2, myButtonStyle)) {
			if (progressHelper != null) {
				string[] cancelCallback = new string[]{"OnCancelDialogFunction", "ProgressObjectReceiver"};
				progressHelper.ShowSpinnerDialog("Please Wait",
					"Click outside the dialog box or hit back button to cancel. " +
					"The callback function OnCancelDialogFunction will be called on Scene Object ProgressObjectReceiver",
					true, cancelCallback);
			} else {
				spin2 = UNITY_EDITOR;
			}
		}	
		
		//Show progress dialog until it reaches 100%. Then auto-close.
		//Caution: auto-updating the progress bar is system intensive. Try to keep the update call to a minimum--like every 2-3 seconds.
		if (GUI.Button (new Rect (15, 600, 450, 75), prog1, myButtonStyle)) {
			if (progressHelper != null) {
				string[] cancelCallback = new string[]{"OnCancelDialogFunction", "ProgressObjectReceiver"};
				progressHelper.ShowProgressDialog("Loading", "Dialog will not change here. click outside the screen or back button to cancel." +
				 	"A cancel callback will be made.", true, cancelCallback, 100, false);
			} else {
				toast1 = UNITY_EDITOR;
			}
		}	
		
		if (GUI.Button (new Rect (15, 700, 450, 75), prog2, myButtonStyle)) {
			if (progressHelper != null) {
				string[] cancelCallback = new string[]{}; //No callback added. Must pass empty string[]
				progressHelper.ShowProgressDialog("Loading", "Dialog will hit 100% and close", false, cancelCallback, 10, true);
				StartCoroutine(AutoUpdateProgress());
				
			} else {
				toast1 = UNITY_EDITOR;
			}
		}	
		
		//show long Toast
		if (GUI.Button (new Rect (15, 800, 205, 75), toast1, myButtonStyle)) {
			if (toastHelper != null) {
				toastHelper.showToastLong("This is a long Toast");
			} else {
				toast1 = UNITY_EDITOR;
			}
		}	
		
		//show short Toast
		if (GUI.Button (new Rect (250, 800, 205, 75), toast2, myButtonStyle)) {
			if (toastHelper != null) {
				toastHelper.showToastShort("This is a short Toast");
			} else {
				toast2 = UNITY_EDITOR;
			}
		}	
	}
	
	private IEnumerator ShowAutoDismissDialog(float waitTime)
	{
		progressHelper.ShowSpinnerDialog("Please Wait", "Showing this message for " + waitTime + " seconds...");
		yield return new WaitForSeconds(waitTime);
		progressHelper.DismissProgressDialog();
	}
	
	//Not the best way to do it, but works for this example.
	private IEnumerator AutoUpdateProgress()
	{
		progressHelper.UpdateProgressDialog(2);
		yield return new WaitForSeconds(2);
		progressHelper.UpdateProgressDialog(4);
		yield return new WaitForSeconds(2);
		progressHelper.UpdateProgressDialog(6);
		yield return new WaitForSeconds(2);
		progressHelper.UpdateProgressDialog(8);
		yield return new WaitForSeconds(2);
		progressHelper.UpdateProgressDialog(10);
			
	}
	
}
