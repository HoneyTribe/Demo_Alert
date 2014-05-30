using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace FourthSky {
	namespace Android {

		public class ServiceConnection : AndroidWrapper {
			
			// Delegates for service connection
			public delegate void OnServiceConnectedDelegate(AndroidJavaObject componentName, AndroidJavaObject binder);
			public delegate void OnServiceDisconnectedDelegate(AndroidJavaObject componentName);
			
			public event OnServiceConnectedDelegate OnServiceConnected;
			public event OnServiceDisconnectedDelegate OnServiceDisconnected;

#region internal delegate

			// Delegates for service connection
			private delegate void OnServiceConnectedInternal(IntPtr componentNamePtr, IntPtr binderPtr);
			private delegate void OnServiceDisconnectedInternal(IntPtr componentNamePtr);
			
			private OnServiceConnectedInternal onServiceConnectedInternal;
			private GCHandle onServiceConnectedHandle;
			private IntPtr onServiceConnectedPtr;

			void OnServiceConnectedInternalImpl(IntPtr componentNamePtr, IntPtr binderPtr) {
				if (OnServiceConnected != null) {
					if (OnServiceConnected != null) {
						AndroidJavaObject componentName = AndroidSystem.ConstructJavaObjectFromPtr (componentNamePtr);
						AndroidJavaObject binder = AndroidSystem.ConstructJavaObjectFromPtr (binderPtr);

					
						OnServiceConnected(componentName, binder);
					}
				}
			}

			private OnServiceDisconnectedInternal onServiceDisconnectedInternal;
			private GCHandle onServiceDisconnectedHandle;
			private IntPtr onServiceDisconnectedPtr;

			void OnServiceDisconnectedInternalImpl(IntPtr componentNamePtr) {
				if (OnServiceDisconnected != null) {
					if (OnServiceDisconnected != null) {
						AndroidJavaObject componentName = AndroidSystem.ConstructJavaObjectFromPtr (componentNamePtr);
						
						OnServiceDisconnected(componentName);
					}
				}
			}

#endregion
			
			// Use this for initialization
			public ServiceConnection() : base() {
				
			}
			
			~ServiceConnection() {
				Dispose (false);
			}
			
			/// <summary>
			/// Creates the service connection.
			/// </summary>
			/// <returns>
			/// The service connection.
			/// </returns>
			/// <param name='onConnectedClbk'>
			/// On connected clbk.
			/// </param>
			/// <param name='onDisconnectedClbk'>
			/// On disconnected clbk.
			/// </param>
			/// <exception cref='System.ArgumentNullException'>
			/// Is thrown when the argument null exception.
			/// </exception>
			public bool Bind(AndroidJavaClass serviceClass, int flags = /*BIND_AUTO_CREATED*/1) {
				bool retValue = false;
				
#if UNITY_ANDROID
				// Create java instance of broadcast receiver
				if (mJavaObject == null) {
					onServiceConnectedInternal = new OnServiceConnectedInternal(OnServiceConnectedInternalImpl);
					onServiceConnectedHandle = GCHandle.Alloc(onServiceConnectedInternal);
					onServiceConnectedPtr = Marshal.GetFunctionPointerForDelegate(onServiceConnectedInternal);
					
					onServiceDisconnectedInternal = new OnServiceDisconnectedInternal(OnServiceDisconnectedInternalImpl);
					onServiceDisconnectedHandle = GCHandle.Alloc(onServiceDisconnectedInternal);
					onServiceDisconnectedPtr = Marshal.GetFunctionPointerForDelegate(onServiceDisconnectedInternal);
				
					IntPtr connPtr = CreateJavaServiceConnection(onServiceConnectedPtr, onServiceDisconnectedPtr);
					if (IntPtr.Zero != connPtr) {
						mJavaObject = AndroidSystem.ConstructJavaObjectFromPtr(connPtr);
					}
				}
				
				// Get application context
				using (AndroidJavaObject context = AndroidSystem.UnityContext) {
					using(AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", context, serviceClass)) {
					
						// Now, bind to service
						retValue = context.Call<bool>("bindService", intent, mJavaObject, flags);
					}
				}
#endif
			
				return retValue;
			}
			
			/// <summary>
			/// Binds to the Android service.
			/// </summary>
			/// <returns>
			/// The service.
			/// </returns>
			/// <param name='intent'>
			/// If set to <c>true</c> intent.
			/// </param>
			/// <param name='serviceConnection'>
			/// If set to <c>true</c> service connection.
			/// </param>
			/// <param name='flags'>
			/// If set to <c>true</c> flags.
			/// </param>
			/// <exception cref='System.ArgumentNullException'>
			/// Is thrown when the argument null exception.
			/// </exception>
			public bool Bind(string action, int flags = /*Context.BIND_AUTO_CREATE*/1) {
				if (action == null || "" == action) {
					throw new System.ArgumentNullException("Intent action cannot be null");
				}
				
				bool retValue = false;
				
#if UNITY_ANDROID
				// Create java instance of broadcast receiver
				if (mJavaObject == null) {
					onServiceConnectedInternal = new OnServiceConnectedInternal(OnServiceConnectedInternalImpl);
					onServiceConnectedHandle = GCHandle.Alloc(onServiceConnectedInternal);
					onServiceConnectedPtr = Marshal.GetFunctionPointerForDelegate(onServiceConnectedInternal);
					
					onServiceDisconnectedInternal = new OnServiceDisconnectedInternal(OnServiceDisconnectedInternalImpl);
					onServiceDisconnectedHandle = GCHandle.Alloc(onServiceDisconnectedInternal);
					onServiceDisconnectedPtr = Marshal.GetFunctionPointerForDelegate(onServiceDisconnectedInternal);
				
					IntPtr connPtr = CreateJavaServiceConnection(onServiceConnectedPtr, onServiceDisconnectedPtr);
					if (IntPtr.Zero != connPtr) {
						mJavaObject = AndroidSystem.ConstructJavaObjectFromPtr(connPtr);
					}
				}
				
				// bind to the service
				using (AndroidJavaObject activityInstance = AndroidSystem.UnityActivity) {
					using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", action)) {
						retValue = activityInstance.Call<bool>("bindService", intent, mJavaObject, flags);
					}
				}
#endif
				
				return retValue;
			}
			
			/// <summary>
			/// Unbinds from the Android service.
			/// </summary>
			/// <param name='serviceConnection'>
			/// Service connection.
			/// </param>
			/// <exception cref='System.ArgumentNullException'>
			/// Is thrown when the argument null exception.
			/// </exception>
			public void Unbind() {
#if UNITY_ANDROID
				if (mJavaObject == null &&!Application.isEditor) {
					throw new System.ArgumentNullException("Service connection cannot be null");
				}
				
				using (AndroidJavaObject activityInstance = AndroidSystem.UnityActivity) {
					activityInstance.Call("unbindService", mJavaObject);
				}
				
				
				onServiceConnectedHandle.Free();
				onServiceDisconnectedHandle.Free();
				
				onServiceConnectedInternal = null;
				onServiceDisconnectedInternal = null;
#endif
			}
			
			protected override void Dispose(bool disposing) {
				if (!disposed) {
				
					if (disposing) {
						Unbind();
					}
				}
				
				base.Dispose(disposing);
			}
			
#if UNITY_ANDROID
			/// <summary>
			/// Native function used to create Android service connection
			/// </summary>
			/// <returns>
			/// Object pointer to Android service connection.
			/// </returns>
			/// <param name='onConnectedClbk'>
			/// On connected clbk.
			/// </param>
			/// <param name='onDisconnectedClbk'>
			/// On disconnected clbk.
			/// </param>
			[DllImport("unityandroidsystem")]
			private static extern IntPtr CreateJavaServiceConnection(IntPtr onConnectedClbk, 
																	 IntPtr onDisconnectedClbk);
#endif
		}
		
	}
}