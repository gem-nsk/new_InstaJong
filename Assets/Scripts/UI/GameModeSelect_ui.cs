using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameModeSelect_ui : ui_basement
{
    public GameObject FindCanvas;
    public GameObject FindHashtag;

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
            string key = "20021759479.9f7d92e.e4cf6803ec204e899ce887aab2b88cbf";
            StartCoroutine(PreloadingManager.instance.PreloadSelfImages(key));
        }
        else
        {
            GameObject.FindGameObjectWithTag("Login").GetComponent<SampleWebView>().Login();
        }
    }
}
