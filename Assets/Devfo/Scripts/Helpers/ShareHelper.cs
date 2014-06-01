using UnityEngine;
using System.Collections;

public class ShareHelper {
	
	private AndroidJavaObject curActivity = null;
	private AndroidJavaObject sHandler = null;
	
	
	public ShareHelper()
	{
		AndroidJNI.AttachCurrentThread ();
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		curActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		sHandler = curActivity.Call<AndroidJavaObject> ("getShareIntent");
	}
	
	// Opens Share dialog. Device will automatically populate chooser list with
	// appropriate apps.
	// body: Message that will show up in main body, such as email content
	// subject: Message subject line, such as subject line of email
	// chooseDialogTitle: title that shows up on top of the native chosser.
	public void ShareText(string body, string subject, string chooseDialogTitle)
	{
		AndroidJNI.AttachCurrentThread ();
		sHandler.Call("shareText", body, subject, chooseDialogTitle);
	}
	
	//Opens share dialog. Passes the path of an image. You must know the full file path of the image, Like:
	//		"file:///sdcard/images/myImage.jpg"
	//		"android.resource://com.myAndroidApp.test/*"
	//		"/data/data/com.your.BundleIdentifer/files"
	// Unity can be tricky when saving images, then trying to specify the path. You may want to experiment with
	// Application.persistantDataPath, or hard coding the path.  
	//make sure you are specifying the correct location as per internal/external access using
	// Configuration-->write access-->external/internal
	public void ShareImage(string path, string chooseDialogTitle)
	{
		AndroidJNI.AttachCurrentThread ();
		sHandler.Call("shareImage", path, chooseDialogTitle);
	}
	
	// Generic function to call specific Share dialogs
	// A full list of MIME types can be seen here: http://www.webmaster-toolkit.com/mime-types.shtml
	public void ShareGeneric(string type, string body, string subject, string chooseDialogTitle)
	{
		AndroidJNI.AttachCurrentThread ();
		sHandler.Call("shareGeneric", type, body, subject, chooseDialogTitle);
	}
	
	// Updated generic share function. Added extra paramater "path" to pass in media path.
	// If you want to share text with an image, use "*/*" as your type.
	// example ShareGeneric( "*/*", "my body text", "my subject text", "my title", "file:///sdcard/images/myImage.jpg")
	public void ShareGeneric(string type, string body, string subject, string chooseDialogTitle, string path)
	{
		AndroidJNI.AttachCurrentThread ();
		sHandler.Call("shareGeneric", type, body, subject, chooseDialogTitle, path);
	}
	
	// Similar to ShareGeneric, except that an extra paramater that specifies your package name (BundleIdentifier).
	// What this will do is show the "Choose dialog" with only YOUR app. The user will no longer have the option to
	// choose a different app. 
	// This function was added for developers that want to customize a Facebook or Twitter call.
	public void ShareByPackageName(string type, string body, string subject, string chooseDialogTitle, string path, string bundleIdentifier)
	{
		AndroidJNI.AttachCurrentThread ();
		sHandler.Call("shareGeneric", type, body, subject, chooseDialogTitle, path, bundleIdentifier);
	}
}
