using UnityEngine;
using System;
using System.Collections;
using FourthSky.Android;

public class IInAppBillingService : AndroidServiceBinder {

	private static readonly int TRANSACTION_isBillingSupported = AndroidServiceBinder.FIRST_CALL_TRANSACTION + 0;
	private static readonly int TRANSACTION_getSkuDetails = AndroidServiceBinder.FIRST_CALL_TRANSACTION + 1;
	private static readonly int TRANSACTION_getPurchases = AndroidServiceBinder.FIRST_CALL_TRANSACTION + 2;
	private static readonly int TRANSACTION_consumePurchase = AndroidServiceBinder.FIRST_CALL_TRANSACTION + 3;

	protected IInAppBillingService(AndroidJavaObject binder)
		: base("com.android.vending.billing.IInAppBillingService", binder) {

	}

	public static IInAppBillingService Wrap(AndroidJavaObject binder) {
		return new IInAppBillingService(binder);

	}

	public int IsBillingSupported(int apiVersion, string packageName, string type) {
		int _result = 0;

		if (UseProxy) {
			AndroidJavaObject _data = CreateParcel();
			AndroidJavaObject _reply = CreateParcel();

			try {
				_data.Call("writeInterfaceToken", Descriptor);
				_data.Call("writeInt", apiVersion);
				_data.Call("writeString", packageName);
				_data.Call("writeString", type);

				mJavaObject.Call<bool>("transact", TRANSACTION_isBillingSupported, _data, _reply, 0);
				_reply.Call("readException");

				_result = _reply.Call<int>("readInt");

			} finally {
				_data.Call("recycle");
				_reply.Call("recycle");
			}

		} else {
			return mJavaObject.Call<int>("isBillingSupported", apiVersion, packageName, type);
		}

		return _result;
	}

	public AndroidJavaObject GetSkuDetails(int apiVersion, string packageName, string type, AndroidJavaObject skusBundle) {
		AndroidJavaObject _result = null;

		if (UseProxy) {
			AndroidJavaObject _data = CreateParcel();
			AndroidJavaObject _reply = CreateParcel();

			try {
				_data.Call("writeInterfaceToken", Descriptor);
				_data.Call("writeInt", apiVersion);
				_data.Call("writeString", packageName);
				_data.Call("writeString", type);
				if (skusBundle != null) {
					_data.Call("writeInt", 1);
					skusBundle.Call("writeToParcel", _data, 0);
				} else {
					_data.Call("writeInt", 0);
				}

				mJavaObject.Call<bool>("transact", TRANSACTION_getSkuDetails, _data, _reply, 0);
				_reply.Call("readException");

				if (0 !=_reply.Call<int>("readInt")) {
					using(AndroidJavaClass klazz = new AndroidJavaClass("android.os.Bundle")) {
						AndroidJavaObject CREATOR = klazz.GetStatic<AndroidJavaObject>("CREATOR");
						_result = CREATOR.Call<AndroidJavaObject>("createFromParcel", _reply);
					}
				} else {
					_result = null;
				}

			} finally {
				_data.Call("recycle");
				_reply.Call("recycle");
			}

		} else {
			return mJavaObject.Call<AndroidJavaObject>("getSkuDetails", apiVersion, packageName, type, skusBundle);
		}

		return _result;
	}

	public AndroidJavaObject GetPurchases(int apiVersion, string packageName, string type, string continuationToken) {
		AndroidJavaObject _result = null;

		if (UseProxy) {
			AndroidJavaObject _data = CreateParcel();
			AndroidJavaObject _reply = CreateParcel();

			try {
				_data.Call("writeInterfaceToken", Descriptor);
				_data.Call("writeInt", apiVersion);
				_data.Call("writeString", packageName);
				_data.Call("writeString", type);
				_data.Call("writeString", continuationToken);

				mJavaObject.Call<bool>("transact", TRANSACTION_getPurchases, _data, _reply, 0);
				_reply.Call("readException");

				if (0 !=_reply.Call<int>("readInt")) {
					using(AndroidJavaClass klazz = new AndroidJavaClass("android.os.Bundle")) {
						AndroidJavaObject CREATOR = klazz.GetStatic<AndroidJavaObject>("CREATOR");
						_result = CREATOR.Call<AndroidJavaObject>("createFromParcel", _reply);
					}
				} else {
					_result = null;
				}

			} finally {
				_data.Call("recycle");
				_reply.Call("recycle");
			}

		} else {
			return mJavaObject.Call<AndroidJavaObject>("getPurchases", apiVersion, packageName, type, continuationToken);
		}

		return _result;
	}

	public int ConsumePurchase(int apiVersion, string packageName, string purchaseToken) {
		int _result = 0;

		if (UseProxy) {
			AndroidJavaObject _data = CreateParcel();
			AndroidJavaObject _reply = CreateParcel();

			try {
				_data.Call("writeInterfaceToken", Descriptor);
				_data.Call("writeInt", apiVersion);
				_data.Call("writeString", packageName);
				_data.Call("writeString", purchaseToken);

				mJavaObject.Call<bool>("transact", TRANSACTION_consumePurchase, _data, _reply, 0);
				_reply.Call("readException");

				_result = _reply.Call<int>("readInt");

			} finally {
				_data.Call("recycle");
				_reply.Call("recycle");
			}

		} else {
			return mJavaObject.Call<int>("consumePurchase", apiVersion, packageName, purchaseToken);
		}

		return _result;
	}

}

