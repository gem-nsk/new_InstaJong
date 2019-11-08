using System.Collections;
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

    public Animator TimeAddObject;

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

        //TimeAddObject.SetActive(false);
        timerSlider.maxValue = TotalTime;

        while (_time > 0)
        {
            switch (_isPaused)
            {
                case false:
                    timerSlider.value = _time;

                    if ((timerSlider.value / timerSlider.maxValue) < 0.3f)
                        TimeAddObject.SetBool("Active", true);
                    else
                        TimeAddObject.SetBool("Active", false);

                    _time -= Time.deltaTime * GameControllerScr.numMap * 0.5f;
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