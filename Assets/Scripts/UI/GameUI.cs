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

    public Tutorial rules;

    public void Init()
    {
        GameControllerScr.instance.stats._addInstaCoins += UpdateCoins;
        GameControllerScr.instance.stats._addPoints += UpdatePoints;

        UpdateCoins(GameControllerScr.instance.stats.InstaCoins);
        UpdatePoints(GameControllerScr.instance.stats.Points);
        UpdateLevel(GameControllerScr.gameStrategy);

        if (!PlayerPrefs.HasKey("firstStart"))
        {
            StartCoroutine(rules.Init());
            PlayerPrefs.SetInt("firstStart", 1);
        }

    }
    private void OnDisable()
    {
        GameControllerScr.instance.stats._addInstaCoins -= UpdateCoins;
        GameControllerScr.instance.stats._addPoints -= UpdatePoints;

    }

    public void UpdateCoins(int _coins)
    {
        text_InstaCoins.text = _coins + "";
    }
    public void UpdatePoints(int _points)
    {
        StartCoroutine(PointsAnim(_points, GameControllerScr.instance.stats.Points));
    }
    public void UpdateLevel(GameStrategy strategy)
    {
        int level = int.Parse(numLevel.text);
        numLevel.text = (level + 1).ToString();
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
        if(GameControllerScr.instance.stats.InstaCoins >= GameControllerScr.instance.stats.RefreshPrice)
        {
            _refresh(true);
        }
        else
        {
            CanvasController.instance.OpenCanvas(2);
        }
    }

    public void _refresh(bool i)
    {
        StartCoroutine(GameControllerScr.instance.Refresh(i));
    }

    public void ShowHelp()
    {
        if (GameControllerScr.instance.stats.InstaCoins >= GameControllerScr.instance.stats.HelpPrice)
        {
              StartCoroutine( GameControllerScr.instance.HighlightHelpers());
            GameControllerScr.instance.stats.AddInstaCoins(-GameControllerScr.instance.stats.HelpPrice);
        }
        else
        {
            CanvasController.instance.OpenCanvas(0);
        }
    }
    public void Button_AddTime()
    {
        if (GameControllerScr.instance.stats.InstaCoins >= GameControllerScr.instance.stats.AddTimePrice)
        {
            GameControllerScr.instance._Timer.AddTime();
            GameControllerScr.instance.stats.AddInstaCoins(-GameControllerScr.instance.stats.AddTimePrice);
        }
        else
        {
            CanvasController.instance.OpenCanvas(0);
        }
    }
}
