using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endGamePreviewer : ui_basement
{
    public Text Description;
    private int state;


    public void Preview(int state)
    {
        GameControllerScr.instance._Timer.TimerState(true);
        this.state = state;
        switch (state)
        {
            case 1:
                {
                    Description.text = "You are won";
                    break;
                }
            case 2:
                {
                    Description.text = "You are loose";
                    break;
                }
            default: break;
        }
    }
    public override void DeActivate()
    {
        base.DeActivate();
    }

    public void Continue()
    {
        GameControllerScr gameController = GameControllerScr.instance;
        switch (state){
            case 1: { gameController.nextLevelFlag = true;  DeActivate(); break; }
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

}
