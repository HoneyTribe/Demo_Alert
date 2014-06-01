using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			Application.Quit();
        }
		
    }
	
	void OnGUI ()
	{
		GUIStyle myButtonStyle = new GUIStyle (GUI.skin.button);
		myButtonStyle.fontSize = 20;
		myButtonStyle.wordWrap = true;
		
		GUIStyle myLabelStyle = new GUIStyle (GUI.skin.label);
		myLabelStyle.fontSize = 30;
		
		GUI.Label (new Rect (20, 10, 450, 40), "Native Android Examples", myLabelStyle);		
		
		//Device Information
		//Dialog Examples
		//Package Manager Example
		//SMS Examples
		if (GUI.Button (new Rect (15, 100, 450, 75), "Device Information", myButtonStyle)) {
			Application.LoadLevel(1);
		}	
		if (GUI.Button (new Rect (15, 200, 450, 75), "Dialog Examples", myButtonStyle)) {
			Application.LoadLevel(2);
		}
		if (GUI.Button (new Rect (15, 300, 450, 75), "Package Manager Example", myButtonStyle)) {
			Application.LoadLevel(3);
		}
		if (GUI.Button (new Rect (15, 400, 450, 75), "SMS Examples", myButtonStyle)) {
			Application.LoadLevel(4);
		}
		if (GUI.Button (new Rect (15, 500, 450, 75), "Share Examples", myButtonStyle)) {
			Application.LoadLevel(5);
		}
	}
}

