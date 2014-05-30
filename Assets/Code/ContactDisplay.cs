using UnityEngine;
using System.Collections;

public class ContactDisplay : MonoBehaviour {

	public UILabel nameLabel;
	public UISprite bgSprite;
	string name;
	string number;
	ContactList contactList;
	// Use this for initialization
	void Start () {
	
	}

	public string GetNumber(){

		return number;
	}

	public void Contacted(){

		bgSprite.spriteName = "contactAlerted";
	}

	public void SetDisplayInfo(string nameToSet, string numberToSet){

		name = nameToSet;
		number = numberToSet;
		nameLabel.text = name;
		gameObject.name = name;

	}

	// Update is called once per frame
	void Update () {
	
	}
}
