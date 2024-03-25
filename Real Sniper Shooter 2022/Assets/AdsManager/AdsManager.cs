using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    InterstitialAd interstitialAd;
    public string _adUnitId;
    public static AdsManager Instance;
    private bool mShowAds = true;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        DontDestroyOnLoad(this.gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        mShowAds = PlayerPrefs.GetInt("NO_ADS") == 0;

        //var adss= MobileAds.GetRequestConfiguration();
        //adss.TestDeviceIds = new List<string> { (SystemInfo.deviceUniqueIdentifier) };
        //MobileAds.SetRequestConfiguration(adss);
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            if(mShowAds)
            LoadInterstitialAd();
            AdsManagerRwd.Instance.LoadRewardedAd();
        });
    }

    public void OnPurchasedRemoveAds()
    {
        PlayerPrefs.SetInt("NO_ADS", 1);
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
    }

    public void LoadInterstitialAd()
    {
            // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        if (!mShowAds)
            return;

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder()
                .Build();


        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }

    [ContextMenu("ShowAD")]
    public void ShowAd()
    {
        if (!mShowAds)
            return;

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");

        }
    }


    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();
        };
    }
}
