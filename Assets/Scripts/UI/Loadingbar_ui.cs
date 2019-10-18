using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loadingbar_ui : ui_basement
{
    public Slider progressSlider;

    private float _value;
    private float _totalValue;

    public RectTransform TipSize;
    public Text TipText;

    public override void Activate()
    {
        base.Activate();
        DownloadManager.ProgressHandler += SetValues;
        StartCoroutine(SmoothLoad());
        StartCoroutine(ShowTips());
    }

    public void SetValues(int value, int totalValue)
    {
        _value = value;
        _totalValue = totalValue;
    }

    public IEnumerator ShowTips()
    {
        while(true)
        {
            string str = Tips.GetRandomTip();
            TipText.text = str;
            yield return StartCoroutine(SizeTip(true));
            yield return new WaitForSeconds(5);
            yield return StartCoroutine(SizeTip(false));

        }
    }
    public IEnumerator SizeTip(bool hided)
    {
        float _time = 0;
        float _smoothTime = 1;

        while (_time < _smoothTime)
        {
            TipSize.sizeDelta = new Vector2(TipSize.sizeDelta.x, Mathf.Lerp(TipSize.sizeDelta.y, hided? 440: 150, _time / _smoothTime));
            _time += Time.deltaTime;
            yield return null;
        }
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
