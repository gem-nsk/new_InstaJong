using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FindAccount_ui : ui_basement
{
    public InputField InputField;
    public GameObject LoadingObject;
    public GameObject InputObject;
    public Text Error_tex;

    public void TryFind(bool acc)
    {
        if (InputField.text.Length < 3)
            return;

        DownloadManager.ErrorHandler += Failed;
        DownloadManager.SuccessfullHandler += success;

        if(acc)
        StartCoroutine(PreloadingManager.instance.PreloadAccountImages(InputField.text));
        else
            StartCoroutine(PreloadingManager.instance.PreloadHashtagImages(InputField.text));

        InputObject.SetActive(false);
        LoadingObject.SetActive(true);

        Error_tex.text = "";
    }

    void Failed(string args)
    {
        LoadingObject.SetActive(false);
        InputObject.SetActive(true);

        Error_tex.text = args;
    }

    void success(string msg)
    {
        Debug.Log("Finded");
    }

    public override void DeActivate()
    {
        DownloadManager.ErrorHandler -= Failed;
        DownloadManager.SuccessfullHandler -= success;

        DownloadManager.instance.StopLoading();

        base.DeActivate();
    }
}
