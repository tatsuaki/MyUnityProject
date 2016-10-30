using UnityEngine;
using System.Collections;

public class AndroidPlugin : MonoBehaviour {
	private const string TAG = "AndroidPlugin";
	private const string NATIVE_CLASS   = "com.tatuaki.unity.utils.NativePlugin";
	private const string NATIVE_CONTEXT = "com.unity3d.player.UnityPlayer";

	// Javaオブジェクト取得(static)
	public AndroidJavaClass getStaticNativeClass() {
		return new AndroidJavaClass (NATIVE_CLASS);
	}

	// Javaインスタンス取得
	// non staticな場合は、インスタンス作成が必要なので AndroidJavaObjectを使う
	// AndroidJavaClass はインスタンスを作らないのでStaticフィールドにアクセスする事しかできない
	public AndroidJavaObject getNativeObject() {
		return new AndroidJavaObject (NATIVE_CLASS);
	}

	// UnityPlayerを取得
	public AndroidJavaClass getUnityPlayer() {
		return new AndroidJavaClass(NATIVE_CONTEXT); 
	}

	// UnityPlayerActivityを取得
	public AndroidJavaObject getContext() {
		return getUnityPlayer().GetStatic<AndroidJavaObject>("currentActivity");
	}

	// ApplicationContextを取得
	public AndroidJavaObject getApplicationContext() {
		return getContext().Call<AndroidJavaObject>("getApplicationContext");
	}

	public void inits() {
		if (Application.platform == RuntimePlatform.Android) {
			MyLog.I("inits");
			AndroidJavaClass nativePlugin = getStaticNativeClass();
			AndroidJavaObject context  = getContext();

			// static public void ShowMessage
			// (final Context context, String title, String message, String positiveMessage, 
			// String NeutralMessage, String negativeMessage, final String showMessage)
			// AndroidのUIスレッドで動かす
			context.Call ("runOnUiThread", new AndroidJavaRunnable(() => {
				// ダイアログ表示のstaticメソッドを呼び出す
				nativePlugin.CallStatic (
					"init",
					context
				);
			}));
		}
	}

	public void ShowDialog(string method, string title, string message, 
		                   string okMS, string nMS, string noMS, string showMS) {
		MyLog.I("ShowDialog method = " + method + " title = " + title + " message = " + message);
		MyLog.I("OKMS = " + okMS + " nMS = " + nMS + " noMS = " + noMS + " showMS " + showMS);
		#if UNITY_ANDROID

			AndroidJavaClass nativePlugin = getStaticNativeClass();
			AndroidJavaObject context  = getContext();

	 		// static public void ShowMessage
			// (final Context context, String title, String message, String positiveMessage, 
			// String NeutralMessage, String negativeMessage, final String showMessage)
			// AndroidのUIスレッドで動かす
			context.Call ("runOnUiThread", new AndroidJavaRunnable(() => {
				// ダイアログ表示のstaticメソッドを呼び出す
				nativePlugin.CallStatic (
					method,
					context,
					title,
					message,
					okMS,
					nMS,
					noMS,
					showMS
				);
			}));
		#endif
	}

	public int ChangeNative(string method, int before) {
		MyLog.I("ChangeNative method = " + method + " before = " + before);
		int after = 0;
		#if UNITY_ANDROID
			AndroidJavaObject nativePlugin = getNativeObject();
			after = nativePlugin.Call<int>(method, before);
			MyLog.I("ChangeNative after = " + after);
		#endif
		return after;
	}

	public string GetToken(string method) {
		MyLog.I(TAG + " GetToken method = " + method);
		string tokens = null;
		#if UNITY_ANDROID
		AndroidJavaClass nativePlugin = getStaticNativeClass();
		AndroidJavaObject context  = getContext();

		context.Call ("runOnUiThread", new AndroidJavaRunnable(() => {
			tokens = nativePlugin.CallStatic<string> (method, context);
			MyLog.W(TAG + " GetToken tokens = " + tokens);
		}));
		#endif
		return tokens;
	}

	public string getVersionName() 
	{
		AndroidJavaObject context = getApplicationContext();
		AndroidJavaObject pManager = context.Call<AndroidJavaObject>("getPackageManager");
		AndroidJavaObject pInfo = pManager.Call<AndroidJavaObject>( "getPackageInfo", context.Call<string>("getPackageName"), pManager.GetStatic<int>("GET_ACTIVITIES") );
		string versionName = pInfo.Get<string>( "versionName" );
		return versionName;
	}
	
	public void Send() {
		// Find the UnityPlayer and get the static current activity
		AndroidJavaClass cUnityPlayer = getStaticNativeClass();
		AndroidJavaObject oCurrentActivity = cUnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");

		// Get defenitions of Intent and it's constructor.
		AndroidJavaObject oIntent = new AndroidJavaObject ("android.content.Intent");
		// Call some methods
		oIntent.Call<AndroidJavaObject> ("setAction", "android.intent.action.SEND");
		oIntent.Call<AndroidJavaObject> ("setType", "text/plain");
		oIntent.Call<AndroidJavaObject> ("putExtra", "android.intent.extra.TITLE", "Hello");
		// Start the activity!
		oCurrentActivity.Call ("startActivity", oIntent);
		//Dispose them. Not sure if I need to do it or not...
		oIntent.Dispose ();
		oCurrentActivity.Dispose ();
	}
}
