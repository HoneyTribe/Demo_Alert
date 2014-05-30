using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace FourthSky {
	namespace Android {
		
		/// <summary>
		/// Broadcast receiver.
		/// </summary>
		public class BroadcastReceiver : AndroidWrapper {
		
			/// <summary>
			/// On receive delegate.
			/// </summary>
			public delegate void OnReceiveDelegate(AndroidJavaObject contextPtr, AndroidJavaObject intentPtr);

			public event OnReceiveDelegate OnReceive;
			
			public bool mRegistered;
			public bool Registered {
				get {
					return mRegistered;
				}
			}

#region internal delegate 

			private delegate void OnReceiveInternal(IntPtr contextPtr, IntPtr intentPtr);

			private OnReceiveInternal m_OnReceiveInternal;
			private GCHandle onReceiveInternalHandle;
			private IntPtr m_OnReceiveInternalPtr;

			void OnReceiveInternalImpl(IntPtr contextPtr, IntPtr intentPtr) {
				if (OnReceive != null) {
					AndroidJavaObject context = AndroidSystem.ConstructJavaObjectFromPtr (contextPtr);
					AndroidJavaObject intent = AndroidSystem.ConstructJavaObjectFromPtr (intentPtr);

					OnReceive (context, intent);
				}
			}

#endregion
			
			public BroadcastReceiver() : base() {
				
			}
			
			public void Register(params string[] actions) {
#if UNITY_ANDROID
				// Create java instance of broadcast receiver
				if (mJavaObject == null) {
					// Prepare callback delegate
					m_OnReceiveInternal = new OnReceiveInternal(OnReceiveInternalImpl);
					onReceiveInternalHandle = GCHandle.Alloc(m_OnReceiveInternal);
					m_OnReceiveInternalPtr = Marshal.GetFunctionPointerForDelegate(m_OnReceiveInternal);

					IntPtr receiverPtr = CreateJavaBroadcastReceiver(m_OnReceiveInternalPtr);
					if (IntPtr.Zero != receiverPtr) {
						mJavaObject = AndroidSystem.ConstructJavaObjectFromPtr(receiverPtr);
					
					} else {
						Debug.Log("Could not create broadcast receiver");
					}
				}

				using (AndroidJavaObject activityInstance = AndroidSystem.UnityActivity) {
					using (AndroidJavaObject intentFilter = new AndroidJavaObject(AndroidSystem.INTENT_FILTER)) {
						
						// Configuring actions to receiver
						foreach (string s in actions) {
							intentFilter.Call("addAction", s);
						}
						
						// Register broadcast receiver
						try {
							activityInstance.Call<AndroidJavaObject>("registerReceiver", mJavaObject, intentFilter);
						
						} catch (Exception e) {
							string message = e.Message;

							// I believe this exception occurs due to a bug in AndroidJavaObject class,
							// because everything is OK after that, so I will ignore
							if (message.Contains("Init'd AndroidJavaObject with null ptr")) {
								Debug.Log("Exception registering Broadcast Receiver, ignore: " + message);
							
							} else {
								throw e;
							}
						}
					}
				}
				
				mRegistered = true;
#endif
				
			}
			
			public void Unregister() {	
#if UNITY_ANDROID
				if (mRegistered && mJavaObject != null) {
					using (AndroidJavaObject activityInstance = AndroidSystem.UnityActivity) {
							
						// Unregister broadcast receiver
						activityInstance.Call("unregisterReceiver", mJavaObject);

						// Release java broadcast receiver
						mJavaObject.Dispose();
						mJavaObject = null; 

						// Free internal delegate objects
						onReceiveInternalHandle.Free();
						m_OnReceiveInternal = null;

					}
				
					mRegistered = false;
				}
#endif
			}

			public int GetResultCode()
			{
#if UNITY_ANDROID
				if (mJavaObject != null)
				{
					return mJavaObject.Call<int>("getResultCode");
				}
#endif
				
				return -1;
			}
			
			public void AbortBroadcast()
			{
#if UNITY_ANDROID
				if (mJavaObject != null)
				{
					mJavaObject.Call("abortBroadcast");
				}
#endif
			}
			
			public void ClearAbortBroadcast()
			{
#if UNITY_ANDROID
				if (mJavaObject != null)
				{
					mJavaObject.Call("clearAbortBroadcast");
				}
#endif
			}
			
			public bool GetAbortBroadcast()
			{
#if UNITY_ANDROID
				if (mJavaObject != null)
				{
					return mJavaObject.Call<bool>("getAbortBroadcast");
				}
#endif
				return false;
			}
			
			protected override void Dispose(bool disposing) {
				if (!disposed) {
				
					if (disposing) {
						Unregister();
					}
				}
				
				base.Dispose(disposing);
			}
			
			[DllImport("unityandroidsystem")]
			private static extern IntPtr CreateJavaBroadcastReceiver(IntPtr callback);
		}
		
	}
}