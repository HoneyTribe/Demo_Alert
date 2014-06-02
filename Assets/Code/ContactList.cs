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
	public bool localList = true;
	public List<string> retryList = new List<string>();
	bool retrying = false;
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

	public void RemoveFromRetryList(string num){

		for(int i = 0; i < retryList.Count; i++){

			if(retryList[i] == num){

				retryList.RemoveAt (i);
				break;
			}
		}

		if(retryList.Count <= 0){

			CancelInvoke ("RetrySending");
			retrying  = false;
		}
	}

	public void RetrySending(){

		foreach(string num in retryList){

			appManager.SendMessageToNumber(num);
		}
	}

	public void AddToRetryList(string num){
		bool numberExists = false;
		foreach(string c in retryList){

			if(c == num){

				numberExists = true;
				break;
			}
		}

		if(!numberExists){

			retryList.Add (num);
			if(!retrying){
				retrying = true;
				InvokeRepeating("RetrySending", 4f,5f);
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

		ResetList();

		JSONObject json = JSONObject.Parse(phoneList);
		Debug.Log (json);

		foreach(KeyValuePair<string, JSONValue> f in json){
			
			char[] charList = new char[5];
			charList[0] = ':';
			charList[1] = '"';
			charList[2] = '}';
			charList[3] = '{';
			charList[4] = '"';
			
			string trimmedString = f.Value.ToString();
			Contact newContact = new Contact();
			newContact.contactName = f.Key.ToString();
			string[] trimmedStrings = f.Value.ToString().Split ('"');
	
			newContact.contactNumber = trimmedStrings[3];
			Debug.Log (newContact.contactNumber);
			testContactList.Add (newContact);
		}
		
		loadingString.SetActive(false);
		PopulateContactList();


	}

	public void CreateContactString(){
		string contactString = "";

		foreach(Contact c in testContactList){

			string newContactEntry = c.contactName + "," + c.contactNumber;
			contactString = contactString + ":" + newContactEntry;
		}
				Debug.Log (contactString);
		PlayerPrefs.SetString ("SavedContacts",contactString);
		PlayerPrefs.Save();
	}

	public string GetContactsFromSavedList(){

		string savedContactList = PlayerPrefs.GetString ("SavedContacts");
		return savedContactList;

	}


	public void CheckSavedContent(){
		
		string savedContacts = GetContactsFromSavedList();
		Debug.Log (savedContacts);
		if(savedContacts.Length > 1f){
			
			string[] splitAtName = savedContacts.Split (':');
			//string[] splitAtNumber = savedContacts.Split(',');
			for(int i = 0; i < splitAtName.Length;i++){
				
				if(splitAtName[i] != "" && splitAtName[i] != " "){
					Debug.Log (splitAtName[i]);
					Contact newContact = new Contact();
					//newContact.contactName = numberSplit[0];
					string[] contactSplit = splitAtName[i].Split (',');				
			
					newContact.contactName = contactSplit[0];
					newContact.contactNumber = contactSplit[1];
					Debug.Log (newContact.contactName);
					Debug.Log (newContact.contactNumber);
					testContactList.Add (newContact);
				}
			}
		}
		
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
		if(localList){

			CreateContactString();
		}

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
