﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class root_posts
{
    public string AccountKey;
    public List<PostInfo> _p = new List<PostInfo>();
}

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
        if(File.Exists(filepath))
        {

            string dataAsJson = File.ReadAllText(filepath);

            root js = JsonUtility.FromJson<root>(dataAsJson);
            List<CellJson> list = new List<CellJson>();

            for (int i = 0; i < js.data.Count; i++)
            {
                list.Add(js.data[i]);
            }

            return ((js.height, js.width), list);
        }
        else
        {
            return ((0,0), null);
        }
    }

    public static void SavePostsInfo(root_posts data)
    {
        for (int i = 0; i < data._p.Count; i++)
        {
            SaveImage(data._p[i].ThumbnailTexture, "t_" + i, Application.persistentDataPath + "/images/thumbnails/");
            SaveImage(data._p[i].StandartTexture, "s_" + i, Application.persistentDataPath + "/images/standart/");
        }

        string str = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/posts.json", str);
    }

   
    public static root_posts GetpostsData()
    {
        root_posts root = new root_posts();
        string filepath = Application.persistentDataPath + "/posts.json";

        if (File.Exists(filepath))
        {
            string dataAsJson = File.ReadAllText(filepath);
            root = JsonUtility.FromJson<root_posts>(dataAsJson);
            return root;
        }
        else
        {
            return null;
        }
    }
    public static void SaveImage(Texture2D texture, string name, string filepath)
    {
        var bytes = texture.EncodeToPNG();
        var file = File.Open(Path.Combine(filepath, name + ".png"), FileMode.Create, FileAccess.ReadWrite);

        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);

        writer.Close();
    }

}
