using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;
using System.Text.RegularExpressions;
public class ContactList : MonoBehaviour {

	// Use this for initialization
	public UIGrid grid;
	public GameObject contactPrefab;
	public GameObject addContactButton;
	public List<Contact> testContactList = new List<Contact>();
	public List<ContactDisplay> contactDisplayList = new List<ContactDisplay>();
	public UIScrollView scrollView;
	public string testJSON = "";
	AppManager appManager;
	public GameObject loadingString;
	void Start () {


		appManager = GameObject.FindWithTag("GameController").GetComponent<AppManager>();
	}

	public void ContactedSuccessfully(string number){

		foreach(ContactDisplay cDisplay in contactDisplayList){

			if(cDisplay.GetNumber().Equals(number)){

				cDisplay.Contacted();
			}

		}

	}
	public void AddNewContact(string num, string nam){

		Contact newContact = new Contact();
		newContact.contactName = nam;
		newContact.contactNumber = num;
		testContactList.Add (newContact);
		ResetList();
		appManager.ContactAdded();

	}

	public void ClearPhoneContactList(){
		loadingString.SetActive(true);
		foreach(ContactDisplay cD in contactDisplayList){
			
			DestroyImmediate (cD.gameObject);
			
		}
		testContactList.Clear ();
		contactDisplayList.Clear ();
		grid.Reposition();
		
		scrollView.ResetPosition();


	}

	public void GetContactsFromPhone(string phoneList){


		JSONObject json = JSONObject.Parse(phoneList);
		foreach(KeyValuePair<string, JSONValue> f in json){
			
			Debug.Log (f.Key);
			char[] charList = new char[5];
			charList[0] = ':';
			charList[1] = '"';
			charList[2] = '}';
			charList[3] = '{';
			charList[4] = '"';
			
			string trimmedString = f.Value.ToString();
			Regex rgx = new Regex("[^-][^0-9]+");
			trimmedString = rgx.Replace(trimmedString, "");
			Debug.Log (trimmedString);

			Contact newContact = new Contact();
			newContact.contactName = f.Key.ToString();
			newContact.contactNumber = trimmedString;
			testContactList.Add (newContact);
		}
		
		loadingString.SetActive(false);
		PopulateContactList();

	}




	public void PopulateContactList(){

		foreach(Contact c in testContactList){

			GameObject cButton = GetContactPrefab();
			ContactDisplay cDisplay = cButton.GetComponent<ContactDisplay>();
			cDisplay.SetDisplayInfo(c.contactName, c.contactNumber,this);
			contactDisplayList.Add (cDisplay);
		}

		grid.Reposition();

		scrollView.ResetPosition();
	}

	void AddContactButton(){
		
		GameObject newcontact = NGUITools.AddChild(grid.gameObject,addContactButton);
		newcontact.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
	}

	public void Delete(string n){

		appManager.ContactDeleted();
		for(int i = 0; i < testContactList.Count; i++){


			if(testContactList[i].contactNumber.Equals(n)){

				testContactList.RemoveAt (i);
				break;
			}

		}
		
		ResetList ();

	}

	void ResetList(){

		foreach(ContactDisplay cD in contactDisplayList){
			
			DestroyImmediate (cD.gameObject);
			
		}
		
		contactDisplayList.Clear ();
		grid.Reposition();
		
		scrollView.ResetPosition();
		PopulateContactList();


	}

	GameObject GetContactPrefab(){

		GameObject newcontact = NGUITools.AddChild(grid.gameObject,contactPrefab);
		newcontact.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
		return newcontact;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
