using System;
using UnityEngine;

namespace FourthSky
{
	namespace Android
	{
		namespace Services
		{
			
			public static class Telephony
			{
				#region public methods

				public static bool SendSMS(string phoneNumber, string message, Action<bool> sentCallback, Action<bool> deliveredCallback)
				{
#if UNITY_ANDROID
					AndroidJavaObject sentPendingIntent = null;
					AndroidJavaObject deliveredPendingIntent = null;
					
					// Only register broadcast receiver if callbacks if given
					if (sentCallback != null || deliveredCallback != null)
					{
						// Store callbacks
						smsSentCallback = sentCallback;
						smsDeliveredCallback = deliveredCallback;
						
						// Prepare broadcast receiver
						if (smsReceiver == null)
						{
							smsReceiver = new BroadcastReceiver();
							smsReceiver.OnReceive += OnReceive;
							
							// Register to receive events
							smsReceiver.Register(SMS_SENT, SMS_DELIVERED);
						}
						
						// Create pending intent to trigger broadcast
						using (AndroidJavaClass PendingIntent = new AndroidJavaClass("android.app.PendingIntent"))
						{
							if (smsSentCallback != null)
							{
								sentPendingIntent = PendingIntent.CallStatic<AndroidJavaObject>("getBroadcast", 
								                                                                AndroidSystem.UnityContext,
								                                                                0,
								                                                                new AndroidJavaObject(AndroidSystem.INTENT, SMS_SENT),
								                                                                0);
							}
							
							if (smsDeliveredCallback != null)
							{
								deliveredPendingIntent = PendingIntent.CallStatic<AndroidJavaObject>("getBroadcast", 
								                                                                     AndroidSystem.UnityContext,
								                                                                     0,
								                                                                     new AndroidJavaObject(AndroidSystem.INTENT, SMS_DELIVERED),
								                                                                     0);
							}
						}
					}
					
					// Send the message
					if (message.Length <= 160)
					{
						SmsManager.Call("sendTextMessage",	phoneNumber, 
						                null, 
						                message, 
						                sentPendingIntent,
						                deliveredPendingIntent);
						
						// Dispose the C# peers, it's already referenced by Android
						sentPendingIntent.Dispose();
						deliveredPendingIntent.Dispose();
					}
					else
					{
						// If message is greater than 160, divide the message in multiple parts
						Debug.Log("Message has more than 160 characters, don't implemented yet");
						return false;
					}
					
					return true;
#else
					return false;
#endif
					
				}
				
				/// <summary>
				/// Makes the a phone call.
				/// </summary>
				/// <returns><c>true</c>, if call was made, <c>false</c> otherwise.</returns>
				/// <param name="phoneNumber">Phone number.</param>
				/// <param name="callImmediately">If set to <c>true</c> call immediately.</param>
				public static bool MakeCall(string phoneNumber, bool callImmediately = true)
				{
#if UNITY_ANDROID
					// Fill intent
					AndroidJavaObject intent = new AndroidJavaObject (AndroidSystem.INTENT, 
					                                                  callImmediately ? ActivityActions.ACTION_CALL
					                                                  : ActivityActions.ACTION_DIAL);
					using (AndroidJavaClass Uri = new AndroidJavaClass("android.net.Uri"))
					{
						intent.Call<AndroidJavaObject>("setData", Uri.CallStatic<AndroidJavaObject>("parse", "tel:" + phoneNumber));
					}
					
					// Call dialer
					AndroidSystem.UnityActivity.Call ("startActivity", intent);
					
					return true;
#else
					return false;
#endif
				}
				
				#endregion
				
				#region private variables
				private const int MAKE_CALL_ID = 999813;
				
				private static readonly string SMS_SENT = "SMS_SENT";
				private static readonly string SMS_DELIVERED = "SMS_DELIVERED";
				
				// Broadcast receiver to handle sms events
				private static BroadcastReceiver smsReceiver;

				private static Action<bool> smsSentCallback = null;
				private static Action<bool> smsDeliveredCallback = null;

#if UNITY_ANDROID
				// android.telephony.SmsManager object
				private static AndroidJavaObject mSmsManager;
#endif
				
				#endregion
				
				
				#region private methods
	
#if UNITY_ANDROID
				private static AndroidJavaObject SmsManager {
					get {
						if (mSmsManager == null)
						{
							using(AndroidJavaClass SmsManager = new AndroidJavaClass("android.telephony.SmsManager"))
							{
								mSmsManager = SmsManager.CallStatic<AndroidJavaObject>("getDefault");
								if (mSmsManager == null || mSmsManager.GetRawObject().ToInt32() == 0) {
									return null;
								}
							}
						}
						
						return mSmsManager;
					}
				}
				
				/// <summary>
				/// Handles SMS sent and delivered broadcasts
				/// </summary>
				/// <param name="context">activity where broadcast receiver was registered</param>
				/// <param name="intent">Intent containing info about broadcast</param>
				private static void OnReceive(AndroidJavaObject context, AndroidJavaObject intent)
				{
					string action = intent.Call<string> ("getAction");
					
					// Get result code 
					int resultCode = smsReceiver.GetResultCode ();
					
					if ( SMS_SENT == action )
					{
						if (smsSentCallback != null)
							smsSentCallback.Invoke((resultCode == AndroidSystem.RESULT_OK));
					}
					else if ( SMS_DELIVERED == action )
					{
						if (smsDeliveredCallback != null)
							smsDeliveredCallback.Invoke((resultCode == AndroidSystem.RESULT_OK));
					}
				}

#endif
				
				#endregion
			}
		}
	}
}	