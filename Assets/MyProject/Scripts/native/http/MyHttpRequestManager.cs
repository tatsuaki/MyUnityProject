using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MyHttpRequestManager : MonoBehaviour {
	// URL
	string url = "http://www.yahoo.co.jp/";
	// サーバへリクエストするデータ
	string user_id = "0";
	string user_name = "matudozer";
	string user_data = "xx,yy,zz";
	// タイムアウト時間
	float timeoutsec = 5f;

	//	public SgpHttpRequestManager(string url) 
	//	{
	//		this.url = url;
	//	}

	void Start() {
		// サーバへPOSTするデータを設定 
		Dictionary<string, string> dic = new Dictionary<string, string>();
		dic.Add ("id", user_id);
		dic.Add ("name", user_name);
		dic.Add ("data", user_data);
		StartCoroutine(HttpPost(url, dic));  // POST

		// サーバへGETするデータを設定
		//        string get_param = "?id=" + user_id + "&name=" + user_name + "&data=" + user_data;
		//        StartCoroutine(HttpGet(url + get_param));  // GET
		// StartCoroutine(HttpGet(url));  // GET
	}

	// HTTP POST リクエスト
	IEnumerator HttpPost(string url, Dictionary<string, string> post)
	{
		MyLog.E("HttpPost");
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<String, String> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);

		// CheckTimeOut()の終了を待つ。5秒を過ぎればタイムアウト
		yield return StartCoroutine(CheckTimeOut(www, timeoutsec));

		if (www.error != null) {
			MyLog.I("HttpPost NG: " + www.error);
		}
		else if (www.isDone) {
			// サーバからのレスポンスを表示
			MyLog.I("HttpPost OK: " + www.text);
		}
	}

	// HTTP GET リクエスト
	IEnumerator HttpGet(string url)
	{
		MyLog.E("HttpGet");
		WWW www = new WWW(url);

		// CheckTimeOut()の終了を待つ。5秒を過ぎればタイムアウト
		yield return StartCoroutine(CheckTimeOut(www, timeoutsec));

		if (www.error != null) {
			MyLog.I("HttpGet NG: " + www.error);
		}
		else if (www.isDone) {
			// サーバからのレスポンスを表示
			MyLog.I("HttpGet OK: " + www.text);
		}
	}

	// HTTPリクエストのタイムアウト処理
	IEnumerator CheckTimeOut(WWW www, float timeout)
	{
		float requestTime = Time.time;

		while(!www.isDone)
		{
			if(Time.time - requestTime < timeout)
				yield return null;
			else
			{
				MyLog.I("TimeOut");  //タイムアウト
				break;
			}
		}
		yield return null;
	}
}