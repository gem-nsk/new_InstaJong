﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endGamePreviewer : ui_basement
{
    public TextLocalization AddedInstaCoins_text;
    public TextLocalization[] Score_text;
    public TextLocalization[] Highscore_text;
    public GameObject _doubleCoinsButton;
    public GameObject _doubleCoinsObject;
    private int state;

    public GameObject[] Endings;


    public void Preview(int state)
    {
        GameControllerScr.instance._Timer.TimerState(true);
        this.state = state;


        Debug.Log(GameControllerScr.instance.stats.GetScore().ToString());

        switch (state)
        {
            case 1:
                {
                    Endings[0].SetActive(true);

                    Invoke("UpdateScore", 0.05f);

                    Endings[1].SetActive(false);
                    break;
                }
            case 2:
                {
                    Endings[1].SetActive(true);

                    Invoke("UpdateScore", 0.05f);

                    Endings[0].SetActive(false);
                    break;
                }
            default: break;
        }
    }

    public void UpdateScore()
    {

        Score_text[0].AddToText(GameControllerScr.instance.stats.GetScore().ToString());
        Highscore_text[0].AddToText(GameControllerScr.instance.stats.GetHighscore().ToString());

        Score_text[1].AddToText(GameControllerScr.instance.stats.GetScore().ToString());
        Highscore_text[1].AddToText(GameControllerScr.instance.stats.GetHighscore().ToString());

        AddedInstaCoins_text.AddToText(GameControllerScr.instance.stats.AddLevelInstaCoins(GameControllerScr.numMap).ToString());
    }

    public override void DeActivate()
    {
        base.DeActivate();
        GameControllerScr.instance._Timer.SetPaused("end game", false);
    }

    public void Continue()
    {
        GameControllerScr gameController = GameControllerScr.instance;
        switch (state){
            case 1: {  DeActivate(); break; }
            case 2:
                {
                    gameController.nextLevelFlag = false;
                    GameControllerScr.numMap = 0;
                    SceneManager.LoadScene("Menu");
                    break;
                }
        }
        gameController._Timer.TimerState(false);
    }

    public void RestartGame()
    {
        GameControllerScr.instance.stats.SetPointsTo(0);
        GameControllerScr.instance._Timer.AddTime();
        GameControllerScr.instance._Timer.SetPaused("end game", false) ;
        StartCoroutine(GameControllerScr.instance.CreateButtonCells());
        CanvasControllerClose();
    }

    public void DoubleCoins()
    {
        if( AdsController.instance.ShowVideo())
        {
            AdsController.instance._video.OnUserEarnedReward += RewardHandler;
        }
        else
        {
            CanvasControllerClose();
        }

    }

    private void RewardHandler(object sender, GoogleMobileAds.Api.Reward e)
    {
        PlayerStats.instance.AddLevelInstaCoins(GameControllerScr.numMap);
        AdsController.instance._video.OnUserEarnedReward -= RewardHandler;
        _doubleCoinsObject.SetActive(true);    
        _doubleCoinsButton.SetActive(false);
    }
}
