using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Transform _Conteiner;
    public Image _ImageConteiner;
    public Text _TextContent;

    public IEnumerator Init()
    {
        yield return StartCoroutine(GameControllerScr.instance.SearchPath());

        GameObject[] helpers = GameControllerScr.instance.GetHelpers();

        yield return StartCoroutine(AnimateStart());
        yield return StartCoroutine(textAnimate(0));

        //helpers[0].transform.SetParent(_Conteiner);
        //helpers[1].transform.SetParent(_Conteiner);
    }

    IEnumerator AnimateStart()
    {
        float _time = 0;
        float _elapsedTime = 3;

        _ImageConteiner.color = new Color(_ImageConteiner.color.r, _ImageConteiner.color.g, _ImageConteiner.color.b, 0);

        while (_time < _elapsedTime)
        {
            _ImageConteiner.color = new Color(_ImageConteiner.color.r, _ImageConteiner.color.g, _ImageConteiner.color.b, Mathf.Lerp(_ImageConteiner.color.a, 0.8f, _time / _elapsedTime));

            _time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator textAnimate(int _step)
    {
        yield return StartCoroutine(TextShow());
        yield return null;
        yield return StartCoroutine(TextHide());
    }    

    public void WaitForTouch()
    {

    }

    IEnumerator TextShow()
    {
        float _time = 0;
        float _elapsedTime = 1;

        RectTransform _TRect = _TextContent.GetComponent<RectTransform>();

        while(_time < _elapsedTime)
        {
            _TRect.localPosition = Vector3.Lerp(_TRect.localPosition, new Vector3(0,0, _TRect.localPosition.z), _time / _elapsedTime);

            _time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator TextHide()
    {
        float _time = 0;
        float _elapsedTime = 1;

        RectTransform _TRect = _TextContent.GetComponent<RectTransform>();

        while (_time < _elapsedTime)
        {
            _TRect.localPosition = Vector3.Lerp(_TRect.localPosition, new Vector3(0, -1000, _TRect.localPosition.z), _time / _elapsedTime);

            _time += Time.deltaTime;
            yield return null;
        }
    }
}
