using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ui_basement : MonoBehaviour
{
    private CanvasGroup _group;
    private CanvasGroup group
    {
        get
        {
            if(_group == null)
            {
                GetComponent<CanvasGroup>();
            }
            return _group;
        }
        set
        {
            if (_group == null)
            {
                GetComponent<CanvasGroup>();
            }
            _group = value;
        }
    }
    private float SmoothTIme = 1;

    public virtual void Activate()
    {
        
        StartCoroutine(SmoothAlpha(1));
    }
    public virtual void DeActivate()
    {

        StartCoroutine(SmoothAlpha(0));
    }

    IEnumerator SmoothAlpha(float To)
    {
        group.interactable = false;
        float _time = 0;
        while(_time < SmoothTIme)
        {
            group.alpha = Mathf.Lerp(group.alpha, To, _time / SmoothTIme);
            _time += Time.deltaTime;
            yield return null;
        }
        group.interactable = true;
    }
}
