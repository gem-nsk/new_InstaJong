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

    public void TryFind()
    {
        if (InputField.text.Length < 3)
            return;
        AtlasController._failedAcc += Failed;
        AtlasController._successFinded += success;

        AtlasController.instance.CheckAccount(InputField.text);

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

    void success()
    {
        CanvasControllerClose();
        Debug.Log("Finded");
    }

    public override void DeActivate()
    {
        AtlasController._failedAcc -= Failed;
        AtlasController._successFinded -= success;

        GameControllerScr.loadGame = false;


        SceneManager.LoadScene("Game");



        base.DeActivate();
    }
}
