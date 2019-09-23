using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalization : MonoBehaviour
{
    public string key;
    Text _text;
    // Use this for initialization
    private void Start()
    {
        GetText();
    }
    public void GetText()
    {
        _text = GetComponent<Text>();
        _text.text = LocalizationManager.instance.GetLocalizedValue(key);
    }

    public void AddToText(string t)
    {
        GetText();
        _text.text += t;
    }

    private void OnEnable()
    {
        LocalizationManager.instance.Addtext(this);
    }
    private void OnDisable()
    {
        LocalizationManager.instance.RemoveText(this);
    }
}
