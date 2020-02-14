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
    public string InterstitialId = "";
    public string VideoId = "";

    //debug video = ca-app-pub-3940256099942544/5224354917
    //debug interstitial = ca-app-pub-3940256099942544/1033173712

    public RewardedAd _video;
    public InterstitialAd _interstital;

    private bool DisabledAd;
    private bool _authorized;

    #region Singleton
    public static AdsController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

#if UNITY_IOS
            InterstitialId = "ca-app-pub-6218488866205337/5406669266";
            VideoId = "ca-app-pub-6218488866205337/3953986799";
#elif UNITY_ANDROID
            InterstitialId = "ca-app-pub-6218488866205337/5406669266";
            VideoId = "ca-app-pub-6218488866205337/3953986799";
#endif

            //Init(false);
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

        MobileAds.Initialize(initStatus => { _authorized = true; });
        LoadVideo();

        if (!_disabledAd)
            LoadInterstital();




    }

    private void PurchaseManager_OnPurchaseNonConsumable(UnityEngine.Purchasing.PurchaseEventArgs args)
    {
        DisabledAd = true;
    }



    private void _interstital_OnAdOpening(object sender, System.EventArgs e)
    {
    }

    private void Debug_InterstitialLoaded(object sender, System.EventArgs e)
    {
        Debug.Log("Interstitial is successfuly loaded");
    }

    private void Video_failed(object sender, AdErrorEventArgs e)
    {
        Debug.Log("Failed to load video, trying to load new...");
        Music.instance.TurnOn();
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
        Music.instance.TurnOn();
        LoadVideo();
    }

    void LoadInterstital()
    {
        Music.instance.TurnOn();
        if (_interstital != null)
        {
            _interstital.Destroy();
        }
        this._interstital = new InterstitialAd(InterstitialId);

        this._interstital.OnAdClosed += InterstitialWatched;
        this._interstital.OnAdOpening += _interstital_OnAdOpening;
        this._interstital.OnAdFailedToLoad += Interstitial_failedtoLoad;
        this._interstital.OnAdLoaded += Debug_InterstitialLoaded;

        AdRequest request = new AdRequest.Builder().Build();
        this._interstital.LoadAd(request);
    }

    void LoadVideo()
    {
        this._video = new RewardedAd(VideoId);

        _video.OnAdClosed += VideoWatched;
        _video.OnAdFailedToLoad += Video_failed;
        _video.OnAdFailedToShow += Video_failed;

        AdRequest request = new AdRequest.Builder().Build();

        _video.LoadAd(request);
    }


    public bool ShowVideo()
    {
        if (_video.IsLoaded())
        {
            Music.instance.TurnOff();
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
        if (!DisabledAd)
            if (_authorized)
            {
                if (_interstital.IsLoaded())
                {
                    Music.instance.TurnOff();
                    _interstital.Show();

                }
                else
                {
                    Debug.Log("Interstitial is not ready");
                }
            }
    }
}
