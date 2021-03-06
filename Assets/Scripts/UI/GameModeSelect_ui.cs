﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameModeSelect_ui : ui_basement
{
    public GameObject FindCanvas;
    public GameObject FindHashtag;
    public GameObject UserSearching;
    public GameObject HistoryCanvas;

    public GameObject MyAccount_t;
    public GameObject Hashtag_t;
    public GameObject FindAccoount_t;
    public GameObject History_t;

    public GameObject Tutorial;

    public Canvas canvas;

    public override void Activate()
    {
        base.Activate();

        if(!PlayerPrefs.HasKey("_tut3"))
        {
            List<RectTransform> RTs = new List<RectTransform>();
            RTs.Add((RectTransform)MyAccount_t.transform);
            RTs.Add((RectTransform)Hashtag_t.transform);
            RTs.Add((RectTransform)FindAccoount_t.transform);
            RTs.Add((RectTransform)History_t.transform);

            string[] messages = {
        "_t_tut3_1",
        "_t_tut3_2",
        "_t_tut3_3",
        "_t_tut3_4",
        };

            CanvasController.instance.OpenCanvas(Tutorial, false);
            TutorialMenu_ui.instance.Init(RTs, 4, messages, false, 2);

            PlayerPrefs.SetInt("_tut3", 1);
        }
    }

    public void PlayGame(int arg)
    {
        switch (arg)
        {
            case 0:
                PlayAuthorized();
                AnalyticsEventsController.LogEvent("GameMode_Self");
                break;
                //find account
            case 1:
                FindAcc();
                AnalyticsEventsController.LogEvent("GameMode_Account");
                break;
            case 2:
                FindHash();
                AnalyticsEventsController.LogEvent("GameMode_Hashtag");
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
