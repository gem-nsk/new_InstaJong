using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rules_ui : ui_basement
{

    public int _slide = 0;
    public GameObject[] go_Slides;

    public Image[] SlidesIcons;

    public override void CanvasControllerClose()
    {
        base.CanvasControllerClose();
    }
    public override void Activate()
    {
        base.Activate();
        NextSlide(false);
    }

    public void NextSlide(bool next)
    {
        _slide = Mathf.Clamp(_slide += next ? -1 : 1, 0, go_Slides.Length - 1);
        UpdateSlides();
    }
    void UpdateSlides()
    {
        for (int i = 0; i < go_Slides.Length; i++)
        {
            if(i == _slide)
            {
                go_Slides[i].SetActive(true);
                SlidesIcons[i].color = new Color(1,1,1,0.8f);
            }
            else
            {
                go_Slides[i].SetActive(false);
                SlidesIcons[i].color = new Color(1, 1, 1, 1f);
            }
        }
    }
}
