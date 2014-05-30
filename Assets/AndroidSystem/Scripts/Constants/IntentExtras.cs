using System;

namespace FourthSky
{
	namespace Android
	{
		public static class IntentExtras
		{
			
			public static class Bluetooth
			{
				public static readonly string EXTRA_DEVICE = "android.bluetooth.device.extra.DEVICE";
				public static readonly string EXTRA_UUID = "android.bluetooth.device.extra.UUID";
				public static readonly string EXTRA_BOND_STATE = "android.bluetooth.device.extra.BOND_STATE";
			}

			public static class NFC
			{
				public static readonly string EXTRA_NDEF_MESSAGES = "android.nfc.extra.NDEF_MESSAGES";
			}

		}
	}
}