using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endGamePreviewer : ui_basement
{
    public TextLocalization[] Score_text;
    public TextLocalization[] Highscore_text;

    private int state;

    public GameObject[] Endings;


    public void Preview(int state)
    {
        GameControllerScr.instance._Timer.TimerState(true);
        this.state = state;

        AdsController.instance.ShowInterstitial();

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
    }

    public override void DeActivate()
    {
        base.DeActivate();
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
        GameControllerScr.instance._Timer._isPaused = false;
        StartCoroutine(GameControllerScr.instance.CreateButtonCells());
        CanvasControllerClose();
    }
}
