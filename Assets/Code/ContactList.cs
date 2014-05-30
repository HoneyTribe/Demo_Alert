using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ContactList : MonoBehaviour {

	// Use this for initialization
	public UIGrid grid;
	public GameObject contactPrefab;
	public List<Contact> testContactList = new List<Contact>();
	public List<ContactDisplay> contactDisplayList = new List<ContactDisplay>();

	void Start () {
	
	}

	public void ContactedSuccessfully(string number){

		foreach(ContactDisplay cDisplay in contactDisplayList){

			if(cDisplay.GetNumber().Equals(number)){

				cDisplay.Contacted();
			}

		}

	}


	public void PopulateContactList(){

		foreach(Contact c in testContactList){

			GameObject cButton = GetContactPrefab();
			ContactDisplay cDisplay = cButton.GetComponent<ContactDisplay>();
			cDisplay.SetDisplayInfo(c.contactName, c.contactNumber);
			contactDisplayList.Add (cDisplay);
		}

		grid.Reposition();
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
