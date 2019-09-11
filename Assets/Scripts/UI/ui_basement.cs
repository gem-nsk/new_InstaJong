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
               _group = GetComponent<CanvasGroup>();
            }
            return _group;
        }
        set
        {
            if (_group == null)
            {
                _group = GetComponent<CanvasGroup>();
            }
            _group = value;
        }
    }
    private float SmoothTIme = 0.3f;

    public virtual void Activate()
    {
        
        StartCoroutine(SmoothAlpha(1));
    }
    public virtual void DeActivate()
    {
        StartCoroutine(_DeActivate());
    }
    public virtual IEnumerator _DeActivate()
    {
       yield return StartCoroutine(SmoothAlpha(0));
        Destroy(gameObject);
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
