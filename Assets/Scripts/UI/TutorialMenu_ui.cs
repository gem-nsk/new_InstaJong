using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu_ui: ui_basement
{

    public Transform hints;
    public GameObject hint;
    public GameObject hintReverse;
    private string hintName = "Hint";
    private int countHints = 5;
    private int idCurrentHint = 0;
    private List<KeyValuePair<string, RectTransform>> hints_messages = new List<KeyValuePair<string, RectTransform>>();

    private float width;
    private float height;

    private bool rotate;

    public static TutorialMenu_ui instance;

    public Sprite normalPos;
    public Sprite inversePos;

    private string[] messages = {
        "Вы можете играть фотографиями популярного аккаунта",
        "Вы можете войти в свой аккаунт, чтобы играть фотографиями вашего профиля",
        "Вы можете поделиться своим аккаунтом, чтобы ваши друзья могли в него поиграть",
        "Здесь вы можете купить монетки, подсказки, бонусы или отключить рекламу",
        "Здесь вы можете начать игру"
    };

    public override void Activate()
    {
        base.Activate();
        instance = this;

        //GetComponent<Canvas>().worldCamera = Camera.main;

        RectTransform rt = (RectTransform)hint.transform;

        width = rt.rect.width;
        height = rt.rect.height;
       

        //Camera.main.WorldToScreenPoint();
        //Camera.main.ScreenToWorldPoint();
        
    }

    public void Init(List<RectTransform> RTs, int countHints, string[] messages, bool rotate)
    {
        this.messages = messages;
        this.countHints = countHints;
        this.rotate = rotate;
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

        // new Vector2(position.localPosition.y, position.localPosition.x);
        if (rotate)
        {
            GameObject _hint = Instantiate(hintReverse);
            _hint.transform.SetParent(hints, false);
            _hint.name = hintName + (idCurrentHint + 1);
            _hint.GetComponentInChildren<Text>().text = message;

            _hint.GetComponent<Transform>().position = new Vector2(position.transform.position.x, position.transform.position.y);
            _hint.GetComponent<Image>().sprite = inversePos;
        }
        else
        {
            GameObject _hint = Instantiate(hint);
            _hint.transform.SetParent(hints, false);
            _hint.name = hintName + (idCurrentHint + 1);
            _hint.GetComponentInChildren<Text>().text = message;
            _hint.GetComponent<Transform>().position = new Vector2(position.transform.position.x, position.transform.position.y);
        }

        

        Debug.Log(position.transform.position);
        
        idCurrentHint = idCurrentHint + 1;
        //StartCoroutine(ShowHint());

    }

    private IEnumerator ShowHint()
    {
        while (true)
        {
            yield return StartCoroutine(SizeTip(hint.GetComponent<RectTransform>(),true));
            yield return new WaitForSeconds(5);
            yield return StartCoroutine(SizeTip(hint.GetComponent<RectTransform>(),false));

        }
    }

    private IEnumerator SizeTip(RectTransform TipSize, bool hided)
    {
        float _time = 0;
        float _smoothTime = 1;


        while (_time < _smoothTime)
        {
            TipSize.sizeDelta = new Vector2(TipSize.sizeDelta.x, Mathf.Lerp(TipSize.sizeDelta.y, hided ? 440 : 150, _time / _smoothTime));
            _time += Time.deltaTime;
            yield return null;
        }
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
