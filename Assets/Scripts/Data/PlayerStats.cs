using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct loadSettings
{
    public bool SelfAccount;
    public string AcountId;
}

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

    public string AccountKey;

    public string accountId;
    
    public delegate void del_AddPoint(int points);
    public del_AddPoint _addPoints;

    public delegate void del_AddInstaCoins(int coins);
    public del_AddInstaCoins _addInstaCoins;

    public static PlayerStats instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
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
            InstaCoins = PlayerPrefs.GetInt(_instaCoinsPath);
            Points = PlayerPrefs.GetInt(_pointsPath);
            Points_highscore = PlayerPrefs.GetInt(_points_hs);
        }
        else //default values
        {
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

    public void AddInstaCoins(int _coins)
    {
        InstaCoins += _coins;
        InstaCoins = Mathf.Clamp(InstaCoins, 0, InstaCoins);
        _addInstaCoins?.Invoke(InstaCoins);
        SaveData();
    }
}