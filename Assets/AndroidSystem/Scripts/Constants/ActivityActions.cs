using System;
namespace FourthSky
{
	namespace Android
	{
		public static class ActivityActions
		{
			public static readonly string ACTION_ALL_APPS = "android.intent.action.ALL_APPS";
			public static readonly string ACTION_ANSWER = "android.intent.action.ANSWER";
			public static readonly string ACTION_APP_ERROR = "android.intent.action.APP_ERROR";
			public static readonly string ACTION_ASSIST = "android.intent.action.ASSIST";
			public static readonly string ACTION_ATTACH_DATA = "android.intent.action.ATTACH_DATA";
			public static readonly string ACTION_BUG_REPORT = "android.intent.action.BUG_REPORT";
			public static readonly string ACTION_CALL = "android.intent.action.CALL";
			public static readonly string ACTION_CALL_BUTTON = "android.intent.action.CALL_BUTTON";
			public static readonly string ACTION_CHOOSER = "android.intent.action.CHOOSER";
			public static readonly string ACTION_CREATE_DOCUMENT = "android.intent.action.CREATE_DOCUMENT";
			public static readonly string ACTION_CREATE_LIVE_FOLDER = "android.intent.action.CREATE_LIVE_FOLDER";
			public static readonly string ACTION_CREATE_SHORTCUT = "android.intent.action.CREATE_SHORTCUT";
			public static readonly string ACTION_DELETE = "android.intent.action.DELETE";
			public static readonly string ACTION_DIAL = "android.intent.action.DIAL";
			public static readonly string ACTION_EDIT = "android.intent.action.EDIT";
			public static readonly string ACTION_EVENT_REMINDER = "android.intent.action.EVENT_REMINDER";
			public static readonly string ACTION_GET_CONTENT = "android.intent.action.GET_CONTENT";
			public static readonly string ACTION_INSERT = "android.intent.action.INSERT";
			public static readonly string ACTION_INSERT_OR_EDIT = "android.intent.action.INSERT_OR_EDIT";
			public static readonly string ACTION_INSTALL_PACKAGE = "android.intent.action.INSTALL_PACKAGE";
			public static readonly string ACTION_MAIN = "android.intent.action.MAIN";
			public static readonly string ACTION_MANAGE_NETWORK_USAGE = "android.intent.action.MANAGE_NETWORK_USAGE";
			public static readonly string ACTION_MEDIA_SEARCH = "android.intent.action.MEDIA_SEARCH";
			public static readonly string ACTION_MUSIC_PLAYER = "android.intent.action.MUSIC_PLAYER";
			public static readonly string ACTION_OPEN_DOCUMENT = "android.intent.action.OPEN_DOCUMENT";
			public static readonly string ACTION_PASTE = "android.intent.action.PASTE";
			public static readonly string ACTION_PICK = "android.intent.action.PICK";
			public static readonly string ACTION_PICK_ACTIVITY = "android.intent.action.PICK_ACTIVITY";
			public static readonly string ACTION_POWER_USAGE_SUMMARY = "android.intent.action.POWER_USAGE_SUMMARY";
			public static readonly string ACTION_RESPOND_VIA_MESSAGE = "android.intent.action.RESPOND_VIA_MESSAGE";
			public static readonly string ACTION_RINGTONE_PICKER = "android.intent.action.RINGTONE_PICKER";
			public static readonly string ACTION_RUN = "android.intent.action.RUN";
			public static readonly string ACTION_SEARCH = "android.intent.action.SEARCH";
			public static readonly string ACTION_SEARCH_LONG_PRESS = "android.intent.action.SEARCH_LONG_PRESS";
			public static readonly string ACTION_SEND = "android.intent.action.SEND";
			public static readonly string ACTION_SENDTO = "android.intent.action.SENDTO";
			public static readonly string ACTION_SEND_MULTIPLE = "android.intent.action.SEND_MULTIPLE";
			public static readonly string ACTION_SET_ALARM = "android.intent.action.SET_ALARM";
			public static readonly string ACTION_SET_TIMER = "android.intent.action.SET_TIMER";
			public static readonly string ACTION_SET_WALLPAPER = "android.intent.action.SET_WALLPAPER";
			public static readonly string ACTION_SHOW_ALARMS = "android.intent.action.SHOW_ALARMS";
			public static readonly string ACTION_SYNC = "android.intent.action.SYNC";
			public static readonly string ACTION_SYSTEM_TUTORIAL = "android.intent.action.SYSTEM_TUTORIAL";
			public static readonly string ACTION_UNINSTALL_PACKAGE = "android.intent.action.UNINSTALL_PACKAGE";
			public static readonly string ACTION_VIEW = "android.intent.action.VIEW";
			public static readonly string ACTION_VIEW_DOWNLOADS = "android.intent.action.VIEW_DOWNLOADS";
			public static readonly string ACTION_VOICE_COMMAND = "android.intent.action.VOICE_COMMAND";
			public static readonly string ACTION_WEB_SEARCH = "android.intent.action.WEB_SEARCH";

			public static class DeviceAdmin
			{
				public static readonly string ACTION_ADD_DEVICE_ADMIN = "android.app.action.ADD_DEVICE_ADMIN";
				public static readonly string ACTION_SET_NEW_PASSWORD = "android.app.action.SET_NEW_PASSWORD";
				public static readonly string ACTION_START_ENCRYPTION = "android.app.action.START_ENCRYPTION";
			}

			public static class Bluetooth
			{
				public static readonly string ACTION_REQUEST_DISCOVERABLE = "android.bluetooth.adapter.action.REQUEST_DISCOVERABLE";
				public static readonly string ACTION_REQUEST_ENABLE = "android.bluetooth.adapter.action.REQUEST_ENABLE";
			}

			public static class Media
			{
				public static readonly string ACTION_DISPLAY_AUDIO_EFFECT_CONTROL_PANEL = "android.media.action.DISPLAY_AUDIO_EFFECT_CONTROL_PANEL";
				public static readonly string ACTION_IMAGE_CAPTURE = "android.media.action.IMAGE_CAPTURE";
				public static readonly string ACTION_IMAGE_CAPTURE_SECURE = "android.media.action.IMAGE_CAPTURE_SECURE";
				public static readonly string ACTION_MEDIA_PLAY_FROM_SEARCH = "android.media.action.MEDIA_PLAY_FROM_SEARCH";
				public static readonly string ACTION_STILL_IMAGE_CAMERA = "android.media.action.STILL_IMAGE_CAMERA";
				public static readonly string ACTION_STILL_IMAGE_CAMERA_SECURE = "android.media.action.STILL_IMAGE_CAMERA_SECURE";
				public static readonly string ACTION_TEXT_OPEN_FROM_SEARCH = "android.media.action.TEXT_OPEN_FROM_SEARCH";
				public static readonly string ACTION_VIDEO_CAMERA = "android.media.action.VIDEO_CAMERA";
				public static readonly string ACTION_VIDEO_CAPTURE = "android.media.action.VIDEO_CAPTURE";
				public static readonly string ACTION_VIDEO_PLAY_FROM_SEARCH = "android.media.action.VIDEO_PLAY_FROM_SEARCH";
			}

			public static class Net
			{
				public static class Wifi
				{
					public static readonly string ACTION_PICK_WIFI_NETWORK = "android.net.wifi.PICK_WIFI_NETWORK";
					public static readonly string ACTION_REQUEST_SCAN_ALWAYS_AVAILABLE = "android.net.wifi.action.REQUEST_SCAN_ALWAYS_AVAILABLE";
				}
			}

			public static class NFC
			{
				public static readonly string ACTION_NDEF_DISCOVERED = "android.nfc.action.NDEF_DISCOVERED";
				public static readonly string ACTION_TAG_DISCOVERED = "android.nfc.action.TAG_DISCOVERED";
				public static readonly string ACTION_TECH_DISCOVERED = "android.nfc.action.TECH_DISCOVERED";
				public static readonly string ACTION_CHANGE_DEFAULT = "android.nfc.cardemulation.action.ACTION_CHANGE_DEFAULT";
			}

			public static class Telephony
			{
				public static readonly string ACTION_CHANGE_DEFAULT = "android.provider.Telephony.ACTION_CHANGE_DEFAULT";
				public static readonly string ACTION_HANDLE_CUSTOM_EVENT = "android.provider.calendar.action.HANDLE_CUSTOM_EVENT";
				public static readonly string ACTION_SEARCH_SETTINGS = "android.search.action.SEARCH_SETTINGS";
			}

			public static class Settings
			{
				public static readonly string ACTION_ACCESSIBILITY_SETTINGS = "android.settings.ACCESSIBILITY_SETTINGS";
				public static readonly string ACTION_ACTION_PRINT_SETTINGS = "android.settings.ACTION_PRINT_SETTINGS";
				public static readonly string ACTION_ADD_ACCOUNT_SETTINGS = "android.settings.ADD_ACCOUNT_SETTINGS";
				public static readonly string ACTION_AIRPLANE_MODE_SETTINGS = "android.settings.AIRPLANE_MODE_SETTINGS";
				public static readonly string ACTION_APN_SETTINGS = "android.settings.APN_SETTINGS";
				public static readonly string ACTION_APPLICATION_DETAILS_SETTINGS = "android.settings.APPLICATION_DETAILS_SETTINGS";
				public static readonly string ACTION_APPLICATION_DEVELOPMENT_SETTINGS = "android.settings.APPLICATION_DEVELOPMENT_SETTINGS";
				public static readonly string ACTION_APPLICATION_SETTINGS = "android.settings.APPLICATION_SETTINGS";
				public static readonly string ACTION_BLUETOOTH_SETTINGS = "android.settings.BLUETOOTH_SETTINGS";
				public static readonly string ACTION_CAPTIONING_SETTINGS = "android.settings.CAPTIONING_SETTINGS";
				public static readonly string ACTION_DATA_ROAMING_SETTINGS = "android.settings.DATA_ROAMING_SETTINGS";
				public static readonly string ACTION_DATE_SETTINGS = "android.settings.DATE_SETTINGS";
				public static readonly string ACTION_DEVICE_INFO_SETTINGS = "android.settings.DEVICE_INFO_SETTINGS";
				public static readonly string ACTION_DISPLAY_SETTINGS = "android.settings.DISPLAY_SETTINGS";
				public static readonly string ACTION_DREAM_SETTINGS = "android.settings.DREAM_SETTINGS";
				public static readonly string ACTION_INPUT_METHOD_SETTINGS = "android.settings.INPUT_METHOD_SETTINGS";
				public static readonly string ACTION_INPUT_METHOD_SUBTYPE_SETTINGS = "android.settings.INPUT_METHOD_SUBTYPE_SETTINGS";
				public static readonly string ACTION_INTERNAL_STORAGE_SETTINGS = "android.settings.INTERNAL_STORAGE_SETTINGS";
				public static readonly string ACTION_LOCALE_SETTINGS = "android.settings.LOCALE_SETTINGS";
				public static readonly string ACTION_LOCATION_SOURCE_SETTINGS = "android.settings.LOCATION_SOURCE_SETTINGS";
				public static readonly string ACTION_MANAGE_ALL_APPLICATIONS_SETTINGS = "android.settings.MANAGE_ALL_APPLICATIONS_SETTINGS";
				public static readonly string ACTION_MANAGE_APPLICATIONS_SETTINGS = "android.settings.MANAGE_APPLICATIONS_SETTINGS";
				public static readonly string ACTION_MEMORY_CARD_SETTINGS = "android.settings.MEMORY_CARD_SETTINGS";
				public static readonly string ACTION_NETWORK_OPERATOR_SETTINGS = "android.settings.NETWORK_OPERATOR_SETTINGS";
				public static readonly string ACTION_NFCSHARING_SETTINGS = "android.settings.NFCSHARING_SETTINGS";
				public static readonly string ACTION_NFC_PAYMENT_SETTINGS = "android.settings.NFC_PAYMENT_SETTINGS";
				public static readonly string ACTION_NFC_SETTINGS = "android.settings.NFC_SETTINGS";
				public static readonly string ACTION_PRIVACY_SETTINGS = "android.settings.PRIVACY_SETTINGS";
				public static readonly string ACTION_QUICK_LAUNCH_SETTINGS = "android.settings.QUICK_LAUNCH_SETTINGS";
				public static readonly string ACTION_SECURITY_SETTINGS = "android.settings.SECURITY_SETTINGS";
				public static readonly string ACTION_SETTINGS = "android.settings.SETTINGS";
				public static readonly string ACTION_SOUND_SETTINGS = "android.settings.SOUND_SETTINGS";
				public static readonly string ACTION_SYNC_SETTINGS = "android.settings.SYNC_SETTINGS";
				public static readonly string ACTION_USER_DICTIONARY_SETTINGS = "android.settings.USER_DICTIONARY_SETTINGS";
				public static readonly string ACTION_WIFI_IP_SETTINGS = "android.settings.WIFI_IP_SETTINGS";
				public static readonly string ACTION_WIFI_SETTINGS = "android.settings.WIFI_SETTINGS";
				public static readonly string ACTION_WIRELESS_SETTINGS = "android.settings.WIRELESS_SETTINGS";
			}

			public static class Speech
			{
				public class Tts
				{
					public static readonly string ACTION_CHECK_TTS_DATA = "android.speech.tts.engine.CHECK_TTS_DATA";
					public static readonly string ACTION_GET_SAMPLE_TEXT = "android.speech.tts.engine.GET_SAMPLE_TEXT";
					public static readonly string ACTION_INSTALL_TTS_DATA = "android.speech.tts.engine.INSTALL_TTS_DATA";
				}
			}
		}
	}
}