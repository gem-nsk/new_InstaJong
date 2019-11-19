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
    private readonly string hintName = "Hint";
    private readonly string objName = "Obj";
    private int countHints = 5;
    private int idCurrentHint;
    private readonly List<KeyValuePair<string, RectTransform>> hints_messages = new List<KeyValuePair<string, RectTransform>>();

    private GameObject _CurrentHint;
    private Canvas canvas;
    private static bool flg;
    private RectTransform[] list;
    private bool rotate;

    public static TutorialMenu_ui instance;

    public Sprite normalPos;
    public Sprite inversePos;
    public static bool endFlg;
    public static bool MainEnd;
    private int type;

    public override void Activate()
    {
        base.Activate();
        instance = this;
        endFlg = false;
    }

    public void Init(List<RectTransform> RTs, int countHints, string[] messages, bool rotate, int type, params RectTransform[] args)
    {
        this.countHints = countHints;
        this.rotate = rotate;
        this.type = type;
        list = args;
        for(int i = 0; i < countHints; i++)
        {
            var kp = new KeyValuePair<string, RectTransform>(
            messages[i],
            RTs[i]
            );
            hints_messages.Add(kp);
            
        }
        flg = true;
        NextHint();
        
    }

    public void Init(List<RectTransform> RTs, int countHints, string[] messages, bool rotate, Canvas canvas, int type, params RectTransform[] args)
    {
        this.countHints = countHints;
        this.rotate = rotate;
        this.canvas = canvas;
        this.type = type;
        list = args;
        if (canvas)
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        for (int i = 0; i < countHints; i++)
        {
            var kp = new KeyValuePair<string, RectTransform>(
            messages[i],
            RTs[i]
            );
            hints_messages.Add(kp);

        }
        flg = true;
        NextHint();

    }

    public void MakeHint(string message, RectTransform position, bool e)
    {
        if(canvas)
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // подсвечивание
        
        if(!endFlg) {
            var pos = position.position;

            if(list.Length != 0)
            {
                foreach (RectTransform rectTransform in list)
                {
                    if (rectTransform.name == position.name)
                        rotate = true;
                }
            }
            
            var obj = Instantiate(position);
            obj.transform.SetParent(hints, false);
            obj.transform.position = pos;
            obj.name = objName + (idCurrentHint + 1);
            var delBut = obj.GetComponent<Button>();
            Destroy(delBut);

            // new Vector2(position.localPosition.y, position.localPosition.x);
            if (rotate)
            {
                _CurrentHint = Instantiate(hintReverse);
                _CurrentHint.GetComponent<Image>().sprite = inversePos;
            }
            else
            {
                _CurrentHint = Instantiate(hint);
            }

            _CurrentHint.transform.SetParent(hints, false);
            _CurrentHint.name = hintName + (idCurrentHint + 1);
            _CurrentHint.GetComponentInChildren<Text>().text = LocalizationManager.instance.GetLocalizedValue( message);
            _CurrentHint.transform.position = new Vector2(position.transform.position.x, position.transform.position.y);
            //if(type == 0)
            _CurrentHint.GetComponent<Button>().onClick.AddListener(() => NextHint());

            StartCoroutine(ShowHint());

        }

        Debug.Log(position.transform.position);
        if(e == true)
            idCurrentHint += 1;
        //StartCoroutine(ShowHint());

       

    }

   

    private IEnumerator ShowHint()
    {
     if(_CurrentHint)       
            yield return StartCoroutine(SizeTip(_CurrentHint.GetComponent<RectTransform>(),true));

    }

    private IEnumerator SizeTip(RectTransform TipSize, bool hided)
    {
        float _time = 0;
        float _smoothTime = 0.3f;

        TipSize.sizeDelta = new Vector2(TipSize.sizeDelta.x, 0);

        while (_time < _smoothTime)
        {
            if(TipSize)
            TipSize.sizeDelta = new Vector2(TipSize.sizeDelta.x, Mathf.Lerp(TipSize.sizeDelta.y, hided ? 500 : 0, _time / _smoothTime));
            _time += Time.deltaTime;
            yield return null;
        }
    }

    public void NextHint()
    {
        Debug.Log(countHints);
        switch (type)
        {
            case 0:
                {
                    
                    if (idCurrentHint >= countHints-1)
                    {
                        DeleteHint();

                        var msg = hints_messages[idCurrentHint].Key;
                        var pos = hints_messages[idCurrentHint].Value;
                        MakeHint(msg, pos, false);
                        _CurrentHint.GetComponent<Button>().onClick.RemoveListener(() => NextHint());
                        _CurrentHint.GetComponent<Button>().onClick.AddListener(() => OpenDaily());
                        endFlg = true;

                    }
                    else
                    {
                        DeleteHint();

                        var msg = hints_messages[idCurrentHint].Key;
                        var pos = hints_messages[idCurrentHint].Value;
                        MakeHint(msg, pos, true);
                        Debug.Log("idCurrentHint " + idCurrentHint);
                    }
                    break;
                }
            case 1:case 2: case 3:
                {
                    if (idCurrentHint < countHints)
                    {
                        DeleteHint();

                        var msg = hints_messages[idCurrentHint].Key;
                        var pos = hints_messages[idCurrentHint].Value;
                        MakeHint(msg, pos, true);

                    }
                    else
                    {
                        if(type != 2 && type != 3)
                        {
                            if (flg == true)
                            {

                                GameControllerScr.instance.StartTutorial();

                                endFlg = true;
                                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                                canvas.worldCamera = Camera.main;

                            }
                        }
                        


                        base.DeActivate();

                    }
                    break;
                }
        }

        

    }

    public void OpenDaily()
    {
        if(endFlg)
        MainMenuControl.instance.OpenDailyAcc();
        
    }

    public void DeleteHint()
    {
        var delObj = GameObject.Find(objName + idCurrentHint);
        Destroy(delObj);
        var del = GameObject.Find(hintName + idCurrentHint);
        
        Destroy(del);
        
    }
}
