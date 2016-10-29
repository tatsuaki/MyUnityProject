using UnityEngine;
using System.Collections;

public class AndroidPlugin : MonoBehaviour {

	void Start () {
		// ShowDialog ();
	}

	void Update () {

	}

	public void ShowDialog(string method, string title, string message, 
		                   string positiveMS, string neutralMS, string negativeMS, string showMS) {
		MyLog.I("ShowDialog method = " + method + " title = " + title + " message = " + message);
		MyLog.I("positiveMS = " + positiveMS + " neutralMS = " + neutralMS + " negativeMS = " + negativeMS + " showMS " + showMS);
		#if UNITY_ANDROID
		// Javaのオブジェクトを作成
		AndroidJavaClass nativePlugin = new AndroidJavaClass ("com.tatuaki.androidplugin.NativePlugin");

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
				positiveMS,
				neutralMS,
				negativeMS,
				showMS
			);
		}));
		#endif
	}
}
