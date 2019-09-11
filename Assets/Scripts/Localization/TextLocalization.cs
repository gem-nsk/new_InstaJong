using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalization : MonoBehaviour
{
    public string key;

    // Use this for initialization
    public void GetText()
    {
        Text text = GetComponent<Text>();
        text.text = LocalizationManager.instance.GetLocalizedValue(key);
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
