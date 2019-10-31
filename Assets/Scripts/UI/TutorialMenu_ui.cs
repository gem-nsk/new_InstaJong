using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu_ui: ui_basement
{
    public Transform hints;
    public GameObject hint;
    private string hintName = "Hint";
    private int countHints = 0;
    private int idCurrentHint = 0;
    private List<KeyValuePair<string, Vector3>> hints_messages = new List<KeyValuePair<string, Vector3>>();
    

    public override void Activate()
    {
        base.Activate();
        var KP= new KeyValuePair<string, Vector3>("Аккаунт дня", new Vector3(600, 600, 0));
        hints_messages.Add(KP);

        KP = new KeyValuePair<string, Vector3>("Вход в аккаунт", new Vector3(2000, 800, 0));
        hints_messages.Add(KP);

        NextHint();
    }

    public void MakeHint(string message, Vector3 position)
    {
        GameObject _hint = Instantiate(hint);
        _hint.transform.SetParent(hints, false);
        _hint.name = hintName + (idCurrentHint + 1);
        _hint.GetComponentInChildren<Text>().text = message;
        _hint.GetComponent<Transform>().position = position;
        countHints++;
        idCurrentHint = idCurrentHint + 1;
    }

    public void NextHint()
    {
        DeleteHint();

        var msg = hints_messages[idCurrentHint].Key;
        var pos = hints_messages[idCurrentHint].Value;

        MakeHint(msg, pos);
    }

    public void DeleteHint()
    {
        var del = GameObject.Find(hintName + idCurrentHint);
        Destroy(del);
    }
}
