using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FourthSky.Android;
using FourthSky.Android.Services;
using Boomlagoon.JSON;
using System.Text.RegularExpressions;
public class AppManager : MonoBehaviour {

	// Use this for initialization
	public GameObject splashScreen;
	public GameObject addContactScreen;
	public GameObject mainScreen;
	public ContactList contactList;
	public ContactList phoneContactList;
	public string phoneNumber;
	public string message;
	public AndroidSystemDemoMultipleScreens asd;
	float timer = 0f;
	float timerMax = 3f;
	public GUIText output;

	public AudioSource tap;
	public AudioSource add;
	public AudioSource delete;
	public AudioSource close;
	public string testJSON = "";
	void Start () {
		Input.location.Start();

	
	}



	// Broadcast Receiver constants and variables
	public void ShowMainScreen(){

		TweenPosition.Begin (splashScreen,0.5f, new Vector3(splashScreen.transform.localPosition.x,2000,0));
		contactList.CheckSavedContent();
		tap.Play();

	}
	
	public void GetAllContacts(){

		ContactsHelper contactsHelper = new ContactsHelper();
		string response = contactsHelper.GetAllContacts();

	}

	public void ShowAddContactScreen(){

		tap.Play ();
		TweenPosition.Begin (addContactScreen,0.3f, new Vector3(splashScreen.transform.localPosition.x,-67,0));
		phoneContactList.ClearPhoneContactList();
	//	phoneContactList.GetContactsFromPhone("d");
		Invoke ("GetContacts",0.4f);

	}

	void GetContacts(){

		ContactsHelper contactsHelper = new ContactsHelper();
		string response = contactsHelper.GetAllContacts();
		phoneContactList.GetContactsFromPhone(response);
	}

	public void ContactAdded(){

		add.Play ();

	}

	public void ContactDeleted(){

		delete.Play ();
	}

	public void HideAddContactScreen(){

		TweenPosition.Begin (addContactScreen,0.15f, new Vector3(splashScreen.transform.localPosition.x,2000,0));
		close.Play ();

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
	public void SendMessageToNumber(string num){

		string newMessage = "Hi " + message + ". I am at " + "LAT : " +  Input.location.lastData.latitude + " LONG : " + Input.location.lastData.longitude;
		asd.CustomSendSMS(num, newMessage, contactList);

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
