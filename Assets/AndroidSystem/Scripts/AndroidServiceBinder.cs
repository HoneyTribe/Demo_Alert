using UnityEngine;
using System;
using System.Collections;

namespace FourthSky {
	namespace Android {
		
		public abstract class AndroidServiceBinder : AndroidWrapper {
		
			protected static readonly int FIRST_CALL_TRANSACTION = 1;
			
			private static AndroidJavaClass ParcelClass = null;
			private static AndroidJavaClass BundleClass = null;
						
			protected readonly string mDescriptor;
			protected bool mUseProxy;
			
			
			protected static AndroidJavaClass Parcel {
				get {
					if (ParcelClass == null) {
						ParcelClass = new AndroidJavaClass("android.os.Parcel");
					}					
					
					return ParcelClass;
				}
			}
			
			protected static AndroidJavaClass Bundle {
				get {
					if (BundleClass == null) {
						BundleClass = new AndroidJavaClass("android.os.Bundle");
					}					
					
					return BundleClass;
				}
			}
			
			public string Descriptor {
				get {
					return mDescriptor;
				}
			}
			
			public bool UseProxy {
				get {
					return mUseProxy;
				}
			}
			
			protected AndroidServiceBinder(string descriptor, IntPtr binderPtr) {
				mDescriptor = descriptor;
				mJavaObject = AndroidSystem.ConstructJavaObjectFromPtr(binderPtr);
				
				Initialize();
			}
			
			protected AndroidServiceBinder(string descriptor, AndroidJavaObject binder) {
				mDescriptor = descriptor;
				mJavaObject = binder;
								
				Initialize();
			}
			
			~AndroidServiceBinder() {
				Dispose (false);
			}
			
			private void Initialize() {
				// Check if methods are called by transactions or directly
				Debug.Log("Querying for " + mDescriptor + " interface from binder object");
				/*
				using (AndroidJavaObject iInterface = mJavaObject.Call<AndroidJavaObject>("queryLocalInterface", mDescriptor)) {
					if (iInterface == null || iInterface.GetRawObject() == IntPtr.Zero) {
						Debug.Log(mDescriptor + " local interface query from binder object failed, use transactions");
						mUseProxy = true;
						
					} else {
						try {
							AndroidJavaClass klazz = new AndroidJavaClass(mDescriptor);
							if (klazz == null || klazz.GetRawClass() == IntPtr.Zero) {
								Debug.LogError(mDescriptor + " class not found, use transactions");
								mUseProxy = true;
								
							} else {
								Debug.Log("Test if local interface is an instance of " + mDescriptor + " class");
								if (AndroidJNI.IsInstanceOf(iInterface.GetRawObject(), klazz.GetRawClass())) {
									mUseProxy = false;
								} else {
									mUseProxy = true;
									
								}		
								
							}
					
						} catch (Exception) {
							Debug.LogError(mDescriptor + " class not found, use transactions");
							mUseProxy = true;
						}
					}
				}
				*/
				
				try {
					AndroidJavaObject iInterface = mJavaObject.Call<AndroidJavaObject>("queryLocalInterface", mDescriptor);
					
					// Unity 4.1 and before don't catch the exception if internal Android object pointer is null, 
					// so force flow to exception block 
					if (iInterface == null || iInterface.GetRawObject() == IntPtr.Zero) 
						throw new Exception(mDescriptor + " local interface query from binder object failed, use transactions"); 
					
					
					AndroidJavaClass klazz = new AndroidJavaClass(mDescriptor);
					if (klazz == null || klazz.GetRawClass() == IntPtr.Zero) {
						throw new Exception(mDescriptor + " class not found, use transactions"); 
						
					} else {
						Debug.Log("Test if local interface is an instance of " + mDescriptor + " class");
						if (AndroidJNI.IsInstanceOf(iInterface.GetRawObject(), klazz.GetRawClass())) {
							mUseProxy = false;
						} else {
							mUseProxy = true;
							
						}							
					}
				
				} catch (Exception ex) {
					Debug.Log(ex.Message);
					mUseProxy = true;
				}					
					
			}
			
			protected static AndroidJavaObject CreateParcel() {
				return Parcel.CallStatic<AndroidJavaObject>("obtain");
			}
			
			protected static AndroidJavaObject CreateBundleFromParcel(AndroidJavaObject parcel) {
				using (AndroidJavaObject CREATOR = Bundle.GetStatic<AndroidJavaObject>("CREATOR")) {
					return CREATOR.Call<AndroidJavaObject>("createFromParcel", parcel);						
				}
			}
			
		}
	}
}