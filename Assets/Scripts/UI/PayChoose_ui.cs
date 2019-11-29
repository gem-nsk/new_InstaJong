using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayChoose_ui : ui_basement
{

    public void OpenShop()
    {
        CanvasController.instance.OpenCanvas(0);
    }

    public void OpenAds()
    {
        //ads here...
#if UNITY_ANDROID || UNITY_IOS
        AdsController.instance._video.OnUserEarnedReward += SuccessfulWatchedvideo;
        AdsController.instance.ShowVideo();
#elif UNITY_EDITOR
        SuccessfulWatchedvideo(this, null);
#endif
    }

    private void SuccessfulWatchedvideo(object sender, GoogleMobileAds.Api.Reward e)
    {

        GameControllerScr.instance.ui._refresh(false);
#if UNITY_ANDROID
        AdsController.instance._video.OnUserEarnedReward -= SuccessfulWatchedvideo;
#endif
        CanvasControllerClose();
    }
}
