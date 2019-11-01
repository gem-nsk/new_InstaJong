using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu_ui: ui_basement
{

    public Transform hints;
    public GameObject hint;
    private string hintName = "Hint";
    private int countHints = 5;
    private int idCurrentHint = 0;
    private List<KeyValuePair<string, RectTransform>> hints_messages = new List<KeyValuePair<string, RectTransform>>();

    private float width;
    private float height;

    private string[] messages = {
        "Аккаунт дня",
        "Войти в аккаунт",
        "Поделиться аккаунтом",
        "Магазин",
        "Новая игра"
    };

    public override void Activate()
    {
        base.Activate();

        //GetComponent<Canvas>().worldCamera = Camera.main;

        RectTransform rt = (RectTransform)hint.transform;

        width = rt.rect.width;
        height = rt.rect.height;


        Init();

        //Camera.main.WorldToScreenPoint();
        //Camera.main.ScreenToWorldPoint();
        
    }

    private void Init()
    {
        List<RectTransform> RTs = new List<RectTransform>();
        RTs.Add((RectTransform)MainMenuControl.instance.DAILY_ACC.transform);
        RTs.Add((RectTransform)MainMenuControl.instance.SIGN_IN.transform);
        RTs.Add((RectTransform)MainMenuControl.instance.SHARE_ACC.transform);
        RTs.Add((RectTransform)MainMenuControl.instance.STORE.transform);
        RTs.Add((RectTransform)MainMenuControl.instance.NEW_GAME.transform);

        for(int i = 0; i < countHints; i++)
        {
            var kp = new KeyValuePair<string, RectTransform>(
            messages[i],
            RTs[i]
            );
            hints_messages.Add(kp);
        }

        NextHint();
    }

    public void MakeHint(string message, RectTransform position)
    {
        GameObject _hint = Instantiate(hint);
        _hint.transform.SetParent(hints, false);
        _hint.name = hintName + (idCurrentHint + 1);
        _hint.GetComponentInChildren<Text>().text = message;

        _hint.GetComponent<Transform>().position =  new Vector2(position.transform.position.x, position.transform.position.y + height/2); // new Vector2(position.localPosition.y, position.localPosition.x);
        Debug.Log(position.transform.position);
        
        idCurrentHint = idCurrentHint + 1;
    }

    public void NextHint()
    {
        

        if(idCurrentHint < countHints)
        {
            DeleteHint();

            var msg = hints_messages[idCurrentHint].Key;
            var pos = hints_messages[idCurrentHint].Value;
            MakeHint(msg, pos);
        }
        else
        {
            base.DeActivate();
        }

        

    }

    public void DeleteHint()
    {
        var del = GameObject.Find(hintName + idCurrentHint);
        Destroy(del);
    }
}
