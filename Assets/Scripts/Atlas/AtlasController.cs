﻿using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class PostInfo
{
    public int id;
    public string thumbnail;
    public string standard;
    public int likes;
    public int comments;
    public string post_url;
    public string description;
    public string usernameFrom;
    public string postLink;

    public Texture2D StandartTexture;
}
[System.Serializable]
public class _atlas
{
    public Rect[] rect;
    public Material atlas;

    public List<Material> _CreatedMaterials = new List<Material>();
}


public class AtlasController : MonoBehaviour
{
    public _LoadType LoadType;
    public enum _LoadType
    {
        Resources,
        Internet,
        Cache
    }

    //20021759479.9f7d92e.e1400359759e4f7b9c7bd99e85e102e4 - debug
    //1445481979.9f7d92e.b0c1bd961d644d3aa03ec861a3af7974


   // public List<PostInfo> posts;

    public _atlas[] Atlases = new _atlas[2];



    public delegate void successFindedAcc();
    public static successFindedAcc _successFinded;

    public delegate void failedFindAcc(string arg);
    public static failedFindAcc _failedAcc;

    #region Singleton
    public static AtlasController instance;
    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);


    }
    #endregion;

    public IEnumerator Init(root_posts data)
    {
        //posts.Clear();

        //ClearMaterials();

        Debug.Log("Inited");

        Texture2D[] images = new Texture2D[data._p.Count];
        for (int i = 0; i < data._p.Count; i++)
        {
            images[i] = data._p[i].StandartTexture;
        }
        //posts = data._p;

        yield return StartCoroutine(Pack(Atlases[0], 1024, images));
        yield return StartCoroutine(Pack(Atlases[1], 4096, images));
        yield return StartCoroutine(CreateMaterials(Atlases[0]));
        yield return StartCoroutine(CreateMaterials(Atlases[1]));
       // UnloadTextures();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            //UnloadTextures();
            ClearMaterials();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }

    //void UnloadTextures()
    //{
    //    foreach(var v in posts)
    //    {
    //        v.StandartTexture = null;
    //    }
    //}

    void ClearMaterials()
    {
        foreach(_atlas atlas in Atlases)
        {
            for (int i = 0; i < atlas._CreatedMaterials.Count; i++)
            {
                atlas._CreatedMaterials[i] = null;
            }
            atlas.atlas = null;
            atlas._CreatedMaterials.Clear();
        }
    }
    //public bool CheckImageDirectory()
    //{

    //    if (Directory.Exists(t_path) && Directory.Exists(s_path) && File.Exists(PostsPath))
    //    {
    //        if(Directory.GetFiles(t_path).Length != 0 && Directory.GetFiles(s_path).Length != 0)
    //        {
    //            return true;
    //        }
    //        return true;
    //    }
    //    else
    //    {
    //        Directory.CreateDirectory(t_path);
    //        Directory.CreateDirectory(s_path);
    //        return false;
    //    }
    //}

    public IEnumerator CreateMaterials(_atlas a)
    {
        a._CreatedMaterials.Clear();

        foreach(Rect _rect in a.rect)
        {
            Material _mat = new Material(a.atlas);

            _mat.SetTextureScale("_MainTex", new Vector2(_rect.width, _rect.height));
            _mat.SetTextureOffset("_MainTex", new Vector2(_rect.x, _rect.y));

            a._CreatedMaterials.Add(_mat);
            yield return null;

        }
    }
    //returning offset by image id
    public Material GetMaterialById(_atlas a, int id)
    {
        return a._CreatedMaterials[id - 1];
    }
    

    public IEnumerator Pack(_atlas a, int size, Texture2D[] _textures)
    {
        //packing textures from download


        //Texture2D[] Sprites = new Texture2D[_textures.Length];
        //for (int i = 0; i < _textures.Length; i++)
        //{
        //    Sprites[i] = posts[i].StandartTexture;
        //}

        Texture2D texture = new Texture2D(2,2);

        a.rect = texture.PackTextures(_textures, 5, size);
        a.atlas.SetTexture("_MainTex", texture);
        Debug.Log("Atlas width: " + a.atlas.mainTexture.width + "Atlas height: " + a.atlas.mainTexture.height);
        yield return null;
    }

    public void StopLoading()
    {
        StopAllCoroutines();    
    }


    //public IEnumerator LoadingFromResources()
    //{
    //    TextAsset data = Resources.Load<TextAsset>("saved/posts");
    //    root_posts root = JsonUtility.FromJson<root_posts>(data.text);

    //    for (int i = 1; i <= root._p.Count; i++)
    //    {

    //        root._p[i - 1].StandartTexture = Resources.Load<Texture2D>("saved/s_images/s_" + i);
    //    }

    //    foreach(PostInfo p in root._p)
    //    {
    //        posts.Add(p);
    //    }
    //    yield return null;
    //}

    //public void ClearCache()
    //{
    //    if(Directory.Exists(t_path))
    //        Directory.Delete(t_path, true);
    //    if (Directory.Exists(s_path))
    //        Directory.Delete(s_path, true);
    //}


    //https://www.instagram.com/graphql/query/?query_id=17888483320059182&id=20021759479&first=20
    
    //public void CreatePost(PostInfo data)
    //{
    //    PostInfo post = data;
    //    posts.Add(post);
    //}
}
//https://api.instagram.com/v1/users/self/follows?access_token=20021759479.9f7d92e.e4cf6803ec204e899ce887aab2b88cbf