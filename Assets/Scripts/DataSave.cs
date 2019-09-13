using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataSave : MonoBehaviour
{

    public static void save(List<CellScr> list, (int _height, int _width) size)
    {
        //CellJson[] js = new CellJson[list.Count];
        //for (int i = 0; i < list.Count; i++)
        //{
        //    js[i] = list[i].settings;
        //}
        root _root = new root();

        string str = "";
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].settings);

            _root.data.Add(list[i].settings);
        }
        _root.height = size._height;
        _root.width = size._width;

        str = JsonUtility.ToJson(_root);

        // string str = JsonUtility.ToJson(js);
        File.WriteAllText(Application.persistentDataPath + "/grid.json", str);
        Debug.Log(Application.persistentDataPath);
    }

    public static ((int height, int width), List<CellJson>) GetData()
    {
        string filepath = Application.persistentDataPath + "/grid.json";
        string dataAsJson = File.ReadAllText(filepath);

        root js = JsonUtility.FromJson<root>(dataAsJson);
        List<CellJson> list = new List<CellJson>();

        for (int i = 0; i < js.data.Count; i++)
        {
            list.Add(js.data[i]);
        }

        return ((js.height, js.width), list);
    }
}
