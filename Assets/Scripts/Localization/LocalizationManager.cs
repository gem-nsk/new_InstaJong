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
        foreach (TextLocalization t in _localizableTexts)
        {
            t.GetText();
        }
    }
    
    public IEnumerator Start()
    {
        string name = GetLanguage();
        

        StartCoroutine(LoadLocalizedText(name + ".json"));
        while (!GetIsReady())
        {
            yield return null;
        }
        UpdateTexts();
        Debug.Log("starting text updates");
    }

    public string GetLanguage()
    {
#if UNITY_EDITOR
        return DebugLanguage.ToString();

#elif UNITY_ANDROID || UNITY_IOS
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Russian:
                return "ru";
            case SystemLanguage.English:
                return "eng";
            default:
                return "eng";
        }
#endif
    }

    public IEnumerator LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string dataAsJson = "";
        LocalizationData loadedData = new LocalizationData();
#if UNITY_EDITOR

        if (File.Exists(filePath))
        {
            dataAsJson = File.ReadAllText(filePath);

            filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", fileName);


            loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries, file name: " + filePath);
        }
#elif UNITY_ANDROID || UNITY_IOS


        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            /*
            while (!reader.isDone)
            {
                dataAsJson = reader.text;
            }*/

            dataAsJson = www.downloadHandler.text;
        }
        else
        {
            dataAsJson = System.IO.File.ReadAllText(filePath);
        }

        loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
#endif
        isReady = true;

        Debug.Log(filePath);
        yield return null;
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

