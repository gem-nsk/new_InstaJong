using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
[System.Serializable]
public class Element
{
    public int id;
    public int type;
    public string value;
}

public class RootHistory
{
    public List<Element> history = new List<Element>();
    public RootHistory (Element element)
    {
        bool flg = false;
        string filepath = Application.persistentDataPath + "/history.json";
        if (!File.Exists(filepath))
        {
            RootHistory root = new RootHistory();
            string str = JsonUtility.ToJson(root, true);

            File.WriteAllText(Application.persistentDataPath + "/history.json", str);
        }

        string dataAsJson = File.ReadAllText(filepath);

        history = JsonUtility.FromJson<RootHistory>(dataAsJson).history;
        foreach (Element _element in history)
        {
            if (_element.value == element.value)
                flg = true;

        }
        if (!flg)
            history.Add(element);

    }

    public RootHistory(List<Element> history)
    {
        this.history = history;
    }
    public RootHistory()
    {
        
    }

}

public class History : MonoBehaviour
{


    public static void SaveToHistory(string _value, int _type, int _id)
    {
        Element element = new Element
        {
            id = _id,
            type = _type,
            value = _value
        };

        RootHistory historyJSON = new RootHistory(element);

        string str = "";

        str = JsonUtility.ToJson(historyJSON, true);

        File.WriteAllText(Application.persistentDataPath + "/history.json", str);
        Debug.Log(Application.persistentDataPath);
    }

    public static void SaveToHistory(Element element)
    {

        RootHistory historyJSON = new RootHistory(element);
        
        string str = "";

        str = JsonUtility.ToJson(historyJSON, true);

        File.WriteAllText(Application.persistentDataPath + "/history.json", str);
        Debug.Log(Application.persistentDataPath);
    }

    public static void SaveToHistory(List<Element> history)
    {

        RootHistory historyJSON = new RootHistory();
        historyJSON.history = history;
        string str = "";

        str = JsonUtility.ToJson(historyJSON, true);

        File.WriteAllText(Application.persistentDataPath + "/history.json", str);
        Debug.Log(Application.persistentDataPath);
    }


    public static List<Element> ShowHistory()
    {
        string filepath = Application.persistentDataPath + "/history.json";
        if (File.Exists(filepath))
        {

            string dataAsJson = File.ReadAllText(filepath);

            RootHistory historyJSON = JsonUtility.FromJson<RootHistory>(dataAsJson);

            return historyJSON.history;
            
        }
        return null;
    }

    public static int DeleteUserFromHistory (string username)
    {
        List<Element> history = ShowHistory();
        Debug.Log(history.Count);
        foreach (Element data in history)
        {
            if (data.value == username)
            {
                history.Remove(data);
                break;
            }
        }
        Debug.Log(history.Count);
        if (history.Count == 0) ClearHistory();
        else
        {
            SaveToHistory(history);
        }
        
        Debug.Log("Deleted");
        return 0;
    }

    public static int ClearHistory()
    {
        File.Delete(Application.persistentDataPath + "/history.json");
        return 0;
    }

    public static int RefreshHistory()
    {
        return 0;
    }
}
