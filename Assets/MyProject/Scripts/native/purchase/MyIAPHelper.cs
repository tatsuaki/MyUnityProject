#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile in this sample.
// #define RECEIPT_VALIDATION
#endif

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// IAP
using UnityEngine.Purchasing;

#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif

public delegate void FinishPurchase(object sender, EventArgs e);

public class MyIAPHelper : MonoBehaviour, IStoreListener {

	public static FinishPurchase finishPurchaseEvent;

	public bool m_PurchaseInProgress;

	// Unity IAP objects 
	protected IStoreController m_Controller;
	protected IAppleExtensions m_AppleExtensions;

	#if RECEIPT_VALIDATION
	private CrossPlatformValidator validator;
	#endif

	protected static MyIAPHelper mInstance;

	// Private Constructor
	public MyIAPHelper () {		
		Initialize();
	}

	public static MyIAPHelper Instance {
		get {
			if( mInstance == null )  
			{
				mInstance = new MyIAPHelper();
				MyLog.I("new MyIAPHelper");
			}
			return mInstance;
		}
	}

	public void Initialize() {
		MyLog.D("Initialize start");
		var module = StandardPurchasingModule.Instance();
		module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

		var builder = ConfigurationBuilder.Instance(module);
		// This enables the Microsoft IAP simulator for local testing.
		// You would remove this before building your release package.
		builder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = true;
		builder.Configure<IGooglePlayConfiguration>().SetPublicKey(MyConfig.API_KEY);

		builder.AddProduct("100.gold.coins", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.01", GooglePlay.Name}, });
		builder.AddProduct("Coin 2", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.02", GooglePlay.Name}, });
		builder.AddProduct("Coin 3", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.03", GooglePlay.Name}, });
		builder.AddProduct("Coin 4", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.04", GooglePlay.Name}, });
		builder.AddProduct("Coin 5", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.05", GooglePlay.Name}, });
		builder.AddProduct("Coin 6", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.06", GooglePlay.Name}, });
		builder.AddProduct("Coin 7", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.07", GooglePlay.Name}, });
		builder.AddProduct("Coin 8", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.08", GooglePlay.Name}, });
		builder.AddProduct("Coin 9", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.09", GooglePlay.Name}, });
		builder.AddProduct("Coin 10", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.10", GooglePlay.Name}, });
		builder.AddProduct("Coin 11", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.11", GooglePlay.Name}, });
		builder.AddProduct("Coin 12", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.12", GooglePlay.Name}, });
		builder.AddProduct("Coin 13", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.13", GooglePlay.Name}, });
		builder.AddProduct("Coin 14", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.14", GooglePlay.Name}, });
		builder.AddProduct("Coin 15", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.15", GooglePlay.Name}, });
		builder.AddProduct("Coin 16", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.16", GooglePlay.Name}, });
		builder.AddProduct("Coin 17", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.17", GooglePlay.Name}, });
		builder.AddProduct("Coin 18", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.18", GooglePlay.Name}, });
		builder.AddProduct("Coin 19", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.19", GooglePlay.Name}, });
		builder.AddProduct("Coin 20", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.20", GooglePlay.Name}, });
		builder.AddProduct("Coin 21", ProductType.Consumable, new IDs {
			{"com.tatuaki.unity.21", GooglePlay.Name}, });

		#if RECEIPT_VALIDATION
		validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.bundleIdentifier);
		#endif

		// Now we're ready to initialize Unity IAP.
		UnityPurchasing.Initialize(this, builder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		m_Controller = controller;
		m_AppleExtensions = extensions.GetExtension<IAppleExtensions> ();

		m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

		foreach (var item in controller.products.all)
		{
			if(item.hasReceipt) { // リストア可能なレシートあるよ! 
				MyLog.I("hasReceipt items:" + item.definition.id);
				m_Controller.ConfirmPendingPurchase(item);
				// item.definition.id - アイテムID
				// item.receipt - レシート関連(JSON)
				// サーバー検証しアイテム復元
			}
			if (item.availableToPurchase)
			{
				MyLog.D(string.Join(" - ",
					new[]
					{
						item.metadata.localizedTitle,
						item.metadata.localizedDescription,
						item.metadata.isoCurrencyCode,
						item.metadata.localizedPrice.ToString(),
						item.metadata.localizedPriceString,
						item.transactionID,
						item.receipt
					}));
			}
		}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		MyLog.D("Purchase OK: " + e.purchasedProduct.definition.id);
		MyLog.D("Receipt: " + e.purchasedProduct.receipt);

		m_PurchaseInProgress = false;

		#if RECEIPT_VALIDATION
		if (Application.platform == RuntimePlatform.Android || 
			Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer) {
			try {
				var result = validator.Validate(e.purchasedProduct.receipt);
				MyLog.D("Receipt is valid. Contents:");
				foreach (IPurchaseReceipt productReceipt in result) {
				MyLog.D(productReceipt.productID);
				MyLog.D(productReceipt.purchaseDate);
				MyLog.D(productReceipt.transactionID);

				GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
				if (null != google) {
					MyLog.D(google.purchaseState);
					MyLog.D(google.purchaseToken);
				}

				AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
				if (null != apple) {
					MyLog.D(apple.originalTransactionIdentifier);
					MyLog.D(apple.subscriptionExpirationDate);
					MyLog.D(apple.cancellationDate);
					MyLog.D(apple.quantity);
				}
				}
			} catch (IAPSecurityException) {
				MyLog.D("Invalid receipt, not unlocking content");
				return PurchaseProcessingResult.Pending;
			}
		}
		#endif

		/// Unity: Receipt: 
		/// {"Store":"GooglePlay",
		///  "TransactionID":"jajodepcioojoaihijpmknfo.AO-J1OwyGtSin48PuN7edkU971ENoMJqLptLkptMNMi0
		///   eTPXs-cLbDVNMJYvfMNcYPtt0PcpvQQ_zzJ96DfDImgMVnl-Pc1lDghKxPQI2MS9mrOBAPUH1Zd_3TvYRf3Bm
		///   j3lnaPiTS09",
		///   "Payload":"{\"json\":\"{\\\"packageName\\\":\\\"com.tatuaki.unity\\\",\\\"productId\\\":\\\"com.tatuaki.unity.04\\\",
		///  \\\"purchaseTime\\\":1477152805761,\\\"purchaseState\\\":0,\\\"purchaseToken\\\":
		///  \\\"jajodepcioojoaihijpmknfo.AO-J1OwyGtSin48PuN7edkU971ENoMJqLptLkptMNMi0eTPXs-cL
		///  // bDVNMJYvfMNcYPtt0PcpvQQ_zzJ96DfDImgMVnl-Pc1lDghKxPQI2MS9mrOBAPUH1Zd_3TvYRf3Bmj3lnaPiTS09\\\"}\",
		///  \"signature\":\"UBvK6MVoYWznB8oADRU4ligkTX7MdQCI7EuyBuSD8HvB\\/cCTF3mJirbzQi9gU0MijkHG1gFVg396kSGa5e
		///  eXA5AHrz+eVkYO9sqgMwdJA71S\\/UNW9YAIirVqSEgNGWLz3uEv\\/tPgcmM7ypJvjdepGbm1FD\\/TQcp6oTHhR1aBBuwxxkRJ
		///  rD7S0ScZJjIv8vQZxWY49rmKHGN4BGxJls5h+RnxKoz3arXzMPf4Z0UN5x1PYv9Q3JUxk7Fhy15CaI39ikjt1CiRAr9293jOLV7fI
		///  EX5JbIavbosAZsCtFTlToJtiawYbj3OcOzpCPt3QXeKrmgE5fwfmyd3Sex7FyNrCQ==\"}"}


		// TODO 価格取得
		finishPurchaseEvent = TopViewController.FinishPurchaseEvent;
		finishPurchaseEvent(e.purchasedProduct.definition.id, null);

		return PurchaseProcessingResult.Pending;
	}

	public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
	{
		MyLog.D("Purchase failed: " + item.definition.id);
		Debug.Log(r);

		m_Controller.ConfirmPendingPurchase(item);
		m_PurchaseInProgress = false;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		MyLog.D("Billing failed to initialize!");
		switch (error)
		{
		case InitializationFailureReason.AppNotKnown:
			MyLog.E("Is your App correctly uploaded on the relevant publisher console?");
			break;
		case InitializationFailureReason.PurchasingUnavailable:
			// Ask the user if billing is disabled in device settings.
			MyLog.D("Billing disabled!");
			break;
		case InitializationFailureReason.NoProductsAvailable:
			// Developer configuration error; check product metadata.
			MyLog.D("No products available for purchase!");
			break;
		}
	}

	public void Purchase(Product product) {
		MyLog.I("Purchase start");
		if (m_PurchaseInProgress == true) {
			return;
		}
		m_Controller.InitiatePurchase(m_Controller.products.all[3]); 
		m_PurchaseInProgress = true;
	}

	public void OnTransactionsRestored(bool success)
	{
		MyLog.D("Transactions restored.");
	}

	public void OnDeferred(Product item)
	{
		MyLog.D("Purchase deferred: " + item.definition.id);
	}
}
