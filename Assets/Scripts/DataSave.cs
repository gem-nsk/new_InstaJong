using System.Collections;
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


    public static void save(root _root)
    {
        //CellJson[] js = new CellJson[list.Count];
        //for (int i = 0; i < list.Count; i++)
        //{
        //    js[i] = list[i].settings;
        //}


        string str = "";

        str = JsonUtility.ToJson(_root, true);

        // string str = JsonUtility.ToJson(js);
        File.WriteAllText(Application.persistentDataPath + "/grid.json", str);

    }

    public static root GetData()
    {
        string filepath = Application.persistentDataPath + "/grid.json";
        if(File.Exists(filepath))
        {

            string dataAsJson = File.ReadAllText(filepath);

            root _root = JsonUtility.FromJson<root>(dataAsJson);

            
            return _root;
        }
        else
        {
            return null;
        }
    }

    public static void SavePostsInfo(root_posts data)
    {
        for (int i = 0; i < data._p.Count; i++)
        {

            SaveImage(data._p[i].StandartTexture, "s_" + i, Application.persistentDataPath + "/images/standart/");
        }

        string str = JsonUtility.ToJson(data,true);

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

        CheckDirectory(filepath);

        var file = File.Open(Path.Combine(filepath, name + ".png"), FileMode.Create, FileAccess.ReadWrite);

        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);

        writer.Close();
    }

    static void CheckDirectory(string dir)
    {
        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

}
