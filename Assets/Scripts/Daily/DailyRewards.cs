using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DailyRewards : MonoBehaviour
{
    //public const string _dailyRewardKey = "_dailyReward";
    public const string _refresh_localization_key  = "_pack_refresh";
    public const string _tip_localization_key = "_pack_tip";
    public const string _addtime_localization_key = "_pack_addtime";
    public const string _daily = "_daily";

    public const string _lastDayKey = "_lastDay";
    public string _DebugaDate;

    public GameObject DailyUI;

    public _PackData[] _packVariants;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if(PlayerPrefs.HasKey(_lastDayKey))
        {
            //calcDays
            System.DateTime date = System.Convert.ToDateTime(PlayerPrefs.GetString(_lastDayKey));
            if(date != System.Convert.ToDateTime(_DebugaDate)) //System.DateTime.Today)
            {
                GetReward();
                Debug.Log(GenerateMessege(_packVariants[0]));
                SetDate(System.DateTime.Today);
            }
        }
        else
        {
            //set first date
            SetDate(System.DateTime.Today);
        }
    }

    public void SetDate(System.DateTime _date)
    {
        PlayerPrefs.SetString(_lastDayKey, _date.ToString());
        Debug.Log("Set new date: " + _date);
    }

    public void GetReward()
    {
        Debug.Log("New day, take a reward!");
        CanvasController.instance.OpenCanvas(DailyUI);
    }

    public string GenerateMessege(_PackData _data)
    {
        string msg = LocalizationManager.instance.GetLocalizedValue(_daily);

        if(_data._AddTimeCount > 0)
        {
            msg += "\n" + LocalizationManager.instance.GetLocalizedValue(_addtime_localization_key);
        }
        if (_data._RefreshCount > 0)
        {
            msg += "\n" + LocalizationManager.instance.GetLocalizedValue(_refresh_localization_key);
        }
        if (_data._TipsCount > 0)
        {
            msg += "\n" + LocalizationManager.instance.GetLocalizedValue(_tip_localization_key);
        }

        return msg;
    }
}
