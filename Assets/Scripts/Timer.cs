﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    public Slider timerSlider;

    public float TotalTime = 600;
    public float _time;
    public bool _isPaused;

   public void StartTimer()
    {
        _time = TotalTime;
        StartCoroutine(TimerProgress());
    }
   
    public void SetTime(float time)
    {
        _time = time;
        StartCoroutine(TimerProgress());
    }

    public IEnumerator TimerProgress()
    {
        GameControllerScr gameController = GameControllerScr.instance;

        timerSlider.maxValue = TotalTime;

        while (_time > 0)
        {
            switch (_isPaused)
            {
                case false:
                    timerSlider.value = _time;

                    _time -= Time.deltaTime;
                    yield return null;
                    break;
                case true:

                    yield return null;
                    break;
            }
            
           
        }
        Debug.Log("Time is ended!");
        gameController.endGameFlag = 2;
    }
    public void AddTime()
    {
        _time = TotalTime;
    }
    public void TimerState(bool isPaused)
    {
        _isPaused = isPaused;
    }
}