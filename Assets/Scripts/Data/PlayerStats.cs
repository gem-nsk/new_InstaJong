using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerStats : MonoBehaviour
{
    public int InstaCoins;
    private const string _instaCoinsPath = "InstaCoins";
    public int DefaultInstaCoinsValue = 500;

    public int Points;
    private const string _pointsPath = "Points";

    public int RefreshPrice = 5;
    public int HelpPrice = 20;
    public int AddTimePrice = 50;


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

    //constructor
    public void LoadData()
    {
        if (PlayerPrefs.HasKey(_instaCoinsPath) && PlayerPrefs.HasKey(_pointsPath))
        {
            InstaCoins = PlayerPrefs.GetInt(_instaCoinsPath);
            Points = PlayerPrefs.GetInt(_pointsPath);
        }
        else //default values
        {
            InstaCoins = DefaultInstaCoinsValue;
            Points = 0;
        }
    }

   public void SetPointsTo(int to)
    {
        Points = to;
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(_instaCoinsPath, InstaCoins);
        PlayerPrefs.SetInt(_pointsPath, Points);
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