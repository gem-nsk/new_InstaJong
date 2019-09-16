using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using genField;

using Newtonsoft.Json;
using System.IO;
using System.Net;
using System;

using Assets.Scripts;

public class MainMenuControl : MonoBehaviour
{
    public GameObject BuyInstaCoins_ui;

    public void OpenInstaCoins_button()
    {
        CanvasController.instance.OpenCanvas(BuyInstaCoins_ui);
    }

    public void ContinuePressed()
    {
        loadLevel();
    }
    
      
    public void NewGamePressed()
    {

        //DownloadImagesFromInstagram();

        GameControllerScr.loadGame = false;
        SceneManager.LoadScene("Game");
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

   
    public void Music_button()
    {
        Music.instance.SwitchMusic();
    }

}
