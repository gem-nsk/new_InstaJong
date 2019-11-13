using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text text_InstaCoins;
    public Text text_points;

    public Text _text_AddedPoints;
    public Text numLevel;
    public Animator AddedPointsAnimator;

    public Text _Refresh_text;
    public Text _Tip_text;
    public Text _Time_text;

    public ParticleSystem _timeSystem;

    public Animator _TipAnimator;

    public Tutorial rules;

    public void Init()
    {
        //GameControllerScr.instance.stats._addInstaCoins += UpdateCoins;
        GameControllerScr.instance.stats._addPoints += UpdatePoints;
        GameControllerScr.instance.stats._changePackHandler += UpdatePackInfo;
        GameControllerScr.instance.stats.AddPack(0, 0, 0);

        //numLevel.text = GameControllerScr.numMap.ToString();
        //UpdateCoins(GameControllerScr.instance.stats.InstaCoins);
        UpdatePoints(GameControllerScr.instance.stats.Points);
        UpdateLevel(GameControllerScr.numMap);

        StartCoroutine(TipBlink());

        //if (!PlayerPrefs.HasKey("firstStart"))
        //{
        //    StartCoroutine(rules.Init());
        //    PlayerPrefs.SetInt("firstStart", 1);
        //}

    }

    private void OnDisable()
    {
        //GameControllerScr.instance.stats._addInstaCoins -= UpdateCoins;
        GameControllerScr.instance.stats._addPoints -= UpdatePoints;
        GameControllerScr.instance.stats._changePackHandler -= UpdatePackInfo;
    }

    //public void UpdateCoins(int _coins)
    //{
    //    text_InstaCoins.text = _coins + "";
    //}
    public void UpdatePackInfo(int refresh, int tip, int time)
    {
        _Refresh_text.text = refresh + "";
        _Tip_text.text = tip + "";
        _Time_text.text = time + "";
    }

    public void UpdatePoints(int _points)
    {
        StartCoroutine(PointsAnim(_points, GameControllerScr.instance.stats.Points));
    }

    public void UpdateLevel(int level)
    {
        numLevel.text = level.ToString();
    }

    IEnumerator PointsAnim(int _addedPoints, int TotalPoints)
    {
        AddedPointsAnimator.SetTrigger("Action");
        _text_AddedPoints.text = "+" + _addedPoints;
        yield return new WaitForSeconds(0.6f);
        text_points.text = TotalPoints + " points";
    }

    public void RefreshButton()
    {
        if(PlayerStats.instance._Count_Refresh > 0)
        {
            _refresh(true);
            PlayerStats.instance.AddPack(0, 0, -1);
        }
        else
        {
            CanvasController.instance.OpenCanvas(2);
        }

        //if(GameControllerScr.instance.stats.InstaCoins >= GameControllerScr.instance.stats.RefreshPrice)
        //{
        //    _refresh(true);
        //}
    }

    public void _refresh(bool i)
    {
        StartCoroutine(GameControllerScr.instance.Refresh(i));
    }

    public void ShowHelp()
    {
        if (PlayerStats.instance._Count_Tip > 0)
        {
              StartCoroutine( GameControllerScr.instance.HighlightHelpers());
            GameControllerScr.instance.stats.AddPack(-1, 0,0);
        }
        else
        {
            CanvasController.instance.OpenCanvas(0);
        }
    }

    public void Button_AddTime()
    {
        if (PlayerStats.instance._Count_Time > 0)
        {
            GameControllerScr.instance._Timer.AddTime();
            GameControllerScr.instance.stats.AddPack(0, -1, 0);
            _timeSystem.Play();
        }
        else
        {
            CanvasController.instance.OpenCanvas(0);
        }
    }

    IEnumerator TipBlink()
    {
        float _time = 0;
        float _IdleTime = 5; 
        while(_time < _IdleTime)
        {
            if(Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                _time = 0;
            }
            _time += Time.deltaTime;
            yield return null;
        }

        _TipAnimator.SetTrigger("Blink");
        StartCoroutine(TipBlink());
    }
}
