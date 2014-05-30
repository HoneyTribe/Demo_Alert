using UnityEngine;
using System.Collections;
using FourthSky.Android;
using FourthSky.Android.Services;

public class AndroidSystemDemoMultipleScreens : MonoBehaviour {
	// GUI measures
	private int GUI_MARGIN = 30;
	private int GUI_BUTTON_WIDTH = Screen.width - 60;
	private int GUI_BUTTON_HEIGHT = 100;	

	// GUI states
	#pragma warning disable 414
	private static readonly int GUI_MENU = 0;
	private static readonly int GUI_NFC = 1;
	private static readonly int GUI_TELEPHONY = 2;
	private static readonly int GUI_PICKER = 3;
	private static readonly int GUI_BROADCAST = 4;
	private static readonly int GUI_BILLING = 5;
	private int guiState = 0;

	
	// Use this for initialization
	void Awake () {
		
		// Create broadcast receiver
		testBroadcastReceiver = new BroadcastReceiver();
		testBroadcastReceiver.OnReceive += (context, intent) => {
			string action = intent.Call<string>("getAction");
			
			if (action == testBroadcastAction) {
				string message = intent.Call<string>("getStringExtra", "message");
				
				broadcastMessage = "Broadcast message received: " + message;
				
			} else if (action == BroadcastActions.ACTION_HEADSET_PLUG) {
				int headsetState = intent.Call<int>("getIntExtra", "state", 0);
				
				if (headsetState == 1) {
					string headsetName = intent.Call<string>("getStringExtra", "name");
					int hasMicrophone = intent.Call<int>("getIntExtra", "microphone", 0);
					broadcastMessage = "Headset plugged (" + headsetName + " - " + (hasMicrophone == 1 ? " with mic" : "no mic") + ")";
					
					Debug.Log("Headset plugged (" + headsetName + " - " + (hasMicrophone == 1 ? " with mic" : "no mic") + ")");
				} else {
					broadcastMessage = "Headset unplugged";
					
					Debug.Log("Headset unplugged");
				}
				
			} else if (action == BroadcastActions.ACTION_BATTERY_CHANGED) {
				int batteryLevel = intent.Call<int>("getIntExtra", "level", 0);
				int batteryMaxLevel = intent.Call<int>("getIntExtra", "scale", 0);
				int isPlugged = intent.Call<int>("getIntExtra", "plugged", 0);
				
				if (isPlugged == 1) {
					broadcastMessage = "Power plugged (battery level " + batteryLevel + "/" + batteryMaxLevel + ")";				
				} else {
					broadcastMessage = "Power unplugged (battery level " + batteryLevel + "/" + batteryMaxLevel + ")";
				}
			}
		};
		
		// Create service connection 
		billingServiceConnection = new ServiceConnection();
		billingServiceConnection.OnServiceConnected += (name, binder) => {
			if (binder == null) {
				Debug.Log("Something's wrong");	
			}
			
			billingBinder = IInAppBillingService.Wrap(binder);
			billingMessage = "Billing service enabled";
		};
		billingServiceConnection.OnServiceDisconnected += (name) => {
			billingBinder.Dispose();
			billingBinder = null;
			billingMessage = "Billing service disabled";
		};

		photoPlane.SetActive( false );
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// If back pressed, exit
		if (Input.GetKeyDown(KeyCode.Escape)) {

			if (guiState == 0) {
				Application.Quit();

			} else {
				photoPlane.SetActive( false );
				guiState = 0;
			}
		}

	}
	
	void OnApplicationQuit() {
		// Dispose objects
		if (testBroadcastReceiver != null) 
			testBroadcastReceiver.Dispose();
		
		if (billingServiceConnection != null) 
			billingServiceConnection.Dispose();
	}
	
	void OnGUI() {
	//	GUI.Label(new Rect(GUI_MARGIN, 15, 650, 25), "Android System Plugin Demo v1.3 by Fourth Sky Interactive");

		switch(guiState) {
		// Menu GUI
		case 0:
		//	MenuOnGUI ();
			break;

		// NFC tests
		case 1:
		//	NFCOnGUI ();
			break;

		// Telephony tests
		case 2:
		//	TelephonyOnGUI ();
			break;

		// Image picker tests
		case 3:
		//	PickerOnGUI ();
			break;

		// Broadcast tests
		case 4:
		//	BroadcastReceiverOnGUI ();
			break;

		// Billing tests
		case 5:
		//	BillingServiceOnGUI ();
			break;
		}
	}

	void MenuOnGUI() {
		int buttonSize = (GUI_BUTTON_WIDTH / 2) - 10;

		// Draw buttons
		if (GUI.Button(new Rect(GUI_MARGIN, 40, buttonSize, buttonSize), "NFC")) {
			guiState = 1;
		
		} else if (GUI.Button(new Rect(Screen.width - buttonSize - GUI_MARGIN , 40, buttonSize, buttonSize), "Telephony")) {
			guiState = 2;
		
		} else if (GUI.Button(new Rect(GUI_MARGIN, 40 + buttonSize + 15, buttonSize, buttonSize), "Picker")) {
			guiState = 3;
			
		} else if (GUI.Button(new Rect(Screen.width - buttonSize - GUI_MARGIN , 40 + buttonSize + 15, buttonSize, buttonSize), "Broadcast")) {
			guiState = 4;
			
		} else if (GUI.Button(new Rect(GUI_MARGIN, 40 + buttonSize * 2 + 30, buttonSize, buttonSize), "Billing")) {
			guiState = 5;
			
		}

	}


	string nfcMessage = "Testing NFC";
	string nfcReceivedMessage = "No messages";
	
	void NFCOnGUI () {
		// text message
		nfcMessage = GUI.TextField (new Rect(GUI_MARGIN, 40, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), nfcMessage, 32);
		
		// Check if billing is supported on this device
		// This can be affected if device has network connection
		if (GUI.Button(new Rect(GUI_MARGIN, 150, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Publish NFC Message")) {
			NFC.Publish(nfcMessage);
		}
		
		// Listen for NFC messages
		if (GUI.Button(new Rect(GUI_MARGIN, 260, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT ), "Listen for NFC messages")) {
			NFC.Subscribe( (string s) => { nfcReceivedMessage = s; } );
		}
		
		// Printing billing messages
		GUI.Label(new Rect(GUI_MARGIN, 370, 200, 30), nfcReceivedMessage);
	}


	// Telephony constants and variables
	string phoneNumber = "07456861236";
	string message = "Testing SMS";
	string messageStatus = "No messages";

	public void CustomSendSMS(string myNumber, string myMessage, ContactList cList){

		Telephony.SendSMS(myNumber, 
		                  myMessage, 
		                  (sentOK) => {
			if (sentOK) {

				
				messageStatus = "SMS Sent successfully";
			} else {
				messageStatus = "Failed to send SMS";
			}
		}, 
		(deliveredOK) => {
			if (deliveredOK) {

				messageStatus = "SMS Delivered successfully";
			} else {
				messageStatus = "SMS not delivered";
			}
		});
		cList.ContactedSuccessfully(myNumber);
	}

	void TelephonyOnGUI() {

		// Phone number
		phoneNumber = GUI.TextField (new Rect (GUI_MARGIN, 40, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), phoneNumber, 12);
		
		// Text to send
		message = GUI.TextArea (new Rect (GUI_MARGIN, 150, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), message, 160);
		
		// Button to send text message
		if (GUI.Button(new Rect(GUI_MARGIN, 260, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Send SMS Message")) {
			TestSendSMS();

		}

		if (GUI.Button (new Rect (GUI_MARGIN, 370, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Call phone")) {
			Telephony.MakeCall(phoneNumber, false);
		}
		
		// Printing SMS status messages
		GUI.Label(new Rect(GUI_MARGIN, 480, 500, 30), messageStatus);
		
	}
	

	public GameObject photoPlane;
	private string contactInfo;
	
	void PickerOnGUI() {
		// Pick an image from gallery
		if (GUI.Button(new Rect(GUI_MARGIN, 40, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Pick an image from gallery")) {		
			Picker.PickImageFromGallery((Texture2D tex) => { 
											photoPlane.SetActive( true );
											photoPlane.renderer.material.mainTexture = tex; 
										});
		}

	}
	
	// Broadcast Receiver constants and variables
	private static readonly string testBroadcastAction = "com.unity.bcast.test";
	
	private BroadcastReceiver testBroadcastReceiver;
	private static string broadcastMessage = "BroadcastReceiver unregistered";
	
	void BroadcastReceiverOnGUI() {
		// Register broadcast receiver
		if (GUI.Button(new Rect(GUI_MARGIN, 40, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Register Broadcast Receiver")) {
			testBroadcastReceiver.Register(testBroadcastAction, 
			                               BroadcastActions.ACTION_BATTERY_CHANGED,
			                               BroadcastActions.ACTION_BATTERY_LOW,
			                               BroadcastActions.ACTION_POWER_CONNECTED);
			
			broadcastMessage = "BroadcastReceiver registered";
		}
		
		// Send a custom broadcast
		if (GUI.Button(new Rect(GUI_MARGIN, 150, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Send Broadcast")) {
			Hashtable extras = new Hashtable();
			extras.Add("message", "Hello broadcast!!!");
			
			AndroidSystem.SendBroadcast(testBroadcastAction, extras);
		}
		
		// Unregister broadcast receiver
		if (GUI.Button(new Rect(GUI_MARGIN, 260, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Unregister Broadcast Receiver")) {
			testBroadcastReceiver.Unregister();
			
			broadcastMessage = "BroadcastReceiver unregistered";
		}
		
		// Printing broadcast messages
		GUI.Label(new Rect(GUI_MARGIN, 370, 500, 30), broadcastMessage);
		
	}
		
	
	// Service Connection and Binder constants and variables
	private static readonly string billingServiceAction = "com.android.vending.billing.InAppBillingService.BIND";
	
	private ServiceConnection billingServiceConnection;
	private static IInAppBillingService billingBinder;
	private static string billingMessage = "Billing service disabled";
	
	void BillingServiceOnGUI() {
		// Bind service connection to In-App Billing v3
		if (GUI.Button(new Rect(GUI_MARGIN, 40, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Initiate Billing Service")) {
			billingServiceConnection.Bind(billingServiceAction);
		}
		
		// Check if billing is supported on this device
		// This can be affected if device has network connection
		if (GUI.Button(new Rect(GUI_MARGIN, 150, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Check Billing Supported")) {
			int responseCode = billingBinder.IsBillingSupported(3, AndroidSystem.PackageName, "inapp");
			if (responseCode == 0) {
				billingMessage = "Billing supported";
			} else {
				billingMessage = "Billing unsupported";
			}
		}
		
		// Unbind In-App Billing
		if (GUI.Button(new Rect(GUI_MARGIN, 260, GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), "Shutdown Billing Service")) {
			billingServiceConnection.Unbind();
			billingMessage = "Billing service disabled";
		}
		
		// Printing billing messages
		GUI.Label(new Rect(GUI_MARGIN, 370, 200, 30), billingMessage);
	}
	public void TestSendSMS(){
		Debug.Log ("Test Send SMS");
		Telephony.SendSMS(phoneNumber, 
		                  message, 
		                  (sentOK) => {
			if (sentOK) {
				messageStatus = "SMS Sent successfully";
			} else {
				messageStatus = "Failed to send SMS";
			}
		}, 
		(deliveredOK) => {
			if (deliveredOK) {
				messageStatus = "SMS Delivered successfully";
			} else {
				messageStatus = "SMS not delivered";
			}
		});



	}




}
