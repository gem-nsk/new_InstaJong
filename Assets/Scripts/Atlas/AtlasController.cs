using Assets.Scripts;
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

    public Texture2D StandartTexture;
    public Texture2D ThumbnailTexture;
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


    public List<PostInfo> posts;

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
        posts.Clear();

        Debug.Log("Inited");

        posts = data._p;

        yield return StartCoroutine(Pack(Atlases[0]));
        yield return StartCoroutine(Pack(Atlases[1]));
        yield return StartCoroutine(CreateMaterials(Atlases[0]));
        yield return StartCoroutine(CreateMaterials(Atlases[1]));
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
        }
        yield return null;
    }
    //returning offset by image id
    public Material GetMaterialById(_atlas a, int id)
    {
        return a._CreatedMaterials[id - 1];
    }
    

    public IEnumerator Pack(_atlas a)
    {
        //packing textures from download


        Texture2D[] Sprites = new Texture2D[posts.Count];
        for (int i = 0; i < posts.Count; i++)
        {
            Sprites[i] = posts[i].ThumbnailTexture;
        }

        Texture2D texture = new Texture2D(2048, 2048);

        a.rect = texture.PackTextures(Sprites, 5);
        a.atlas.SetTexture("_MainTex", texture);
        yield return null;
    }

    public void StopLoading()
    {
        StopAllCoroutines();    
    }

<<<<<<< HEAD
        
<<<<<<< HEAD
        string token = "20021759479.9f7d92e.e1400359759e4f7b9c7bd99e85e102e4";
        WebClient webClient = new WebClient();
        var list = webClient.DownloadString("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + token);
        var dyn = JsonConvert.DeserializeObject<RootObject>(list);
=======
        string token = AccountId;
        UnityWebRequest request = UnityWebRequest.Get("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + token);
        yield return request.SendWebRequest();

        //WebClient webClient = new WebClient();
        //var list = webClient.DownloadString("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + token);
        var dyn = JsonConvert.DeserializeObject<RootObject>(request.downloadHandler.text);
>>>>>>> 46f72f3394774b31af86960bf0e7d392fd3ff319
        int i = 1;
=======

    public IEnumerator LoadingFromResources()
    {
        TextAsset data = Resources.Load<TextAsset>("saved/posts");
        root_posts root = JsonUtility.FromJson<root_posts>(data.text);
>>>>>>> whynotworking

        for (int i = 1; i <= root._p.Count; i++)
        {

            root._p[i - 1].ThumbnailTexture = Resources.Load<Texture2D>("saved/t_images/t_" + i);
            root._p[i - 1].StandartTexture = Resources.Load<Texture2D>("saved/s_images/s_" + i);
        }

        foreach(PostInfo p in root._p)
        {
<<<<<<< HEAD
            var post_info = new PostInfo();
            string new_str;
            post_info.id = i;
            post_info.thumbnail = data.images.thumbnail.url;
            post_info.standard = data.images.standard_resolution.url;

            //if (data.caption.text.Length > 20)
            //    new_str = data.caption.text.Remove(50) + "...";
            //else new_str = data.caption.text;

            new_str = data.caption.text;
            //if (new_str.Length > 50) new_str = new_str.Remove(50) + "...";
            post_info.description = new_str;
            post_info.likes = data.likes.count;
            post_info.comments = data.comments.count;
            post_info.usernameFrom = data.caption.from.username;
<<<<<<< HEAD
            post_info.tamestamps = data.caption.created_time;
            posts.Add(post_info);

            using (WebClient client = new WebClient())
            {
                //client.DownloadFileAsync(new Uri(url), @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\image\file"+i+".jpg");
                client.DownloadFileAsync(new System.Uri(post_info.standard),
                    @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\imageStandard\file" + i + ".jpg");
            }
            using (WebClient client = new WebClient())
            {
                //client.DownloadFileAsync(new Uri(url), @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\image\file"+i+".jpg");
                client.DownloadFileAsync(new System.Uri(post_info.thumbnail),
                    @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\image\file" + i + ".jpg");
            }
            i++;



=======


            //using (WebClient client = new WebClient())
            //{
            //    //client.DownloadFileAsync(new Uri(url), @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\image\file"+i+".jpg");
            //    switch(Application.platform)
            //    {
            //        case RuntimePlatform.Android:
            //            client.DownloadFileAsync(new System.Uri(post_info.thumbnail),
            //       "jar:file://" + Application.dataPath + "!/assets/t_" + i + ".jpg");
            //            break;
            //        default:
            //            client.DownloadFileAsync(new System.Uri(post_info.thumbnail),
            //        Application.streamingAssetsPath + "\file\\t_" + i + ".jpg");
            //            break;
            //    }
            //    client.DownloadFileCompleted += Client_DownloadFileCompleted;

            //       // @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\imageStandard\file" + i + ".jpg");
            //}
            //using (WebClient client = new WebClient())
            //{
            //    //client.DownloadFileAsync(new Uri(url), @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\image\file"+i+".jpg");
            //    switch (Application.platform)
            //    {
            //        case RuntimePlatform.Android:
            //            client.DownloadFileAsync(new System.Uri(post_info.standard),
            //       "jar:file://" + Application.dataPath + "!/assets/s_" + i + ".jpg");
            //            break;
            //        default:
            //            client.DownloadFileAsync(new System.Uri(post_info.standard),
            //        Application.streamingAssetsPath + "\file\\s_" + i + ".jpg");
            //            break;
            //    }

            //    // @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\imageStandard\file" + i + ".jpg");
            //}

            UnityWebRequest s_request = UnityWebRequestTexture.GetTexture(post_info.standard);
            yield return s_request.SendWebRequest();

            if (s_request.isNetworkError || s_request.isHttpError)
                Debug.Log("Error");

            post_info.StandartTexture = ((DownloadHandlerTexture)s_request.downloadHandler).texture;


            //thumbnails 
            UnityWebRequest t_request = UnityWebRequestTexture.GetTexture(post_info.thumbnail);
            yield return t_request.SendWebRequest();

            if (t_request.isNetworkError || t_request.isHttpError)
                Debug.Log("Error");

            post_info.ThumbnailTexture = ((DownloadHandlerTexture)t_request.downloadHandler).texture;

            posts.Add(post_info);

            i++;
>>>>>>> 46f72f3394774b31af86960bf0e7d392fd3ff319
=======
            posts.Add(p);
>>>>>>> whynotworking
        }
        yield return null;
    }

    //public void ClearCache()
    //{
    //    if(Directory.Exists(t_path))
    //        Directory.Delete(t_path, true);
    //    if (Directory.Exists(s_path))
    //        Directory.Delete(s_path, true);
    //}


    //https://www.instagram.com/graphql/query/?query_id=17888483320059182&id=20021759479&first=20
    
    public void CreatePost(PostInfo data)
    {
        PostInfo post = data;
        posts.Add(post);
    }
}
//https://api.instagram.com/v1/users/self/follows?access_token=20021759479.9f7d92e.e4cf6803ec204e899ce887aab2b88cbf