using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int InstaCoins;
    public int DefaultInstaCoinsValue = 500;

    private int _points;
    public int Points
    {
        get
        {
            return _points;
        }
        set
        {
            if(value > Points_highscore)
            {
                Points_highscore = value;
            }
            _points = value;
        }
    }
    public int Points_highscore;

    private const string _instaCoinsPath = "InstaCoins";
    private const string _pointsPath = "Points";
    private const string _IdKey = "AccountId";
    public const string _points_hs = "hs";

    public int RefreshPrice = 5;
    public int HelpPrice = 20;
    public int AddTimePrice = 50;
    public int CoinsPerLevel = 50;

    public const string _id_refresh = "_id_refresh";
    public const string _id_tip = "_id_tip";
    public const string _id_time = "_id_time";

    public int _Count_Refresh;
    public int _Count_Tip;
    public int _Count_Time;

    public (string name, string token) playerSettings
    {
        get
        {
            if (PlayerPrefs.HasKey("name") && PlayerPrefs.HasKey("token"))
            {
                Debug.Log("Player settings get - " + (PlayerPrefs.GetString("name")));
                return (PlayerPrefs.GetString("name"), PlayerPrefs.GetString("token"));
            }
            else
            return ("","");
        }
        set
        {
            Debug.Log("Player setting set - " + value.name);

            PlayerPrefs.SetString("name", value.name);
            PlayerPrefs.SetString("token", value.token);

            PlayerStats.AccountKeyHandler();
        }
    }

    public delegate void AccountKeyChanged();
    public static AccountKeyChanged AccountKeyHandler;

    public delegate void del_AddPoint(int points);
    public del_AddPoint _addPoints;

    public delegate void del_AddInstaCoins(int coins);
    public del_AddInstaCoins _addInstaCoins;

    public delegate void del_changePack(int refresh, int tip, int time);
    public del_changePack _changePackHandler;

    public static PlayerStats instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            LoadData();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public (bool auth, string id) IsUserAuthorized()
    //{
    //    if(PlayerPrefs.HasKey(_IdKey))
    //    {
    //        switch(PlayerPrefs.GetString(_IdKey))
    //        {
    //            case "":
    //                return (false, null);
    //            default:
    //                return (true, AccountKey);
    //        }
    //    }
    //    else
    //    {
    //        return (false, null);
    //    }
    //}

    //constructor
    public void LoadData()
    {
        if (PlayerPrefs.HasKey(_instaCoinsPath) && PlayerPrefs.HasKey(_pointsPath) && PlayerPrefs.HasKey(_points_hs))
        {
            _Count_Time = PlayerPrefs.GetInt(_id_time);
            _Count_Tip = PlayerPrefs.GetInt(_id_tip);
            _Count_Refresh = PlayerPrefs.GetInt(_id_refresh);

            InstaCoins = PlayerPrefs.GetInt(_instaCoinsPath);
            Points = PlayerPrefs.GetInt(_pointsPath);
            Points_highscore = PlayerPrefs.GetInt(_points_hs);
        }
        else //default values
        {

            _Count_Time = 1;
            _Count_Refresh = 3;
            _Count_Tip = 5;

            InstaCoins = DefaultInstaCoinsValue;
            Points = 0;
            Points_highscore = 0;
        }
    }

   public void SetPointsTo(int to)
    {
        Points = to;
        SaveData();
    }

    public int GetHighscore()
    {
        return Points_highscore;
    }

    public int GetScore()
    {
        return _points;
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt(_id_refresh, _Count_Refresh);
        PlayerPrefs.SetInt(_id_time, _Count_Time);
        PlayerPrefs.SetInt(_id_tip, _Count_Tip);

        PlayerPrefs.SetInt(_instaCoinsPath, InstaCoins);
        PlayerPrefs.SetInt(_pointsPath, Points);
        PlayerPrefs.SetInt(_points_hs, Points_highscore);
    }

    public void AddPoints(int _points)
    {
        Points += _points;
        _addPoints?.Invoke(_points);
        SaveData();
    }

    public void AddPack(int _tipCount, int _timeCount, int _refreshCount)
    {
        _Count_Refresh += _refreshCount;
        _Count_Time += _timeCount;
        _Count_Tip += _tipCount;

        if (_changePackHandler != null)
            _changePackHandler(_Count_Refresh, _Count_Tip, _Count_Time);

        SaveData();
    }

    public int AddLevelInstaCoins(int level)
    {
        int _c = level * CoinsPerLevel;
        AddInstaCoins(_c);
        return _c;
    }

    public void AddInstaCoins(int _coins)
    {
        InstaCoins += _coins;
        InstaCoins = Mathf.Clamp(InstaCoins, 0, InstaCoins);
        _addInstaCoins?.Invoke(InstaCoins);
        SaveData();
    }
}