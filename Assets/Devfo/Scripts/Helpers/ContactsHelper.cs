using UnityEngine;
using System.Collections;

public class ContactsHelper {

	private AndroidJavaObject curActivity = null;
	private AndroidJavaObject cHandler = null;
		
	public ContactsHelper ()
	{	
		AndroidJNI.AttachCurrentThread ();
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		curActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		cHandler = curActivity.Call<AndroidJavaObject> ("getContacts");
	}
	
	//get Contact information for a given phoneNumber. Returns null if phone number is not in device Contacts.
	//returns contact id, name, phone
	public void GetContactDetailsFromPhone(string phoneNumber)
	{
		AndroidJNI.AttachCurrentThread ();
		cHandler.Call ("getIdFromPhoneNumber", phoneNumber);
	}
	
	//gets Contact information for a given email address. Returns null if email is not in device Contacts.
	//returns contact id, name, phone number(if exists)
	public void GetContactDetailsFromEmail(string emailAddress)
	{
		AndroidJNI.AttachCurrentThread ();
		cHandler.Call ("getIdFromEmail", emailAddress);
	}

	public string GetAllContacts()
	{
		AndroidJNI.AttachCurrentThread ();
		return cHandler.Call<string> ("getAllContacts");
	}
}
