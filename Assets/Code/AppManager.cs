using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FourthSky.Android;
using FourthSky.Android.Services;

public class AppManager : MonoBehaviour {

	// Use this for initialization
	public GameObject splashScreen;

	public ContactList contactList;
	public string phoneNumber;
	public string message;
	public AndroidSystemDemoMultipleScreens asd;
	float timer = 0f;
	float timerMax = 3f;
	void Start () {
		Input.location.Start();

	}
	// Broadcast Receiver constants and variables
	public void ShowMainScreen(){


		TweenPosition.Begin (splashScreen,0.5f, new Vector3(splashScreen.transform.localPosition.x,2000,0));
		contactList.PopulateContactList();

	}

	public void TestMessage(){


		foreach(Contact c in contactList.testContactList){

			string newMessage = "Hi " + c.contactName + " " + message + ". I am at " + "LAT : " +  Input.location.lastData.latitude + " LONG : " + Input.location.lastData.longitude;
			asd.CustomSendSMS(c.contactNumber, newMessage, contactList);
		}



		/*
		//SINGLE USER TEST
		string newMessage = "Hi " + "USER" + " " + message + ". I am at " + "LAT : " +  Input.location.lastData.latitude + " LONG : " + Input.location.lastData.longitude;
		asd.CustomSendSMS(phoneNumber, newMessage, contactList);
		*/

	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown (KeyCode.Pause) ||
		   Input.GetKeyDown (KeyCode.Minus) ||
		   Input.GetKeyDown (KeyCode.Plus) ||
		   Input.GetKeyDown (KeyCode.Space) ||
		   Input.GetKeyDown (KeyCode.Less)){

			TestMessage();
		}

		if(Input.GetMouseButton (0)){

	
			timer += 1 * Time.deltaTime;

		}
		if(Input.GetMouseButtonUp (0)){

			if(timer >= timerMax){

				TestMessage();

			}

			timer = 0f;
		}

	}
}
