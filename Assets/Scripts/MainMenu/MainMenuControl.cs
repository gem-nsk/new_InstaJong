using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using genField;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System;

using Assets.Scripts;

public class MainMenuControl : MonoBehaviour
{
    //Objects
    public GameObject DAILY_ACC;
    public GameObject SIGN_IN;
    public GameObject SHARE_ACC;
    public GameObject STORE;
    public GameObject NEW_GAME;
    //-----------------------

    public GameObject BuyInstaCoins_ui;
    public GameObject Tutorialmenu_ui;
    public GameObject Rules_ui;

    public Image musicImg;
    public Sprite[] Musicicons;
    public static MainMenuControl instance;
    private void Awake()
    {
        List<RectTransform> RTs = new List<RectTransform>();
        RTs.Add((RectTransform)DAILY_ACC.transform);
        RTs.Add((RectTransform)SIGN_IN.transform);
        RTs.Add((RectTransform)SHARE_ACC.transform);
        RTs.Add((RectTransform)STORE.transform);
        RTs.Add((RectTransform)NEW_GAME.transform);

        string[] messages = {
            "Вы можете играть фотографиями популярного аккаунта",
            "Вы можете войти в свой аккаунт, чтобы играть фотографиями вашего профиля",
            "Вы можете поделиться своим аккаунтом, чтобы ваши друзья могли в него поиграть",
            "Здесь вы можете купить монетки, подсказки, бонусы или отключить рекламу",
            "Здесь вы можете начать игру"
        };

        instance = this;
        Application.targetFrameRate = 60;
        CanvasController.instance.OpenCanvas(Tutorialmenu_ui);
        TutorialMenu_ui.instance.Init(RTs, 5, messages, false);
    }

    public void OpenInstaCoins_button()
    {

        CanvasController.instance.OpenCanvas(BuyInstaCoins_ui);
    }

    public void ContinuePressed()
    {
        PreloadingManager.instance._LoadFromCache();
    }
    
    public void NewGamePressed()
    {
        //check instagramm authorization
        CanvasController.instance.OpenCanvas(3);
        //DownloadImagesFromInstagram();

    }

    public void loadLevel()
    {

        //GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;
        //gameController.loadGame = true;
        //gameController.loadMap();
        GameControllerScr.loadGame = true;
        SceneManager.LoadScene("Game");
    }
    
    public void ButtonBack()
    {
        AdsController.instance.ShowInterstitial();
        GameControllerScr.instance.StopLoading();
        GameControllerScr.instance.Save();
        SceneManager.LoadScene("Menu");
    }

    public void Refresh()
    {
        GameControllerScr.instance.StartCoroutine("Refresh");
    }

    private void Start()
    {
       
         bool b = Music.instance.isPlaying;
        switch (b)
        {
            case true:
                musicImg.sprite = Musicicons[0];
                break;
            case false:
                musicImg.sprite = Musicicons[1];
                break;
        }
    }

    public void Music_button()
    {
       bool b = Music.instance.SwitchMusic();
        switch(b)
        {
            case true:
                musicImg.sprite = Musicicons[0];
                break;
            case false:
                musicImg.sprite = Musicicons[1];
                break;
        }
    }
    public void OpenRules()
    {
        CanvasController.instance.OpenCanvas(Rules_ui);
    }
    public void Leaders()
    {
        AdsController.instance.ShowInterstitial();
    }

    public void ShareAccount()
    {
         new NativeShare().SetTitle("lets play InstaJong!").SetText("You can try my Account! @" + PlayerStats.instance.playerSettings.name + "\n https://play.google.com/apps/testing/com.GeM.InstaJong").Share();
    }

    public void BuyNoAds()
    {
        PurchaseManager.instance.BuyNonConsumable(0);
    }
}
