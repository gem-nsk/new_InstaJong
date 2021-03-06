﻿using System.Collections;
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

    public GameObject ScreenCanvas;
    public GameObject AddICBuyShare;
    
    public GameObject BuyInstaCoins_ui;
    public GameObject Tutorialmenu_ui;
    public GameObject Rules_ui;
    public GameObject PrivatePolicy_ui;
    public GameObject PreferAccountSelection_ui;

    [Header("Tutorial")]
    public GameObject DAILY_ACC;
    public GameObject SIGN_IN;
    public GameObject SHARE_ACC;
    public GameObject STORE;
    public GameObject NEW_GAME;
    public GameObject HELLO_SPAWN;
    public Canvas canvas;
    //-----------------------


    public static bool ended;

    public const string _shareKey = "_share";

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

        if (PlayerPrefs.HasKey(_shareKey))
        {
            AddICBuyShare.SetActive(false);
        }
    }

    public void StartTutourial()
    {
        #region tutorial
        if (!PlayerPrefs.HasKey("_tut1"))
        {
            List<RectTransform> RTs = new List<RectTransform>
            {
                (RectTransform)HELLO_SPAWN.transform,
                (RectTransform)HELLO_SPAWN.transform,
                (RectTransform)SIGN_IN.transform,
                (RectTransform)SHARE_ACC.transform,
                (RectTransform)STORE.transform,
                (RectTransform)NEW_GAME.transform,
                (RectTransform)DAILY_ACC.transform
            };

            string[] messages = {
            "_t_tut1_1",
            "_t_tut1_2",
            "_t_tut1_3",
            "_t_tut1_4",
            "_t_tut1_5",
            "_t_tut1_6",
            "_t_tut1_7"
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
        OpenPreferAccountSelection_button();
        //obj.GetComponent<PreferAccountLoading>().StartPlay();
        ended = true;
        PlayerPrefs.SetInt("_tut1", 1);
        //}
    }

    public void OpenInstaCoins_button()
    {

        CanvasController.instance.OpenCanvas(BuyInstaCoins_ui);
    }

    public void OpenPreferAccountSelection_button()
    {
        CanvasController.instance.OpenCanvas(PreferAccountSelection_ui);
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
        switch (b)
        {
            case true:
                musicImg.sprite = Musicicons[0];
                break;
            case false:
                musicImg.sprite = Musicicons[1];
                break;
        }
        AnalyticsEventsController.LogEvent("Music_" + b.ToString());
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

        if (PlayerStats.instance.playerSettings.name != "")
        {
            StartCoroutine(Share(true));
        }
        else
        {
            StartCoroutine(Share(false));
        }
    }

    public void BuyNoAds()
    {
        PurchaseManager.instance.BuyNonConsumable(0);
    }

    IEnumerator Share(bool authorized)
    {
        //share logined account

        ScreenCanvas.SetActive(true);

        string t = "";

        if (authorized)
        {
            t = "Lets play with my account!";
            GameObject.Find("Share_account_name").GetComponent<Text>().text = "@" + PlayerStats.instance.playerSettings.name;
        }
        else
        {
            GameObject.Find("Share_account_name").GetComponent<Text>().text = "";
            t = "Lets play InstaJong!";
        }
      

            GameObject.Find("Share_header").GetComponent<Text>().text = t;

        string path = Application.persistentDataPath + "/share_screen.png";

        yield return new WaitForEndOfFrame();

        //Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
        //DataSave.SaveImage(tex, "share_screen", Application.persistentDataPath);

        var height = 600;
        var width = 600;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect _r = new Rect(0,0, width, height);

        tex.ReadPixels(_r, 0, 0);
        tex.Apply();

        DataSave.SaveImage(tex, "share_screen", Application.persistentDataPath, true);

        Debug.Log(path + " - screenshot saved");

        //yield return new WaitForSeconds(0.05f);

        ScreenCanvas.SetActive(false);


        if (authorized)
        {
            AnalyticsEventsController.LogEvent("Share", "share_type", "authorized");
            new NativeShare().SetTitle("lets play InstaJong!").SetText("Find my account and play! @" + PlayerStats.instance.playerSettings.name + "\n https://play.google.com/apps/testing/com.GeM.InstaJong \n\n\n #InstaJong").AddFile(path).Share();
        }
        else
        {
            AnalyticsEventsController.LogEvent("Share", "share_type", "Not_authorized");
            new NativeShare().SetTitle("lets play InstaJong!").SetText("Hey, lets go play InstaJong! \n https://play.google.com/apps/testing/com.GeM.InstaJong \n\n\n #InstaJong").AddFile(path).Share();
        }
        if (!PlayerPrefs.HasKey(_shareKey))
        {
            PlayerStats.instance.AddInstaCoins(500);
            AddICBuyShare.SetActive(false);

            PlayerPrefs.SetInt(_shareKey, 1);
        }
    }
}
