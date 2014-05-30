using System;
using UnityEngine;


namespace FourthSky
{
	namespace Android
	{
		namespace Services
		{
			public static class Picker
			{
				// Delegate that will receive the picked image
				public delegate void OnImagePickedDelegate (Texture2D tex);

				static Picker() {
					helperGO = new GameObject ("Helper Picker");
					helperGO.hideFlags = HideFlags.HideInHierarchy;
					GameObject.DontDestroyOnLoad (helperGO);
					pickerHelper = helperGO.AddComponent<PickerBehaviorHelper> ();
				}

				public static void PickImageFromGallery(OnImagePickedDelegate callback) {
					// Store callback
					pickerHelper.RegisterDelegate (callback);

#if UNITY_ANDROID
					// Alert: "$" character denotes inner classes
					// From MediaStore.Images class (there are also MediaStore.Video, MediaStore.Audio and MediaStore.Files) ...
					using (AndroidJavaClass mediaClass = new AndroidJavaClass("android.provider.MediaStore$Images$Media")) {

						// ... and start gallery app to pick an image.
						AndroidJavaObject pickImageURI = mediaClass.GetStatic<AndroidJavaObject>("EXTERNAL_CONTENT_URI");
						AndroidSystem.StartActivityForResult(ActivityActions.ACTION_PICK, pickImageURI, PICK_IMAGE, OnActivityResult);
					}
#endif
				}

				// OnActivityResult constants variables
				private static readonly int PICK_IMAGE = 13463;
				private static string imagePickPath = "";
				
				private static GameObject helperGO = null;
				private static PickerBehaviorHelper pickerHelper;


#if UNITY_ANDROID
				// OnActivityResult callback
				private static void OnActivityResult(int requestCode, int resultCode, AndroidJavaObject intent) {
					try {
						if (requestCode == PICK_IMAGE) {
							switch(resultCode) {				
							case AndroidSystem.RESULT_OK:
								
								// Get URI path (for example: content://media/external/images/media/712)
								string contentUri = intent.Call<AndroidJavaObject>("getData").Call<string>("toString");
								//Debug.Log("URI from picked image: " + contentUri);
								
								// Get real path of file (for example: mnt/images/IMG357.jpg )
								string [] columns = { "_data" };
								using (AndroidCursor cursor = AndroidCursor.Open (contentUri, columns))
								{						
									if (cursor.MoveToNext())
									{
										// Finally, get image file path
										imagePickPath = cursor.Get<string>("_data");
									}
								}

								//Debug.Log("File path of picked image: " + imagePickPath);
								
								// Defer image loading to picker helper, 
								// that will load the image in the main thread
								pickerHelper.NotifyImageLoaded(imagePickPath);
								
								break;
								
							case AndroidSystem.RESULT_CANCELED:
								Debug.Log("Pick image operation cancelled");
				 				break;
								
							default:
								Debug.LogError("Error occurred picking image from gallery");
								break;
								
							}
						}
						
					} catch (Exception ex) {
						Debug.LogException(ex);
					}
				}
#endif

			}

		}
	}
}

