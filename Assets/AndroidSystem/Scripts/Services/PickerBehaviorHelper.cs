using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using FourthSky.Android.Services;

public class PickerBehaviorHelper : MonoBehaviour {

	public bool foundTexture = false;

	private byte[] imgBytes = null;

	private Picker.OnImagePickedDelegate m_Callback;
	private GCHandle m_CallbackHandle;
	
	// Update is called once per frame
	void Update () {
		if (foundTexture) {

			// Create a new texture and load file content to it
			Texture2D tex = new Texture2D(4, 4);
			tex.wrapMode = TextureWrapMode.Clamp;
			
			float start = Time.realtimeSinceStartup;
			bool loaded = tex.LoadImage(imgBytes);
			Debug.Log("Image bytes loaded in " + (Time.realtimeSinceStartup - start) + " seconds");
			
			if (loaded) {
				Debug.Log("Texture format: " + tex.format + "\n" +
				          "Texture dimensions" + tex.width + "x" + tex.height);
				
				// Invoke callback
				m_Callback.Invoke(tex);
				
			} else {
				Debug.LogError("Error loading image");
				
			}

			// 
			m_CallbackHandle.Free();
			m_Callback = null;

			foundTexture = false;
		}
	}

	internal void RegisterDelegate(Picker.OnImagePickedDelegate callback) {
		this.m_Callback = new Picker.OnImagePickedDelegate(callback);
		m_CallbackHandle = GCHandle.Alloc (this.m_Callback);
	}

	internal void NotifyImageLoaded(string path) {

		// Read all bytes from image file
		float start = Time.realtimeSinceStartup;
		imgBytes = File.ReadAllBytes(path);
		Debug.Log("Image bytes read from file in " + (Time.realtimeSinceStartup - start) + " seconds");

		foundTexture = true;
	}

}
