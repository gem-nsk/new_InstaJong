using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loadingbar_ui : ui_basement
{
    public Slider progressSlider;

    private float _value;
    private float _totalValue;

    public override void Activate()
    {
        base.Activate();
        DownloadManager.ProgressHandler += SetValues;
        StartCoroutine(SmoothLoad());
    }

    public void SetValues(int value, int totalValue)
    {
        _value = value;
        _totalValue = totalValue;
    }
    IEnumerator SmoothLoad()
    {
        while(true)
        {
            progressSlider.maxValue = _totalValue;
            progressSlider.value = Mathf.Lerp(progressSlider.value, _value, 5 * Time.deltaTime);
            yield return null;
        }
    }
    public override void DeActivate()
    {
        DownloadManager.ProgressHandler -= SetValues;
        StopCoroutine(SmoothLoad());
        base.DeActivate();
    }

    public void CancelLoading()
    {
        DownloadManager.instance.StopLoading();
        CanvasController.instance.CloseCanvas(this);
    }
}
