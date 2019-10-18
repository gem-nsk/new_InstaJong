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

    public GameObject TimeAddObject;

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

        TimeAddObject.SetActive(false);
        timerSlider.maxValue = TotalTime;

        while (_time > 0)
        {
            switch (_isPaused)
            {
                case false:
                    timerSlider.value = _time;

                    if ((timerSlider.value / timerSlider.maxValue) < 0.3f)
                        TimeAddObject.SetActive(true);

                    _time -= Time.deltaTime * GameControllerScr.numMap * 0.5f;
                    yield return null;
                    break;
                case true:

                    yield return null;
                    break;
            }
        TimeAddObject.SetActive(false);
        }
        Debug.Log("Time is ended!");
        //gameController.endGameFlag = 2;
        gameController.OpenEndGamePreview(2);
        GameControllerScr.instance.ClearSave();
    }
    public void AddTime()
    {
        _time = TotalTime;
    }
    public void AddTime(float time)
    {
        _time += time;
    }
    public void TimerState(bool isPaused)
    {
        _isPaused = isPaused;
    }
}