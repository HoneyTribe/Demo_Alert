using UnityEngine;
using System.Collections;

public class ContactDisplay : MonoBehaviour {



	public UILabel nameLabel;
	public UISprite bgSprite;
	string name;
	string number;
	ContactList contactList;
	ContactList storedContactList;
	public bool isInContacts = false;
	public bool phoneContactsList = false;
	// Use this for initialization
	void Start () {
	
	}

	public string GetNumber(){

		return number;
	}

	public void Contacted(){

		bgSprite.spriteName = "contactAlerted";
	}

	public void DeleteEntry(){

		contactList.Delete(number);
	}

	public void ContactSelected(){

		if(isInContacts){

			storedContactList.Delete(GetNumber());
			isInContacts = false;
			SetContactBG();

		}

		else{

			isInContacts = true;
			storedContactList.AddNewContact(number,name);
			SetContactBG ();

		}


	}

	public void SetDisplayInfo(string nameToSet, string numberToSet, ContactList cl){

		name = nameToSet;
		number = numberToSet;
		nameLabel.text = name;
		gameObject.name = name;
		contactList = cl;

		if(phoneContactsList){
			storedContactList = GameObject.FindWithTag ("StoredContactList").GetComponent<ContactList>();
			GetContactStatus();
		}

	}

	void SetContactBG(){

		if(isInContacts){

			bgSprite.spriteName = "personAdded";
		}

		else{

			bgSprite.spriteName = "personNotAdded";
		}

	}


	void GetContactStatus(){

		foreach(Contact c in storedContactList.testContactList){

			if(c.contactNumber == GetNumber()){
				Debug.Log ("Same Number! " + c.contactNumber + " " + this.number);
				isInContacts = true;
				SetContactBG();
			}

		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
