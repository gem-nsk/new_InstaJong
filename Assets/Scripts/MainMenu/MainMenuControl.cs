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
using Assets.Accounts.Convert.preferAccount;

public class MainMenuControl : MonoBehaviour
{
    //Objects
    public Button ContinueButton;
    [Header("Tutorial")]
    public GameObject DAILY_ACC;
    public GameObject SIGN_IN;
    public GameObject SHARE_ACC;
    public GameObject STORE;
    public GameObject NEW_GAME;
    public GameObject HELLO_SPAWN;
    //-----------------------

    public GameObject BuyInstaCoins_ui;
    public GameObject Tutorialmenu_ui;
    public GameObject Rules_ui;
    public GameObject PrivatePolicy_ui;

    public static bool ended = false;
    
    public Image musicImg;
    public Sprite[] Musicicons;
    public static MainMenuControl instance;
    private void Awake()
    {
        instance = this;
        if (!PlayerPrefs.HasKey("_tut1"))
            CanvasController.instance.OpenCanvas(PrivatePolicy_ui);
        Application.targetFrameRate = 60;
        bool b = DataSave.IsSaveExists();
        //ContinueButton.interactable = b;
        ContinueButton.GetComponentInChildren<Text>().color = b ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.7f);
    }

    public void StartTutourial()
    {
        #region tutorial
        if (!PlayerPrefs.HasKey("_tut1"))
        {
            List<RectTransform> RTs = new List<RectTransform>();
            RTs.Add((RectTransform)HELLO_SPAWN.transform);
            RTs.Add((RectTransform)HELLO_SPAWN.transform);
            RTs.Add((RectTransform)SIGN_IN.transform);
            RTs.Add((RectTransform)SHARE_ACC.transform);
            RTs.Add((RectTransform)STORE.transform);
            RTs.Add((RectTransform)NEW_GAME.transform);
            RTs.Add((RectTransform)DAILY_ACC.transform);

            string[] messages = {
            "Добро пожаловать в игру InstaJong!",
            "Сейчас для вас проведем небольшой инструктаж как играть в эту игру",
            "Вы можете войти в свой аккаунт, чтобы играть фотографиями вашего профиля",
            "Потом вы сможете поделиться своим аккаунтом, чтобы ваши друзья могли в него поиграть",
            "Здесь вы можете купить монетки, подсказки, бонусы или отключить рекламу",
            "Здесь вы можете начать игру",
            "А сейчас мы сыграем в аккаунт дня. Нажмите на него прямо сейчас!"
            };


            //"А сейчас мы сыграем в аккаунт дня. Нажмите на него прямо сейчас!"

            CanvasController.instance.OpenCanvas(Tutorialmenu_ui);
            TutorialMenu_ui.instance.Init(RTs, 7, messages, false, 0);

            
            
        }
        #endregion
    }

    public void OpenDailyAcc()
    {
        //if (ended == false)
        //{
            var obj = GameObject.Find("DailyAcc");
            obj.GetComponent<PreferAccountLoading>().StartPlay();
            ended = true;
            PlayerPrefs.SetInt("_tut1", 1);
        //}
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
