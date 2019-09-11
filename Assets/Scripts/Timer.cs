using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    public Slider timerSlider;

    public float TotalTime = 600;
    private float _time;

    private void Start()
    {
        StartTime();
    }
    public void StartTime()
    {
        StartCoroutine(TimerProgress());
    }

    public IEnumerator TimerProgress()
    {
        _time = TotalTime;
        timerSlider.maxValue = TotalTime;

        while(_time > 0)
        {
            timerSlider.value = _time;

            _time -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Time is ended!");
    }
}