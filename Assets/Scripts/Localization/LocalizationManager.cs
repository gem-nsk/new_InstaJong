using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

    private List<TextLocalization> _localizableTexts = new List<TextLocalization>();

    public _DebugLanguge DebugLanguage;
    public enum _DebugLanguge
    {
        ru,
        eng
    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Addtext(TextLocalization t)
    {
        _localizableTexts.Add(t);
    }
    public void RemoveText(TextLocalization t)
    {
        _localizableTexts.Remove(t);
    }
    public void UpdateTexts()
    {
        foreach(TextLocalization t in _localizableTexts)
        {
            t.GetText();
        }
    }

    public IEnumerator Start()
    {
        string name;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Russian:
                name = "ru";
                break;
            case SystemLanguage.English:
                name = "eng";
                break;
            default:
                name = "eng";
                break;
        }

#if UNITY_EDITOR
        name = DebugLanguage.ToString();
#endif
        LoadLocalizedText(name + ".json");
        while(GetIsReady())
        {
            yield return null;
        }
        UpdateTexts();

    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = "";
#if UNITY_EDITOR
           dataAsJson  = File.ReadAllText(filePath);

#elif UNITY_ANDROID
            filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", fileName);

            WWW reader = new WWW(filePath);
            while (!reader.isDone)
            {
                dataAsJson = reader.text;
            }
#endif
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;

    }

    public bool GetIsReady()
    {
        return isReady;
    }

}
