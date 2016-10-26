using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Tapjoy
using TapjoyUnity;

// Adjust
using com.adjust.sdk;

// Facebook
using Facebook;
using Facebook.Unity;
using System.Linq;

public class MyAdManager : MonoBehaviour {

	private static MyAdManager mInstance;

	// Tapjoy
	private const string TAPJOY_KEY = "tacbdTYnTN6S-PdFYa2DewECk4qemAOI04tBG1L0u0BEEavbWgknW1hXfdrP";
	public TJPlacement offerwallPlacement;

	// Facebook
	private string facebookStatus = "Ready";
	private string facebookLastResponse = string.Empty;

	// Private Constructor
	private MyAdManager () {		
		Initialize();
	}
	public static MyAdManager Instance {
		get {
			if( mInstance == null )  
			{
				mInstance = new MyAdManager();
				MyLog.I("new MyAdManager");
			}
			return mInstance;
		}
	}
	private void Initialize() {
		if (!Tapjoy.IsConnected) {
			Tapjoy.Connect(TAPJOY_KEY);
			MyLog.D("Start to Tapjoy.Connect");
		}

		// FB init
		FB.Init(this.OnInitComplete, this.OnHideUnity);
		facebookStatus = "FB.Init() called with " + FB.AppId;
		MyLog.D("Facebook = " + facebookStatus);
	}

	#region Tapjoy
	public void TapjoyEvents() 
	{
		if(Tapjoy.IsConnected) {
			// Create offerwall placement
			if (offerwallPlacement == null) {
				offerwallPlacement = TJPlacement.CreatePlacement("unitys");

			}
			if(offerwallPlacement.IsContentReady()) {
				offerwallPlacement.ShowContent();
			} else {
				offerwallPlacement.RequestContent();
				offerwallPlacement.ShowContent();
				MyLog.W("Tapjoy offerwallPlacement.IsContentReady false");
				//Code to handle situation where content is not ready goes here
			}
		} else {
			Tapjoy.Connect(TAPJOY_KEY);
			MyLog.W("Tapjoy.IsConnected false");
		}
	}
	public void OnDisable() {
		MyLog.D("C#: Disabling and removing Tapjoy Delegates");

		// Placement delegates
		TJPlacement.OnRequestSuccess -= HandlePlacementRequestSuccess;
		TJPlacement.OnRequestFailure -= HandlePlacementRequestFailure;
		TJPlacement.OnContentReady -= HandlePlacementContentReady;
		TJPlacement.OnContentShow -= HandlePlacementContentShow;
		TJPlacement.OnContentDismiss -= HandlePlacementContentDismiss;
		TJPlacement.OnPurchaseRequest -= HandleOnPurchaseRequest;
		TJPlacement.OnRewardRequest -= HandleOnRewardRequest;
	}

	#region Placement Delegate Handlers
	public void HandlePlacementRequestSuccess(TJPlacement placement) {
		if (placement.IsContentAvailable()) {
			placement.ShowContent();
		} else {
			MyLog.D("C#: No content available for " + placement.GetName());
		}
	}

	public void HandlePlacementRequestFailure(TJPlacement placement, string error) {
		MyLog.D("C#: HandlePlacementRequestFailure");
		MyLog.D("C#: Request for " + placement.GetName() + " has failed because: " + error);
	}

	public void HandlePlacementContentReady(TJPlacement placement) {
		MyLog.D("C#: HandlePlacementContentReady");
		if (placement.IsContentAvailable()) {
			placement.ShowContent();
		} else {
			MyLog.D("C#: no content");
		}
	}

	public void HandlePlacementContentShow(TJPlacement placement) {
		MyLog.D("C#: HandlePlacementContentShow");
	}

	public void HandlePlacementContentDismiss(TJPlacement placement) {
		MyLog.D("C#: HandlePlacementContentDismiss");
	}

	public void HandleOnPurchaseRequest (TJPlacement placement, TJActionRequest request, string productId)
	{
		MyLog.D("C#: HandleOnPurchaseRequest");
		request.Completed();
	}

	public void HandleOnRewardRequest (TJPlacement placement, TJActionRequest request, string itemId, int quantity)
	{
		MyLog.D("C#: HandleOnRewardRequest");
		request.Completed();
	}
	#endregion
	#endregion

	#region Facebook
	private void OnInitComplete()
	{
		facebookStatus = "Success - Check log for details";
		facebookLastResponse = "Success Response: OnInitComplete Called\n";
		string logMessage = string.Format(
			"OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
			FB.IsLoggedIn,
			FB.IsInitialized);
		MyLog.I(logMessage);
		if (AccessToken.CurrentAccessToken != null)
		{
			MyLog.I("UserId = " + AccessToken.CurrentAccessToken.UserId);
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		facebookStatus = "Success - Check log for details";
		facebookLastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
		MyLog.I("Is game shown: " + isGameShown);
	}

	public void FBAuth() {
		MyLog.I("FBAuth");
		if (AccessToken.CurrentAccessToken != null) {
			CallFBLogout();
		} else {
			CallFBLogin();
			facebookStatus = "Login called";
		}
	}

	private void CallFBLogin()
	{
		MyLog.I("CallFBLogin");
		FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
	}

	private void CallFBLogout()
	{
		MyLog.I("CallFBLogout");
		FB.LogOut();
	}

	protected void HandleResult(IResult result)
	{
		if (result == null)
		{
			facebookLastResponse = "Null Response\n";
			MyLog.I(facebookLastResponse);
			return;
		}

		// this.LastResponseTexture = null;

		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error))
		{
			facebookStatus = "Error - Check log for details";
			facebookLastResponse = "Error Response:\n" + result.Error;
		}
		else if (result.Cancelled)
		{
			facebookStatus = "Cancelled - Check log for details";
			facebookLastResponse = "Cancelled Response:\n" + result.RawResult;
		}
		else if (!string.IsNullOrEmpty(result.RawResult))
		{
			facebookStatus = "Success - Check log for details";
			facebookLastResponse = "Success Response:\n" + result.RawResult;
		}
		else
		{
			facebookLastResponse = "Empty Response\n";
		}
		MyLog.I(result.ToString());
	}
	#endregion

	public static void EndPurchase(string id)
	{
		MyLog.I("SgpAdManager EndPurchase");
		// 広告SDK関連処理
		Tapjoy.TrackPurchase(id, "JPY", (double)(200), null);

		// TODO Adjust
		//
		//		AdjustEvent adjustEvent = new AdjustEvent("abc123");
		//		adjustEvent.addPartnerParameter("key", "value");
		//		adjustEvent.addPartnerParameter("foo", "bar");
		//		Adjust.trackEvent(adjustEvent);
	}

	public void OnStart()
	{
		MyLog.I("SgpAdManager OnStart");
		// Adjust
		AdjustConfig adjustConfig = new AdjustConfig("7b596f75722041707020546f6b656e7d", AdjustEnvironment.Sandbox);
		adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
		Adjust.start(adjustConfig);
	}

	public void OnUpdate()
	{
		if (Tapjoy.IsConnected) {
			if(null != offerwallPlacement && offerwallPlacement.IsContentReady()) {
				MyLog.W("Update Tapjoy ShowContent");
				offerwallPlacement.ShowContent();
			}
		}
	}

	// Use this for initialization
	void Start() {MyLog.I("SgpAdManager Start");}

	// Update is called once per frame
	void Update() {MyLog.I("SgpAdManager Update");}
}
