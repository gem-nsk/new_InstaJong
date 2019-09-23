using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Android;
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

    public void Start()
    {
        MobileAds.Initialize(initStatus => { });
        LoadVideo();
        LoadInterstital();

        _video.OnAdClosed += VideoWatched;
        _video.OnAdFailedToLoad += Video_failed;
        _video.OnAdFailedToShow += Video_failed;

        _interstital.OnAdClosed += InterstitialWatched;
        _interstital.OnAdOpening += _interstital_OnAdOpening;
        _interstital.OnAdFailedToLoad += Interstitial_failedtoLoad;
        _interstital.OnAdLoaded += Debug_InterstitialLoaded;
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


    public void ShowVideo()
    {
        _video.Show();
    }
    public void ShowInterstitial()
    {
        _interstital.Show();
    }
}
