using UnityEngine;
using System.Collections;

public class ContactDisplay_AddContact : MonoBehaviour {

	public UILabel nameLabel;
	public UISprite bgSprite;
	string name;
	string number;
	ContactList contactList;
	// Use this for initialization

	public void AddContact(){


	}

	public void RemoveContact(){



	}


	public void SetDisplayInfo(string nameToSet, string numberToSet, ContactList cl){

		name = nameToSet;
		number = numberToSet;
		nameLabel.text = name;
		gameObject.name = name;
		contactList = cl;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
