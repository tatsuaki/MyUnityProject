/*ブログ説明用

*/
using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyMailer {

	/// <summary>
	/// メーラーを起動する
	/// </summary>
	public void OpenMailer(){
		//タイトルはアプリ名
		string subject = PlayerSettingsValue.PRODUCT_NAME;

		//本文は端末名、OS、アプリバージョン、言語
		string deviceName = SystemInfo.deviceModel;
		#if UNITY_IOS && !UNITY_EDITOR
		deviceName = iPhone.generation.ToString();
		#endif

		string line = MyConfig.NEW_LINE_STRING;
		string body = line + line + MyConfig.CAUTION_STATEMENT + line;
		body += "Device   : " + deviceName                             + line;
		body += "OS       : " + SystemInfo.operatingSystem             + line;
		body += "Ver      : " + PlayerSettingsValue.BUNDLE_VERSION     + line;
		body += "Language : " + Application.systemLanguage.ToString () + line;

		//エスケープ処理
		body    = System.Uri.EscapeDataString(body);
		subject = System.Uri.EscapeDataString(subject);

		Application.OpenURL("mailto:" + MyConfig.MAIL_ADRESS + "?subject=" + subject + "&body=" + body);
	}
}