/*ブログ説明用

*/
using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyMailer {
	//メール
	private const string MAIL_ADRESS       = "kin.000@gmail.com";
	private const string NEW_LINE_STRING   = "\n";
	private const string CAUTION_STATEMENT = "---------以下の内容はそのままで---------" + NEW_LINE_STRING;

	/// <summary>
	/// メーラーを起動する
	/// </summary>
	public void OpenMailer(){
		// PlayerSettingsValue
		//タイトルはアプリ名
		string subject = PlayerSettingsValue.PRODUCT_NAME;

		//本文は端末名、OS、アプリバージョン、言語
		string deviceName = SystemInfo.deviceModel;
		#if UNITY_IOS && !UNITY_EDITOR
		deviceName = iPhone.generation.ToString();
		#endif

		string body = NEW_LINE_STRING + NEW_LINE_STRING + CAUTION_STATEMENT + NEW_LINE_STRING;
		body += "Device   : " + deviceName                             + NEW_LINE_STRING;
		body += "OS       : " + SystemInfo.operatingSystem             + NEW_LINE_STRING;
		body += "Ver      : " + PlayerSettingsValue.BUNDLE_VERSION     + NEW_LINE_STRING;
		body += "Language : " + Application.systemLanguage.ToString () + NEW_LINE_STRING;

		//エスケープ処理
		body    = System.Uri.EscapeDataString(body);
		subject = System.Uri.EscapeDataString(subject);

		Application.OpenURL("mailto:" + MAIL_ADRESS + "?subject=" + subject + "&body=" + body);
	}
}