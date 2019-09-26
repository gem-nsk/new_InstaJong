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

public class MainMenuControl : MonoBehaviour
{
    public GameObject BuyInstaCoins_ui;
    public GameObject Rules_ui;

    public Image musicImg;
    public Sprite[] Musicicons;


    public void OpenInstaCoins_button()
    {

        CanvasController.instance.OpenCanvas(BuyInstaCoins_ui);
    }

    public void ContinuePressed()
    {
        (bool a, string s) _auth = PlayerStats.instance.IsUserAuthorized();

        if (_auth.a)
        {
            loadLevel();
        }
        else
        {
            //open auth window
            GameObject.FindGameObjectWithTag("Login").GetComponent<SampleWebView>().Login();
        }
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
        SceneManager.LoadScene("Menu");
    }

    public void Refresh()
    {
        GameControllerScr.instance.StartCoroutine("Refresh");

        /*GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;
        Field field = gameController.field.refreshField(gameController.field);
        gameController.field = field;
        GameControllerScr.refresh = true;*/
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
}
