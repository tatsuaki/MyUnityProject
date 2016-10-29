using UnityEngine;
using System.Collections;

public class AndroidPlugin : MonoBehaviour {

	void Start () {
		// ShowDialog ();
	}

	void Update () {

	}

	public void ShowDialog(string method, string title, string message, 
		                   string okMS, string nMS, string noMS, string showMS) {
		MyLog.I("ShowDialog method = " + method + " title = " + title + " message = " + message);
		MyLog.I("OKMS = " + okMS + " nMS = " + nMS + " noMS = " + noMS + " showMS " + showMS);
		#if UNITY_ANDROID
		// Javaのオブジェクトを作成
		AndroidJavaClass nativePlugin = new AndroidJavaClass ("com.tatuaki.androidutil.NativePlugin");

		// Context(Activity)オブジェクトを取得する
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject context  = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

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
}
