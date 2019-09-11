using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text text_InstaCoins;
    public Text text_points;

    public Text _text_AddedPoints;
    public Animator AddedPointsAnimator;

    public void Init()
    {
        GameControllerScr.instance.stats._addInstaCoins += UpdateCoins;
        GameControllerScr.instance.stats._addPoints += UpdatePoints;

        UpdateCoins(GameControllerScr.instance.stats.InstaCoins);
        UpdatePoints(GameControllerScr.instance.stats.Points);
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
            StartCoroutine(GameControllerScr.instance.Refresh(true));
        }
        else
        {
            //Open shop menu
        }
    }
}
