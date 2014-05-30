using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FourthSky {
	namespace Android {
		
		public static class AndroidVersions
		{
			public const int BASE = 1;
			public const int BASE_1_1 = 2;
			public const int CUPCAKE = 3;
			public const int DONUT = 4;
			public const int ECLAIR = 5;
			public const int ECLAIR_0_1 = 6;
			public const int ECLAIR_MR1 = 7;
			public const int FROYO = 8;
			public const int GINGERBREAD = 9;
			public const int GINGERBREAD_MR1 = 10;
			public const int HONEYCOMB = 11;
			public const int HONEYCOMB_MR1 = 12;
			public const int HONEYCOMB_MR2 = 13;
			public const int ICE_CREAM_SANDWICH = 14;
			public const int ICE_CREAM_SANDWICH_MR1 = 15;
			public const int JELLY_BEAN = 16;
			public const int JELLY_BEAN_MR1 = 17;
			public const int JELLY_BEAN_MR2 = 18;
			public const int KITKAT = 19;
		}
		
		public static class AndroidSystem {

			// Delegate for activity result
			public delegate void OnActivityResultDelegate(int requestCode, int resultCode, AndroidJavaObject intent);
			public static event OnActivityResultDelegate OnActivityResult;
			
			// Delegate for on new intent
			public delegate void OnNewIntentDelegate(AndroidJavaObject intent);
			public static event OnNewIntentDelegate OnNewIntent;
			
			#region internal callback objects

			private delegate void OnActivityResultInternal(int requestCode, int resultCode, IntPtr intentPtr);
			private static OnActivityResultInternal onActivityResultInternal;
			private static GCHandle onActivityResultInternalHandle;
			private static IntPtr onActivityResultInternalPtr = IntPtr.Zero;
			
			private static void OnActivityResultInternalImpl(int requestCode, int resultCode, IntPtr intentPtr) {
				if (OnActivityResult != null) {
					AndroidJavaObject intent = AndroidSystem.ConstructJavaObjectFromPtr (intentPtr);

					// Invoke callback
					AndroidSystem.OnActivityResult(requestCode, resultCode, intent);

					// Release callback objects, to prevent memory leak
					onActivityResultInternalHandle.Free();
					onActivityResultInternal = null;
				}
			}
			
			private delegate void OnNewIntentInternal(IntPtr intentPtr);
			private static OnNewIntentInternal onNewIntentInternal;
			private static GCHandle onNewIntentInternalHandle;
			private static IntPtr onNewIntentInternalPtr;
			
			private static void OnNewIntentInternalImpl(IntPtr intentPtr) {
				if (OnNewIntent != null) {
					AndroidJavaObject intent = AndroidSystem.ConstructJavaObjectFromPtr (intentPtr);
					
					AndroidSystem.OnNewIntent(intent);

					// Release callback objects, to prevent memory leak
					onNewIntentInternalHandle.Free();
					onNewIntentInternal = null;
				}
			}

			#endregion
			
			// Stores current Android version
			private static int SDK_VERSION = -1;
			
			// Name of activities used for Android System plugin
			internal static readonly string PLUGIN_PACKAGE = "com.fourthsky.unity.androidtools";
			private static readonly string PLUGIN_ACTIVITY = PLUGIN_PACKAGE + ".UnityPlayerActivityEx";
			private static readonly string PLUGIN_NATIVE_ACTIVITY = PLUGIN_PACKAGE + ".UnityPlayerNativeActivityEx";
			//private static readonly string ACTIVITY_CALLBACKS = PLUGIN_PACKAGE + ".UnityActivityCallbacks";
			
			// Android class names
			internal static readonly string UNITY_PLAYER = "com.unity3d.player.UnityPlayer";
			internal static readonly string INTENT = "android.content.Intent";
			internal static readonly string INTENT_FILTER = "android.content.IntentFilter";
			internal static readonly string PARCELABLE = "android.os.Parcelable";
			
			// Constants for return of StartActivityForResult
			public const int RESULT_OK = -1;
			public const int RESULT_CANCELED = 0x0;
			public const int RESULT_FIRST_USER = 0x1;
			
			/// <summary>
			/// Gets the Android version.
			/// </summary>
			/// <value>Android version in device.</value>
			public static int Version {
				get {
#if UNITY_ANDROID
					if (SDK_VERSION == -1)
					{
						using (AndroidJavaClass Build_VERSION = new AndroidJavaClass("android.os.Build$VERSION"))
						{
							SDK_VERSION = Build_VERSION.GetStatic<int>("SDK_INT");
						}
					}
#endif
					return SDK_VERSION;
				}
			}
			
			public static bool ActivityCallbacksSupported {
				get {
#if UNITY_ANDROID
					string className = UnityActivity.Call<AndroidJavaObject>("getClass").Call<string>("getName");
					return (className == PLUGIN_ACTIVITY || className == PLUGIN_NATIVE_ACTIVITY);
#else
					return false;
#endif
				}
			}
			
			public static AndroidJavaObject UnityActivity {
				get {
#if UNITY_ANDROID
					using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(UNITY_PLAYER)) {
						return unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
					}
#else
					return null;				
#endif
				}
			}
			
			public static AndroidJavaObject UnityContext {
				get {
#if UNITY_ANDROID
					using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(UNITY_PLAYER)) {
						using (AndroidJavaObject activityInstance = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
							return activityInstance.Call<AndroidJavaObject>("getApplicationContext");
						}
					}
#else
					return null;				
#endif
				}
			}

			public static string PackageName {
				get {
#if UNITY_ANDROID
					return AndroidSystem.UnityActivity.Call<string>("getPackageName");
#else
					return "";
#endif
				}
			}
			
			// TODO implement
			public static Hashtable ParseBundle(AndroidJavaObject bundleObject) {
				
				return null;
			}
			
			private static bool RegisterOnActivityResultCallback(OnActivityResultDelegate callback) {
#if UNITY_ANDROID

				// Store the callback
				OnActivityResult = callback;

				if (onActivityResultInternalPtr == IntPtr.Zero) {
					// Get native pointer for delegate
					onActivityResultInternal = new OnActivityResultInternal(OnActivityResultInternalImpl);
					onActivityResultInternalHandle = GCHandle.Alloc(onActivityResultInternal);
					onActivityResultInternalPtr = Marshal.GetFunctionPointerForDelegate (onActivityResultInternal);
					
					if (onActivityResultInternalPtr == IntPtr.Zero) {
						Debug.LogError("Cannot get unmanaged pointer for OnActivityResult delegate, pointer value is " + onActivityResultInternalPtr.ToInt32());
						return false;
					}

					return RegisterOnActivityResultCallback(onActivityResultInternalPtr);

				}
#endif

				return false;
			}
			
			public static bool RegisterOnNewIntentCallback(OnNewIntentDelegate callback) {
#if UNITY_ANDROID
				// Store the callback
				OnNewIntent = callback;

				if (onNewIntentInternalPtr == IntPtr.Zero) {
					// Get native pointer for delegate
					onNewIntentInternal = new OnNewIntentInternal(OnNewIntentInternalImpl);
					onNewIntentInternalHandle = GCHandle.Alloc(onNewIntentInternal);
					onNewIntentInternalPtr = Marshal.GetFunctionPointerForDelegate (onNewIntentInternal);

					if (onNewIntentInternalPtr == IntPtr.Zero) {
						Debug.LogError("Cannot get unmanaged pointer for OnNewIntent delegate");
						return false;
					}

					return RegisterOnNewIntentCallback(onNewIntentInternalPtr);
				}
#endif

				return false;
			}

#if UNITY_ANDROID

			[DllImport("unityandroidsystem")]
			public static extern bool RegisterOnActivityResultCallback(IntPtr callbackPtr);

			[DllImport("unityandroidsystem")]
			public static extern bool RegisterOnNewIntentCallback (IntPtr callbackPtr);

#endif

			public static AndroidJavaObject ConstructJavaObjectFromPtr(IntPtr javaPtr) {
#if UNITY_ANDROID
#if UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1
				return new AndroidJavaObject(javaPtr);			
#else
				// Starting with Unity 4.2, AndroidJavaObject constructor with IntPtr arg is private,
				// so, construct using brute force :)
				Type t = typeof(AndroidJavaObject);
				Type[] types = new Type[1];
				types[0] = typeof(IntPtr);
				ConstructorInfo javaObjConstructor = t.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
				
				return javaObjConstructor.Invoke(new object[] { javaPtr }) as AndroidJavaObject;
#endif
#else
				return null;
#endif
			}
			
			public static void SendBroadcast(string action, Hashtable extras = null) {
#if UNITY_ANDROID
				if (!Application.isEditor) {
					using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(AndroidSystem.UNITY_PLAYER)) {
						using (AndroidJavaObject activityInstance = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
							AndroidJavaObject intent = new AndroidJavaObject(AndroidSystem.INTENT, action);
							
							// Add args to intent
							if (extras != null) {
								foreach (DictionaryEntry entry in extras) {
									if (entry.Value is short) {
										intent.Call<AndroidJavaObject>("putExtra", (string)entry.Key, (short)entry.Value);
									} else if (entry.Value is int) {
										intent.Call<AndroidJavaObject>("putExtra", (string)entry.Key, (int)entry.Value);
									} else if (entry.Value is long) {
										intent.Call<AndroidJavaObject>("putExtra", (string)entry.Key, (long)entry.Value);
									} else if (entry.Value is float) {
										intent.Call<AndroidJavaObject>("putExtra", (string)entry.Key, (float)entry.Value);
									} else if (entry.Value is double) {
										intent.Call<AndroidJavaObject>("putExtra", (string)entry.Key, (double)entry.Value);
									} else if (entry.Value is string) {
										intent.Call<AndroidJavaObject>("putExtra", (string)entry.Key, (string)entry.Value);
									} else if (entry.Value is AndroidJavaObject) {
										AndroidJavaObject javaObj = entry.Value as AndroidJavaObject;
										
										using (AndroidJavaClass _Parcelable = new AndroidJavaClass(AndroidSystem.PARCELABLE)) {
											if (AndroidJNI.IsInstanceOf(javaObj.GetRawObject(), _Parcelable.GetRawClass())) {
												intent.Call<AndroidJavaObject>("putExtra", (string)entry.Key, javaObj);
											} else {
												throw new ArgumentException("Argument is not a Android Parcelable", "extra." + entry.Key);
											}
										}
									}
								}
							}
							
							activityInstance.Call("sendBroadcast", intent);
						}
					}
				}
#endif
			}
			
			public static bool StartActivityForResult(string action, int requestCode, OnActivityResultDelegate callback) {
				return StartActivityForResult(action, null, requestCode, callback);
			}
			
			public static bool StartActivityForResult(string action, AndroidJavaObject uriData, int requestCode, OnActivityResultDelegate callback) {
				if (callback == null) {
					throw new System.ArgumentNullException("OnActivityResult callback cannot be null");
				}
				
				if (string.IsNullOrEmpty(action)) {
					throw new System.ArgumentNullException("");
				}
	
#if UNITY_ANDROID
				// Create intent
				AndroidJavaObject intent = uriData != null ? new AndroidJavaObject(AndroidSystem.INTENT, action, uriData)
														   : new AndroidJavaObject(AndroidSystem.INTENT, action);

				// Start given action
				return StartActivityForResult (intent, requestCode, callback);
#else
				return false;
#endif
			}

			public static bool StartActivityForResult(AndroidJavaObject intent, int requestCode, OnActivityResultDelegate callback) {
				bool ret = false;

#if UNITY_ANDROID 
				using (AndroidJavaObject activityInstance = AndroidSystem.UnityActivity) {
					// Register given callback
					ret = RegisterOnActivityResultCallback(callback);

					activityInstance.Call("startActivityForResult", intent, requestCode);
				}
#endif
				
				return ret;
			}
		}
	}
}
