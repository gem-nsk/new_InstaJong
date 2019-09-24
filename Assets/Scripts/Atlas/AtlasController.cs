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

    public string AccountId;

    public List<PostInfo> posts;

    public _atlas[] Atlases = new _atlas[2];

    public string t_path;
    public string s_path;
    public string PostsPath;

    public Slider loadingBar;


    #region Singleton
    public static AtlasController instance;
    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);


        t_path = Application.persistentDataPath + "/images/thumbnails/";
        s_path = Application.persistentDataPath + "/images/standart/";
        PostsPath = Application.persistentDataPath + "/posts.json";
    }
    #endregion;

    public IEnumerator Init()
    {
        posts.Clear();
        loadingBar = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<Slider>();
        loadingBar?.gameObject.SetActive(true);

        AccountId = PlayerStats.instance.AccountKey;

        //if (CheckImageCache())
        //{
        //    //load cache
        //    yield return StartCoroutine(LoadImagesFromCache());
        //}
        //else
        //{
        //if images not saved
        switch (LoadType)
        {
            case _LoadType.Internet:
                yield return StartCoroutine(DownloadImagesFromInstagram());
                break;
            case _LoadType.Resources:
                yield return StartCoroutine(LoadingFromResources());
                break;
            case _LoadType.Cache:
                break;
        }
            DataSave.SavePostsInfo(posts);
        //}
        Pack(Atlases[0]);
        Pack(Atlases[1]);
        CreateMaterials(Atlases[0]);
        CreateMaterials(Atlases[1]);
    }

    bool CheckImageCache()
    {

        if (Directory.Exists(t_path) && Directory.Exists(s_path) && File.Exists(PostsPath))
        {
            if(Directory.GetFiles(t_path).Length != 0 && Directory.GetFiles(s_path).Length != 0)
            {
                return true;
            }
            return true;
        }
        else
        {
            Directory.CreateDirectory(t_path);
            Directory.CreateDirectory(s_path);
            return false;
        }
    }

    public void SetLoadingSettings(int totalCount, int step)
    {
        if(totalCount  != step)
        {
            loadingBar.gameObject.SetActive(true);
            loadingBar.maxValue = totalCount;
            loadingBar.value = step;
        }
        else
        {
            loadingBar.gameObject.SetActive(false);
        }
    }

    public void CreateMaterials(_atlas a)
    {
        foreach(Rect _rect in a.rect)
        {
            Material _mat = new Material(a.atlas);

            _mat.SetTextureScale("_MainTex", new Vector2(_rect.width, _rect.height));
            _mat.SetTextureOffset("_MainTex", new Vector2(_rect.x, _rect.y));

            a._CreatedMaterials.Add(_mat);
        }
    }
    //returning offset by image id
    public Material GetMaterialById(_atlas a, int id)
    {
        return a._CreatedMaterials[id - 1];
    }
    

    public void Pack(_atlas a)
    {
        //packing textures from download


        Texture2D[] Sprites = new Texture2D[posts.Count];
        for (int i = 0; i < posts.Count; i++)
        {
            Sprites[i] = posts[i].ThumbnailTexture;
        }

        Texture2D texture = new Texture2D(2048, 2048);

        a.rect = texture.PackTextures(Sprites, 5, 4096);
        a.atlas.SetTexture("_MainTex", texture);
    }

    public void StopLoading()
    {
        StopCoroutine(DownloadImagesFromInstagram());
        StopCoroutine(LoadImagesFromCache());
        StopCoroutine(Init());
    }

    public IEnumerator LoadingFromResources()
    {
        TextAsset data = Resources.Load<TextAsset>("saved/posts");
        root_posts root = JsonUtility.FromJson<root_posts>(data.text);

        for (int i = 1; i <= root._p.Count; i++)
        {

            root._p[i - 1].ThumbnailTexture = Resources.Load<Texture2D>("saved/t_images/t_" + i);
            root._p[i - 1].StandartTexture = Resources.Load<Texture2D>("saved/s_images/s_" + i);
            SetLoadingSettings(root._p.Count, i);
        }

        foreach(PostInfo p in root._p)
        {
            posts.Add(p);
        }
        SetLoadingSettings(0, 0);
        yield return null;
    }

    public IEnumerator LoadImagesFromCache()
    {
        string[] t = Directory.GetFiles(t_path);

        root_posts _posts = DataSave.GetpostsData();

        
        loadingBar.maxValue = _posts._p.Count;
        loadingBar.value = 0;
        if (t.Length == _posts._p.Count)
        {
            foreach (string s in t)
            {
                var post_info = new PostInfo();

                UnityWebRequest request = UnityWebRequestTexture.GetTexture(s);
                yield return request.SendWebRequest();

                post_info.ThumbnailTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                posts.Add(post_info);
                Debug.Log("added image");
                loadingBar.value++;
            }
        }
        else
        {
            Debug.Log("not all files downloaded, reload files...");

            Directory.Delete(Application.persistentDataPath + "/images", true);
            yield return StartCoroutine(DownloadImagesFromInstagram());
        }
        loadingBar.gameObject.SetActive(false);
    }

    public IEnumerator DownloadImagesFromInstagram()
    {
        string token = AccountId;
        UnityWebRequest request = UnityWebRequest.Get("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + token);
        yield return request.SendWebRequest();

        var dyn = JsonConvert.DeserializeObject<RootObject>(request.downloadHandler.text);
        int i = 1;

        posts = new List<PostInfo>();


        foreach (var data in dyn.data)
        {
            var post_info = new PostInfo();

            post_info.id = i;
            post_info.thumbnail = data.images.thumbnail.url;
            post_info.standard = data.images.standard_resolution.url;

            post_info.description = data.caption.text;
            post_info.likes = data.likes.count;
            post_info.comments = data.comments.count;
            post_info.usernameFrom = data.caption.from.username;

            UnityWebRequest s_request = new UnityWebRequest();
            s_request = UnityWebRequestTexture.GetTexture(post_info.standard, false);
            yield return s_request.SendWebRequest();

            if (s_request.isNetworkError || s_request.isHttpError)
                Debug.Log("Error");

            yield return s_request.isDone;

            post_info.StandartTexture = ((DownloadHandlerTexture)s_request.downloadHandler).texture;

            //thumbnails 
            UnityWebRequest t_request = new UnityWebRequest();
            t_request = UnityWebRequestTexture.GetTexture(post_info.thumbnail, false);
            yield return t_request.SendWebRequest();

            if (t_request.isNetworkError || t_request.isHttpError)
                Debug.Log("Error");

            yield return s_request.isDone;

            post_info.ThumbnailTexture = ((DownloadHandlerTexture)t_request.downloadHandler).texture;

            posts.Add(post_info);

           // DataSave.SaveImage(post_info.ThumbnailTexture, "t_"+ post_info.id, Application.persistentDataPath + "/t_images");
            //DataSave.SaveImage(post_info.StandartTexture, "s_" + post_info.id, Application.persistentDataPath + "/s_images");

            i++;
            SetLoadingSettings(dyn.data.Count, i);
        }
        yield return null;
        SetLoadingSettings(0, 0);
    }

    public void CreatePost(PostInfo data)
    {
        PostInfo post = data;
        posts.Add(post);
    }
}
