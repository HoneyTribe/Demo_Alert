using UnityEngine;
using System.Collections;

public class DeviceHelper
{

	private AndroidJavaObject curActivity = null;
	private AndroidJavaObject handler = null;
		
	public DeviceHelper ()
	{	
		AndroidJNI.AttachCurrentThread ();
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		curActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		handler = curActivity.Call<AndroidJavaObject> ("getDevice");
	}
	
	/**
	 * Gets the device ID. 
	 * Get ID from TelephonyManager if device is a phone.
	 * Get ID from Secure.ANDROID_ID if TelephonyManager DNE.
	 * 
	 * @return Device ID or "null" on error.
	 */
	public string GetDeviceId ()
	{	
		AndroidJNI.AttachCurrentThread ();					
		return handler.Call<string> ("getDeviceId");
	}
	
	/**
	 * The version name of this package.
	 * 
	 * @return package name or "null" on error.
	 */
	public string GetVersionName ()
	{	
		AndroidJNI.AttachCurrentThread ();		
		return handler.Call<string> ("getVersionName");
	}
		
	/**
	 * The Bundle version number of this package.
	 * 
	 * @return version or -1 on error.
	 */
	public int GetVersionCode ()
	{	
		AndroidJNI.AttachCurrentThread ();		
		return handler.Call<int> ("getVersionCode");
	}
		
	/**
	 * Indicates whether network connectivity exists and it is possible to establish connections and pass data.
	 * 
	 * @return true or false.
	 */
	public bool HasConnection ()
	{	
		AndroidJNI.AttachCurrentThread ();		
		return handler.Call<bool> ("hasConnection");
	}
	
	/**
	 * Returns the phone number.
	 * Availability: Only when device supports TelephonyManager(device is a phone).
	 * 
	 * @return phone number or "null".
	 */
	public string GetDevicePhoneNumber ()
	{
		AndroidJNI.AttachCurrentThread ();	
		return handler.Call<string> ("getDevicePhoneNumber");
	}
		
	/**
	 * Returns virtualMachine and native memory sizes, including: 
	 * freeMemory, maxMemory, heapMemory, vmAllocated, NativeHeapAllocated, NativeHeapSize.
	 * Also returns lowMemory(bool) and number available processors.
	 * 
	 * @return JSON String(vmFree, vmMax, vmHeap, vmAllocated, nativeAllocated, totalAllocated, nativeHeap, availableProcessors, lowMemory)
	 */
	public string GetVmHeapStats ()
	{
		AndroidJNI.AttachCurrentThread ();	
		return handler.Call<string> ("getVmHeapStats");
	}
		
	/**
	 * Returns the ISO country code of the device.
	 * Availability: Only when user is registered to a network and device supports TelephonyManager(device is a phone).
	 * 
	 * @return country code or "null".
	 */
	public string GetCountryCode ()
	{	
		AndroidJNI.AttachCurrentThread ();		
		return handler.Call<string> ("getCountryCode");
	}
	
}
