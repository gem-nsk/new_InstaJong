using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
#if UNITY_ANDROID
using GoogleMobileAds.Android;
#endif

#if UNITY_IOS
using GoogleMobileAds.iOS;
#endif
using GoogleMobileAds.Api;

public class AdsController : MonoBehaviour
{
                                //	ca-app-pub-3940256099942544/1033173712
    public string InterstitialId = "ca-app-pub-3940256099942544/1033173712";
    public string VideoId = "ca-app-pub-3940256099942544/5224354917";

    //debug video = ca-app-pub-3940256099942544/5224354917
    //debug interstitial = ca-app-pub-3940256099942544/1033173712

    public RewardedAd _video;
    public InterstitialAd _interstital;

    private bool DisabledAd;

#region Singleton
    public static AdsController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
#endregion

    public void Init(bool _disabledAd)
    {
        this.DisabledAd = _disabledAd;

        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;

        MobileAds.Initialize(initStatus => { });
        LoadVideo();

        if(!_disabledAd)
        LoadInterstital();

        _video.OnAdClosed += VideoWatched;
        _video.OnAdFailedToLoad += Video_failed;
        _video.OnAdFailedToShow += Video_failed;

        _interstital.OnAdClosed += InterstitialWatched;
        _interstital.OnAdOpening += _interstital_OnAdOpening;
        _interstital.OnAdFailedToLoad += Interstitial_failedtoLoad;
        _interstital.OnAdLoaded += Debug_InterstitialLoaded;
    }

    private void PurchaseManager_OnPurchaseNonConsumable(UnityEngine.Purchasing.PurchaseEventArgs args)
    {
        DisabledAd = true;
    }

  

    private void _interstital_OnAdOpening(object sender, System.EventArgs e)
    {
        LoadInterstital();
    }

    private void Debug_InterstitialLoaded(object sender, System.EventArgs e)
    {
        Debug.Log("Interstitial is successfuly loaded");
    }

    private void Video_failed(object sender, AdErrorEventArgs e)
    {
        Debug.Log("Failed to load video, trying to load new...");
        LoadVideo();
    }

    private void Interstitial_failedtoLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.Log("Interstitital failed to load, trying to load new...");
        LoadInterstital();
    }

    private void InterstitialWatched(object sender, System.EventArgs e)
    {
        Debug.Log("Interstitial watched");
        LoadInterstital();
    }

    private void VideoWatched(object sender, System.EventArgs e)
    {
        Debug.Log("Video watched");
        LoadVideo();
    }

    void LoadInterstital()
    {
        this._interstital = new InterstitialAd(InterstitialId);

        AdRequest request = new AdRequest.Builder().Build();
        _interstital.LoadAd(request);
    }

    void LoadVideo()
    {
        this._video = new RewardedAd(VideoId);

        AdRequest request = new AdRequest.Builder().Build();

        _video.LoadAd(request);
    }


    public bool ShowVideo()
    {
        if(_video.IsLoaded())
        {
            _video.Show();
            return true;
        }
        else
        {
            LoadVideo();
            Debug.Log("Video is not ready");
            return false;
        }
    }
    public void ShowInterstitial()
    {
        if(!DisabledAd)
        _interstital.Show();
    }
}
