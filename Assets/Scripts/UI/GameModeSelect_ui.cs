using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameModeSelect_ui : ui_basement
{
    public GameObject FindCanvas;
    public GameObject FindHashtag;
    public GameObject UserSearching;
    public GameObject HistoryCanvas;

    public GameObject Tutorial;

    public override void Activate()
    {
        base.Activate();
        //CanvasController.instance.OpenCanvas(Tutorial);
    }

    public void PlayGame(int arg)
    {
        switch (arg)
        {
            case 0:
                PlayAuthorized();
                break;
                //find account
            case 1:
                FindAcc();
                break;
            case 2:
                FindHash();
                break;
            case 3:
                ShowHistory();
                break;
        }
    }

    public void FindAcc()
    {
        CanvasController.instance.OpenCanvas(FindCanvas);
    }
    void FindHash()
    {
        CanvasController.instance.OpenCanvas(FindHashtag);
    }

    public void PlayAuthorized()
    {
        if(PlayerStats.instance.playerSettings.name != "")
        {
            string key = PlayerStats.instance.playerSettings.token;
            PreloadingManager.instance._PreloadFromSelfImages(key);
            CanvasController.instance.OpenCanvas(UserSearching);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Login").GetComponent<SampleWebView>().Login();
            CanvasControllerClose();
        }
    }

    public void ShowHistory()
    {
        CanvasController.instance.OpenCanvas(HistoryCanvas);
    }
}
