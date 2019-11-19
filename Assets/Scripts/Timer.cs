using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    public Slider timerSlider;

    public float TotalTime;
    public float _MaximumTime = 270;
    public float _MinimumTIme = 120;
    public float _time;
    private bool _isPaused;

    public Animator TimeAddObject;

    public void StartTimer()
    {

        TotalTime = Mathf.Clamp(_MaximumTime - (GameControllerScr.numMap * 30), _MinimumTIme, _MaximumTime);

        _time = TotalTime;
        StartCoroutine(TimerProgress());
    }

    public void SetTime(float time)
    {
        _time = time;
        StartCoroutine(TimerProgress());
    }

    public void UpdateTimeValues()
    {
        TotalTime = Mathf.Clamp(_MaximumTime - (GameControllerScr.numMap * 30), _MinimumTIme, _MaximumTime);
        _time = TotalTime;

        timerSlider.maxValue = TotalTime;
        timerSlider.value = _time;
    }

    public void SetPaused(string from, bool _isPaused)
    {
        Debug.Log(from + (_isPaused ? " Unpaused" : " Paused"));
        this._isPaused = _isPaused;
    }

    public IEnumerator TimerProgress()
    {
        GameControllerScr gameController = GameControllerScr.instance;

        Debug.Log("Level time - " + TotalTime);

        //TimeAddObject.SetActive(false);
        timerSlider.maxValue = TotalTime;
        timerSlider.value = _time;
        while (_time > 0)
        {
            switch (_isPaused)
            {
                case false:
                    TotalTime = Mathf.Clamp(_MaximumTime - (GameControllerScr.numMap * 30), _MinimumTIme, _MaximumTime);

                    timerSlider.value = _time;

                    if ((timerSlider.value / timerSlider.maxValue) < 0.3f)
                        TimeAddObject.SetBool("Active", true);
                    else
                        TimeAddObject.SetBool("Active", false);

                    _time -= Time.deltaTime;
                    yield return null;
                    break;
                case true:

                    yield return null;
                    break;
            }
        //TimeAddObject.SetActive(false);
        }
        Debug.Log("Time is ended!");
        //gameController.endGameFlag = 2;
        gameController.OpenEndGamePreview(2);
        GameControllerScr.instance.ClearSave();
    }
    public void AddTime()
    {
        _time = Mathf.Clamp(_time = TotalTime, 0, TotalTime);
    }
    public void AddTime(float time)
    {
        _time = Mathf.Clamp(_time + time, 0, TotalTime);
    }
    public void TimerState(bool isPaused)
    {
        _isPaused = isPaused;
    }
}
