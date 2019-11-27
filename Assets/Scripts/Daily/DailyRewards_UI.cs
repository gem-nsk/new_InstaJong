using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public struct SpinElement
{
    public Sprite _sprite;
    public _PackData data;

}

public class DailyRewards_UI : ui_basement
{
    public SpinElement[] Elements;
    private SpinElement _current;
    public Animator _blobAnim;
    public Image _prise;
    public DailyRewards _this;

    public GameObject RollButton;

    [Header("RewardObject")]
    public Text _rewardText;
    public GameObject AcceptButton;

    public override void Activate()
    {
        base.Activate();
    }

    public override void DeActivate()
    {
        base.DeActivate();
    }

    public void Roll()
    {
        StartCoroutine(Rolling());
        RollButton.SetActive(false);
    }

    IEnumerator Rolling()
    {
        for (int i = 0; i < 30; i++)
        {
            _current = GetRandomElement();
            _prise.sprite = _current._sprite;
            
            _blobAnim.SetTrigger("blob");
            yield return new WaitForSeconds((float)i / (float)100);
        }

        yield return new WaitForSeconds(1);

        Debug.Log("You get: " + PackEarn());

        _rewardText.text = "X" + PackEarn();

        PlayerStats.instance.AddPack(_current.data._TipsCount, _current.data._AddTimeCount, _current.data._RefreshCount);
        AcceptButton.SetActive(true);
    }

    private string PackEarn()
    {
        if (_current.data._AddTimeCount > 0)
            return  _current.data._AddTimeCount.ToString();
        if (_current.data._TipsCount > 0)
            return  _current.data._TipsCount.ToString();
        if (_current.data._RefreshCount > 0)
            return _current.data._RefreshCount.ToString();

        return "";
    }

    public void CloseCanvas()
    {
        _this.SetDate(System.DateTime.Today);
        CanvasControllerClose();
    }

    SpinElement GetRandomElement()
    {
        SpinElement el = Elements[Random.Range(0, Elements.Length )];
        do
        {
            el = Elements[Random.Range(0, Elements.Length)];


        }
        while (el._sprite == _current._sprite);
        return el;
    }
}
