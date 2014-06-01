using UnityEngine;
using System.Collections;

public class DialogObjectReceiver : MonoBehaviour {

	public void OnFunctionYes(string message)
	{
		Debug.Log("OnFunctionYes: " + message);
	}
	
	public void OnFunctionNo(string message)
	{
		Debug.Log("OnFunctionNo: " + message);
	}
	
	public void OnFunctionMaybe(string message)
	{
		Debug.Log("OnFunctionMaybe: " + message);
	}
}
