using System;
using UnityEngine;
using FourthSky.Android;

namespace FourthSky
{
	namespace Android
	{
		namespace Services
		{
			public static class NFC
			{
				#region public methods
				
				public static bool Supported {
					get {
#if UNITY_ANDROID
						if (NfcAdapter != null && NfcAdapter.GetRawObject() != IntPtr.Zero) {
							return true;
						}
#endif
						Debug.LogError("NFC adapter not present in this device");
						return false;
					}
				}
				
				public static bool Enabled {
					get {
#if UNITY_ANDROID
						bool supported = NFC.Supported;
						if (!supported) {
							return false;
						}
						
						bool enabled = NfcAdapter.Call<bool>("isEnabled");
						if (!enabled) {
							Debug.LogError("NFC Adapter is disabled");
							return false;
						}
						
						if (AndroidSystem.Version >= AndroidVersions.JELLY_BEAN) {
							enabled = NfcAdapter.Call<bool>("isNdefPushEnabled");
							if (!enabled) {
								Debug.LogError("Android Bean is disabled");
								return false;
							}
						}
						
						return true;
#else
						return false;
#endif
					}
				}
				
				public static bool Publish(string message, Action sentCallback = null)
				{
#if UNITY_ANDROID
					// Register callback to listen when messages was sent
					RegisterOnPushCompletedCallback(sentCallback);

					return InternalPublish(CreateTextMessage(message));
#else
					return false;
#endif
				}

				public static bool Publish(byte[] data, Action sentCallback = null)
				{
#if UNITY_ANDROID
					// Register callback to listen when messages was sent
					RegisterOnPushCompletedCallback(sentCallback);

					return InternalPublish(CreateBinaryMessage(data));
#else
					return false;
#endif
				}
				
				public static void CancelPublish() {
#if UNITY_ANDROID
					IntPtr method = AndroidJNIHelper.GetMethodID(NfcAdapter.GetRawClass(), "setNdefPushMessage");
					jvalue[] args = new jvalue[2];
					args [0].l = IntPtr.Zero;
					args [1].l = IntPtr.Zero;
					AndroidJNI.CallVoidMethod (NfcAdapter.GetRawObject(), 
					                           method, 
					                           args);
#endif
				}
				
				public static bool Subscribe(Action<string> callback)
				{
					// Store callback
					NFC.stringCallback = callback;
#if UNITY_ANDROID					
					// Register for NFC event
					AndroidSystem.RegisterOnNewIntentCallback (OnNewIntent);
					return true;
#else
					return false;
#endif
				}
				
				public static bool Subscribe(Action<byte[]> callback)
				{
					// Store callback
					NFC.binaryCallback = callback;
#if UNITY_ANDROID
					// Register for NFC event
					AndroidSystem.RegisterOnNewIntentCallback (OnNewIntent);
					return true;
#else
					return false;
#endif
				}
				
				#endregion
				
				#region private variables and constants
				private static readonly string NFC_PKG = "android.nfc";
				private static readonly string NFC_ADAPTER = NFC_PKG + ".NfcAdapter";
				private static readonly string NDEF_RECORD = NFC_PKG + ".NdefRecord";
				private static readonly string NDEF_MESSAGE = NFC_PKG + ".NdefMessage";
				
				public static readonly short TNF_EMPTY = 0;
				public static readonly short TNF_WELL_KNOWN = 1;
				public static readonly short TNF_MIME_MEDIA = 2;
				public static readonly short TNF_ABSOLUTE_URI = 3;
				public static readonly short TNF_EXTERNAL_TYPE = 4;
				public static readonly short TNF_UNKNOWN = 5;
				public static readonly short TNF_UNCHANGED = 6;
				
				public static readonly byte[] RTD_TEXT = { 0x54 };  // "T"
				public static readonly byte[] RTD_URI = { 0x55 };   // "U"
				public static readonly byte[] RTD_SMART_POSTER = { 0x53, 0x70 };  // "Sp"
				public static readonly byte[] RTD_ALTERNATIVE_CARRIER = { 0x61, 0x63 };  // "ac"
				public static readonly byte[] RTD_HANDOVER_CARRIER = { 0x48, 0x63 };  // "Hc"
				public static readonly byte[] RTD_HANDOVER_REQUEST = { 0x48, 0x72 };  // "Hr"
				public static readonly byte[] RTD_HANDOVER_SELECT = { 0x48, 0x73 }; // "Hs"
				public static readonly byte[] RTD_ANDROID_APP = System.Text.Encoding.UTF8.GetBytes("android.com:pkg");

				private static Action messageSentCallback;
				private static Action<string> stringCallback;
				private static Action<byte[]> binaryCallback;

#if UNITY_ANDROID
				private static AndroidJavaObject mNfcAdapter;
				private static IntPtr pushCallbackProxy;
				private static NdefPushCompleteCallback pushCallback;

				private class NdefPushCompleteCallback : AndroidJavaProxy {
					
					public NdefPushCompleteCallback() 
					: base("android.nfc.NfcAdapter$OnNdefPushCompleteCallback") {
						
					}
					
					void onNdefPushComplete(/*NfcEvent*/ AndroidJavaObject evt) {
						if (NFC.messageSentCallback != null) {
							NFC.messageSentCallback.Invoke();
						}
						
						NFC.messageSentCallback = null;
					}				
				}

				private class NdefMessageCallback : AndroidJavaProxy {

					public NdefMessageCallback() 
						: base("android.nfc.NfcAdapter$CreateNdefMessageCallback") {

					}

					AndroidJavaObject createNdefMessage(AndroidJavaObject evt) {



						AndroidJavaObject ndefMessage = new AndroidJavaObject("android.nfc.NdefMessage");
						return ndefMessage;
					}

				}
#endif

				#endregion
				
				#region private methods

#if UNITY_ANDROID
				private static AndroidJavaObject NfcAdapter {
					get {
						if (mNfcAdapter == null) {
							using (AndroidJavaClass NfcAdapter = new AndroidJavaClass(NFC_ADAPTER)) {
								mNfcAdapter = NfcAdapter.CallStatic<AndroidJavaObject>("getDefaultAdapter", AndroidSystem.UnityContext);
							}
						}
						
						return mNfcAdapter;
					}
				}

				private static AndroidJavaObject CreateTextMessage(string text) {
					// Publish message 
					AndroidJavaObject record = new AndroidJavaObject (NDEF_RECORD, 
					                                                  TNF_MIME_MEDIA,
					                                                  //System.Text.Encoding.UTF8.GetBytes ("application/" + AndroidSystem.PackageName),
					                                                  System.Text.Encoding.UTF8.GetBytes("application/com.fourthsky.androidsystem"),
					                                                  new byte[0],
					                                                  System.Text.Encoding.UTF8.GetBytes (text));
					
					// Create message with records
					AndroidJavaObject[] records = { record };
					AndroidJavaObject ndefMessage = new AndroidJavaObject(NDEF_MESSAGE, new object[] { records });

					return ndefMessage;
				}

				private static AndroidJavaObject CreateBinaryMessage(byte[] data) {
					// Create binary record
					AndroidJavaObject record = new AndroidJavaObject(NDEF_RECORD, 
					                                                 TNF_UNKNOWN,
					                                                 null,
					                                                 null,
					                                                 data);
					
					// Create message with records
					AndroidJavaObject[] records = { record };
					AndroidJavaObject ndefMessage = new AndroidJavaObject(NDEF_MESSAGE, records);

					return ndefMessage;
				}

				private static bool InternalPublish(AndroidJavaObject message) {
					try {
						jvalue [] args = { new jvalue(), new jvalue(), new jvalue() };
						args[0].l = message.GetRawObject();
						args[1].l = AndroidSystem.UnityActivity.GetRawObject();
						args[2].l = AndroidJNI.NewObjectArray(1,
						                                      AndroidJNI.FindClass("android/app/Activity"),
						                                      AndroidSystem.UnityActivity.GetRawObject());
						IntPtr method = AndroidJNIHelper.GetMethodID(NfcAdapter.GetRawClass(), 
						                                             "setNdefPushMessage",
						                                             "(Landroid/nfc/NdefMessage;Landroid/app/Activity;[Landroid/app/Activity;)V");
						AndroidJNI.CallVoidMethod (NfcAdapter.GetRawObject(), 
						                           method, 
						                           args);

						return true;
					
					} catch (Exception ex) {
						Debug.LogError("Error publishing NFC message: " + ex.Message);

						return false;
					}
				}

				private static void RegisterOnPushCompletedCallback(Action sentCallback = null) {

					messageSentCallback = sentCallback;

					try {
						// Create push completed callback
						if (pushCallback == null || pushCallbackProxy == IntPtr.Zero) {
							pushCallback = new NdefPushCompleteCallback();
							pushCallbackProxy = AndroidJNIHelper.CreateJavaProxy(pushCallback);

							// Prepare arguments
							jvalue[] args = { new jvalue (), new jvalue (), new jvalue () };
							args[0].l = pushCallbackProxy;
							args[1].l = AndroidSystem.UnityActivity.GetRawObject();
							args[2].l = AndroidJNI.NewObjectArray(1,
							                                      AndroidJNI.FindClass("android/app/Activity"),
							                                      AndroidSystem.UnityActivity.GetRawObject());
							// Register push callback
							IntPtr method = AndroidJNIHelper.GetMethodID(NfcAdapter.GetRawClass(), 
							                                             "setOnNdefPushCompleteCallback", 
							                                             "(Landroid/nfc/NfcAdapter$OnNdefPushCompleteCallback;Landroid/app/Activity;[Landroid/app/Activity;)V");
							AndroidJNI.CallVoidMethod (NfcAdapter.GetRawObject(), 
							                           method, 
							                           args);
						}
					
					} catch (Exception ex) {
						Debug.LogError("Error registering OnPushCompleted callback: " + ex.Message);
					}
				}

				private static void OnNewIntent(AndroidJavaObject intent) {
					try {
						// Get NdefMessages from received intent
						AndroidJavaObject[] rawMsgs = intent.Call<AndroidJavaObject[]> ("getParcelableArrayExtra", IntentExtras.NFC.EXTRA_NDEF_MESSAGES); 
						if (rawMsgs != null) {
							
							// Get first (and maybe only) NdefMessage
							AndroidJavaObject msg = rawMsgs [0];
							if (msg != null) {
								
								// Get records for message
								AndroidJavaObject[] records = msg.Call<AndroidJavaObject[]> ("getRecords");
								if (records != null) {
									
									// Get first message
									AndroidJavaObject record = records [0];
									if (record != null) {

										// Get text from message
										byte[] payload = record.Call<byte[]>("getPayload");

										// Get type of record
										byte[] type = record.Call<byte[]>("getType");
										if (type == null) {
											// If no type, call binary callback
											binaryCallback.Invoke(payload);
										
										} else {

											string uri = System.Text.Encoding.UTF8.GetString(type);

											if ("application/com.fourthsky.androidsystem" == uri) {
												if (stringCallback != null) {
													string text = System.Text.Encoding.UTF8.GetString(payload);
													stringCallback.Invoke(text);
												
												} else {
													binaryCallback.Invoke(payload);

												}
											}
										}
									}
								}								
							}
						}
						
					} catch (Exception e) {
						Debug.LogError(e.Message);						
					}
				}
#endif
				
				#endregion
			}
		}
	}
}