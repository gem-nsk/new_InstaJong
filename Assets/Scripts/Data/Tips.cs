using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    public static string GetRandomTip()
    {
        return TipsFile();
    }

    static string TipsFile()
    {
        string lang = LocalizationManager.instance.GetLanguage();

        string[] linesFromfile = Resources.Load<TextAsset>("tips_" + lang).text.Split("\n"[0]);
        return linesFromfile[Random.Range(0, linesFromfile.Length)];
    }
}
