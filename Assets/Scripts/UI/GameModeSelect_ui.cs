using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameModeSelect_ui : ui_basement
{
    public GameObject FindCanvas;

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
        }
    }

    public void FindAcc()
    {
        CanvasController.instance.OpenCanvas(FindCanvas);
    }

    public void PlayAuthorized()
    {


        if (PlayerStats.instance.AccountKey != null)
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

    IEnumerator PreloadSelfImages()
    {
        yield return StartCoroutine(AtlasController.instance.DownloadImagesFromInstagram(PlayerStats.instance.AccountKey));
    }
}
