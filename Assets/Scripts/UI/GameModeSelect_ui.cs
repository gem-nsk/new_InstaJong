using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameModeSelect_ui : ui_basement
{

    public void PlayGame(int arg)
    {
        switch (arg)
        {
            case 0:
                PlayAuthorized();
                break;
        }
    }

    public void PlayAuthorized()
    {
        (bool a, string s) _auth = PlayerStats.instance.IsUserAuthorized();

        if (_auth.a)
        {
            GameControllerScr.loadGame = false;
            SceneManager.LoadScene("Game");
        }
        else
        {
            //open auth window
            CanvasControllerClose();
            GameObject.FindGameObjectWithTag("Login").GetComponent<SampleWebView>().Login();
        }
    }
}
