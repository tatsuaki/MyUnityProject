using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopViewController : MonoBehaviour {

	private MyIAPHelper m_IAPHelper;
	private MyAdManager m_AdManager;

	private MySoundManager m_Sound;

	private AndroidPlugin m_AndroidPluguin;


	public void Awake()
	{
		MyLog.D("TopViewController Awake start");
		// Google or Apple
		m_IAPHelper = MyIAPHelper.Instance;
		m_AdManager = MyAdManager.Instance;

		InitUI();
	}

	private void InitUI()
	{
		MyLog.I("InitUI");
		// FB.Init(this.OnInitComplete, this.OnHideUnity);
		if (GetStartButton() != null) 
		{
			GetStartButton().onClick.AddListener(() => {
				// webView
				// MyHttpRequestManager http = gameObject.AddComponent<MyHttpRequestManager>();
				gameObject.AddComponent<MyHttpRequestManager>();
			});
		}

		if (GetBuyButton() != null) 
		{
			GetBuyButton().onClick.AddListener(() => {
				m_IAPHelper.Purchase(null);
				if (m_IAPHelper.m_PurchaseInProgress == true) {
					return;
				}
			});
		}
		if (GetTapjoyButton() != null)
		{
			GetTapjoyButton().onClick.AddListener(() => {
				m_AdManager.TapjoyEvents();
			});
		}
		if (GetFacebookButton() != null) 
		{
			GetFacebookButton().onClick.AddListener(() => {
				m_AdManager.FBAuth();
			});
		}
		if (GetMovieButton() != null) 
		{
			GetMovieButton().onClick.AddListener(() => {
				MyLog.I("click Movie");
				if (null == m_Sound) {
					m_Sound = GameObject.Find("SoundObject").GetComponent<MySoundManager>();
				}
				if (null != m_Sound) {
					m_Sound.playSound1();
					MyLog.I("playSound1");
				} else {
					MyLog.I("playSound1 null");
				}
				//				// 動画再生
				// Handheld.PlayFullScreenMovie ("opmv", Color.black, FullScreenMovieControlMode.CancelOnInput);
			});
		}
		if (GetSoundButton() != null) 
		{
			GetSoundButton().onClick.AddListener(() => {
				MyLog.I("click Sound");
				if (null == m_Sound) {
					m_Sound = GameObject.Find("SoundObject").GetComponent<MySoundManager>();
				}
				if (null != m_Sound) {
					m_Sound.playSound2();
					MyLog.I("playSound2");
				} else {
					MyLog.I("playSound2 null");
				}
				m_AdManager.toFacebookEvent();
			});
		}
		if (GetMailButton() != null) 
		{
			GetMailButton().onClick.AddListener(() => {
				MyLog.I("click MailButton");
//				GameObject.Find("SoundObject").GetComponent<MyMailer>();
//				gameObject.AddComponent<MyMailer>();
				MyMailer mail = new MyMailer();
				mail.OpenMailer();
			});
		}
		if (GetNativeButton() != null) 
		{
			GetNativeButton().onClick.AddListener(() => {
				MyLog.I("click GetNativeButton");
				if (null == m_AndroidPluguin) {
					m_AndroidPluguin = gameObject.AddComponent<AndroidPlugin>();
					MyLog.I("add AndroidPlugin");
				}
				// ShowDialog(string method, string title, string message, 
				// string positiveMS, string neutralMS, string negativeMS, string showMS) {
				m_AndroidPluguin.ShowDialog("ShowMessage", "NativeDialog", "Select", "Toast", "Neutoral", "no", "Android");
				// Native
			});
		}
	}

	public void Start()
	{
		MyLog.I("TopViewController start");
		m_AdManager.OnStart();
	}

	public static void FinishPurchaseEvent(object sender, EventArgs e){
		MyLog.I("FinishPurchaseEvent " + sender);
		MyAdManager.EndPurchase((String)sender);
	}

	// Update is called once per frame
	public void Update()
	{
		m_AdManager.OnUpdate();
	}

	private Button GetBuyButton()
	{
		MyLog.D("GetBuyButton");
		return GameObject.Find("BuyButton").GetComponent<Button>();
	}

	private Button GetTapjoyButton()
	{
		MyLog.D("GetTapjoyButton");
		return GameObject.Find("TapjoyButton").GetComponent<Button>();
	}

	private Button GetFacebookButton()
	{
		MyLog.D("GetFacebookButton");
		return GameObject.Find("FacebookButton").GetComponent<Button>();
	}

	private Button GetStartButton()
	{
		MyLog.D("GetStartButton");
		return GameObject.Find("StartButton").GetComponent<Button>();
	}
	private Button GetMovieButton()
	{
		MyLog.D("GetMovieButton");
		return GameObject.Find("MovieButton").GetComponent<Button>();
	}
	private Button GetSoundButton()
	{
		MyLog.D("GetSoundButton");
		return GameObject.Find("SoundButton").GetComponent<Button>();
	}
	private Button GetMailButton()
	{
		MyLog.D("GetMailButton");
		return GameObject.Find("MailButton").GetComponent<Button>();
	}
	private Button GetNativeButton()
	{
		MyLog.D("GetNativeButton");
		return GameObject.Find("NativeButton").GetComponent<Button>();
	}
	void OnGUI () {
		// Plane plane = GetLogPlane();
		MyLog.DrawLogWindow(new Rect(10, 10, 800, 1000));
		// Make a background box
		GUI.Box(new Rect(1000,10,200,180), "Loader Menu");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(1020,40,160,40), "Level 1")) {
			SceneManager.LoadScene(1);
		}

		// Make the second button.
		if(GUI.Button(new Rect(1020,90,160,40), "Level 2")) {
			SceneManager.LoadScene(2);
		}
	}
}

