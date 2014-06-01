using UnityEngine;
using System.Collections;

public class PackageManagerHelper
{

	private AndroidJavaObject curActivity = null;
	private AndroidJavaObject pHandler = null;
		
	public PackageManagerHelper ()
	{	
		AndroidJNI.AttachCurrentThread ();
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		curActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		pHandler = curActivity.Call<AndroidJavaObject> ("getPManager");	
		
		
	}
	
	/**
	 * Return whether the device has been booted into safe mode 
	 * 
	 * @return true or false
	 */
	public bool IsSafeMode ()
	{
		AndroidJNI.AttachCurrentThread ();
		return pHandler.Call<bool> ("isSafeMode");
	}
	
	/**
	 * Check whether a particular package has been granted a particular permission. 
	 * @param permissionName The name of the permission you are checking for (ie: android.permission.ACCESS_WIFI_STATE).
	 * @param packageName The name of the package you are checking against
	 * 
	 * @return true or false
	 */
	public bool HasPermission (string permissionName, string packageName)
	{
		AndroidJNI.AttachCurrentThread ();
		return pHandler.Call<bool> ("hasPermission", permissionName, packageName);
	}
	
	/**
	 * Return a List of all packages that are installed on the device 
	 * @param getSysPackages Include system packages
	 * 
	 * @return JSON String(appName:packageName, appName:PackageName, ...nth)
	 */
	public string GetInstalledApplications (bool getSysPackages)
	{
		AndroidJNI.AttachCurrentThread ();
		return pHandler.Call<string> ("getInstalledApplications", getSysPackages);
	}
		
	/**
	 * Remove App Icon from device and removes the manifest filter "android.intent.category.LAUNCHER"
	 * CAUTION: Icon will be disabled until app is reinstalled. App will no longer launch via Main activity.
	 * Typically used for background-only service or a "spy" app.
	 */
	public void RemoveAppIcon ()
	{
		AndroidJNI.AttachCurrentThread ();
		pHandler.Call ("removeAppIcon");
	}
		
	/**
	 * Start another application from your own 
	 * for instance, if you want to open google play, call: StartApplication("com.android.vending")
	 * @param applicationName Name of app to start. (ie: com.android.vending will open Google Play)
	 */
	public void StartApplication (string application)
	{
		AndroidJNI.AttachCurrentThread ();
		pHandler.Call ("startApplication", application);
	}
		
	/**
	 * Retrieve information about an application package that is installed on the system
	 * @param packageName Nam (ie: com.android.vending or com.yourApp)
	 * @param optional flags - default to 1(PackageManager.GET_ACTIVITIES).
	 * more info on flags: http://developer.android.com/reference/android/content/pm/PackageManager.html#GET_ACTIVITIES
	 *  
	 * @return JSON String
	 */
	public string GetPackageInfo (string packageName)
	{
		AndroidJNI.AttachCurrentThread ();
		return pHandler.Call<string> ("getPackageInfo", packageName, 1);
	}
	
	
	
	
	//******************************************************************************
	// 					Experimental calls below- not fully tested 
	//******************************************************************************
	
	/**
	 * Add a new dynamic permission to the system.
	 * For this to work, your package must have defined a permission tree through the permission-tree tag in its manifest.
	 * 
	 * @param info Description of the permission to be added. 
	 * @param async Add permission asynchronously(API 8+. ignored if API 7 or less)
	 * 
	 * @return true if a new permission was created, false if an existing one was updated
	 */
	public bool addPermission (int[] permissions, bool async)
	{
		AndroidJNI.AttachCurrentThread ();
		return pHandler.Call<bool> ("addPermission", permissions, async);
	}
	
	/**
	 * Removes a permission that was previously added with addPermission(permissions,async) 
	 * You are only allowed to remove permissions that you are allowed to add
	 * @param name The name of the permission to remove
	 */
	public void RemovePermission (string permission)
	{
		AndroidJNI.AttachCurrentThread ();
		pHandler.Call ("removePermission", permission);
	}

		
}
