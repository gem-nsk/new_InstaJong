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

    public string t_path;
    public string s_path;
    public string PostsPath;

    public Slider loadingBar;

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


        t_path = Application.persistentDataPath + "/images/thumbnails/";
        s_path = Application.persistentDataPath + "/images/standart/";
        PostsPath = Application.persistentDataPath + "/posts.json";
    }
    #endregion;

    public IEnumerator Init()
    {
        posts.Clear();

        Debug.Log("Inited");

        loadingBar = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<Slider>();
        loadingBar?.gameObject.SetActive(true);


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

                if(CheckImageDirectory())
                {
                    yield return StartCoroutine(LoadImagesFromCache());
                }
                else
                {
                    yield return StartCoroutine(DownloadImagesFromAccount(PlayerStats.instance.AccountKey));
                }

                break;

            case _LoadType.Resources:

                yield return StartCoroutine(LoadingFromResources());

                break;

            case _LoadType.Cache:

                break;
        }
            //DataSave.SavePostsInfo(posts);
        //}
        yield return StartCoroutine(Pack(Atlases[0]));
        yield return StartCoroutine(Pack(Atlases[1]));
        yield return StartCoroutine(CreateMaterials(Atlases[0]));
        yield return StartCoroutine(CreateMaterials(Atlases[1]));
    }

    public bool CheckImageDirectory()
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
            if(loadingBar != null)
            {

                loadingBar.gameObject.SetActive(true);
                loadingBar.maxValue = totalCount;
                loadingBar.value = step;

            }
        }
        else
        {
            if(loadingBar != null)
            loadingBar.gameObject.SetActive(false);
        }
    }

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

    public void CheckAccount(string AccountName)
    {
        StartCoroutine(Finding(AccountName));
    }
    public IEnumerator Finding(string accountName)
    {
        UnityWebRequest IdRequest = UnityWebRequest.Get("https://www.instagram.com/" + accountName + "/?__a=1");
        yield return IdRequest.SendWebRequest();


        if (IdRequest.downloadHandler.data.Length != 20713) 
        {
            var _accId = JsonConvert.DeserializeObject<Assets.Accounts.RootObject>(IdRequest.downloadHandler.text);
            UnityWebRequest request = UnityWebRequest.Get("https://www.instagram.com/graphql/query/?query_id=17888483320059182&id=" + _accId.graphql.user.id + "&first=20");
            yield return request.SendWebRequest();

            var dyn = JsonConvert.DeserializeObject<Assets.Accounts.LoadImages.RootObject>(request.downloadHandler.text);

            if(dyn.data.user.edge_owner_to_timeline_media.edges.Count >= 20)
            {
                PlayerStats.instance.AccountKey = accountName;
                _successFinded?.Invoke();

            }
            else
            {
                PlayerStats.instance.AccountKey = null;
                _failedAcc?.Invoke("Photos less then 20!");
            }
        }
        else
        {
            PlayerStats.instance.AccountKey = null;
            _failedAcc?.Invoke("Account not found!");
        }
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


        root_posts _posts = DataSave.GetpostsData();

        loadingBar.maxValue = _posts._p.Count;
        loadingBar.value = 0;

        if (_posts.AccountKey != PlayerStats.instance.AccountKey)
        {
            yield return StartCoroutine(DownloadImagesFromAccount(PlayerStats.instance.AccountKey));
            yield break;
        }



        string[] tpath = Directory.GetFiles(t_path);
        string[] spath = Directory.GetFiles(s_path);



        for (int i = 0; i < _posts._p.Count; i++)
        {

            //UnityWebRequest t_request = UnityWebRequestTexture.GetTexture(tpath[i]);
            //UnityWebRequest s_request = UnityWebRequestTexture.GetTexture(spath[i]);

            //UnityWebRequest t_request = new UnityWebRequest(tpath[i]);
            //UnityWebRequest s_request = new UnityWebRequest(spath[i]);


            //yield return t_request.SendWebRequest();
            //yield return s_request.SendWebRequest();

            byte[] t_imgBytes = File.ReadAllBytes(t_path + "t_" + i + ".png"); // File.ReadAllBytes(tpath[i]);
            byte[] s_imgBytes = File.ReadAllBytes(s_path + "s_" + i + ".png");

            Texture2D t_tex = new Texture2D(2, 2);
            Texture2D s_tex = new Texture2D(2, 2);

            t_tex.LoadImage(t_imgBytes);
            s_tex.LoadImage(s_imgBytes);

            //yield return t_request.SendWebRequest();
            //yield return s_request.SendWebRequest();



            PostInfo info = new PostInfo
            {
                StandartTexture = s_tex, //((DownloadHandlerTexture)s_request.downloadHandler).texture,
                ThumbnailTexture = t_tex, //((DownloadHandlerTexture)t_request.downloadHandler).texture,
                comments = _posts._p[i].comments,
                thumbnail = _posts._p[i].thumbnail,
                description = _posts._p[i].description,
                id = _posts._p[i].id,
                likes = _posts._p[i].likes,
                post_url = _posts._p[i].post_url,
                standard = _posts._p[i].standard,
                usernameFrom = _posts._p[i].usernameFrom
            };

            posts.Add(info);
            //t_request.Abort();
            //s_request.Abort();
        }
        #region wtf
        ////load thumbnails;
        //if (tpath.Length == _posts._p.Count)
        //{
        //    foreach (string _s in tpath)
        //    {
        //        var post_info = new PostInfo();


        //        post_info.ThumbnailTexture = 

        //        posts.Add(post_info);
        //        loadingBar.value++;
        //    }
        //}
        //else
        //{
        //    Debug.Log("not all files downloaded, reload files...");

        //    Directory.Delete(Application.persistentDataPath + "/images", true);
        //}

        ////load standart textures
        //if (spath.Length == _posts._p.Count)
        //{
        //    foreach (string _s in spath)
        //    {
        //        var post_info = new PostInfo();

        //        UnityWebRequest request = UnityWebRequestTexture.GetTexture(_s);
        //        yield return request.SendWebRequest();

        //        post_info.StandartTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        //        posts.Add(post_info);
        //        loadingBar.value++;
        //    }
        //}
        //else
        //{
        //    Debug.Log("not all files downloaded, reload files...");

        //    Directory.Delete(Application.persistentDataPath + "/images", true);
        //}
        #endregion

        loadingBar.gameObject.SetActive(false);
    }

    public void ClearCache()
    {
        if(Directory.Exists(t_path))
            Directory.Delete(t_path, true);
        if (Directory.Exists(s_path))
            Directory.Delete(s_path, true);
    }

    public IEnumerator DownloadImagesFromInstagram(string token)
    {
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
            
            if(data.caption != null)
            post_info.description = data.caption.text;
            post_info.likes = data.likes.count;
            post_info.comments = data.comments.count;

            if(data.caption != null)
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

            yield return t_request.isDone;

            post_info.ThumbnailTexture = ((DownloadHandlerTexture)t_request.downloadHandler).texture;

            posts.Add(post_info);

           // DataSave.SaveImage(post_info.ThumbnailTexture, "t_" + post_info.id, Application.persistentDataPath + "/t_images");
          //  DataSave.SaveImage(post_info.StandartTexture, "s_" + post_info.id, Application.persistentDataPath + "/s_images");

            i++;
            SetLoadingSettings(dyn.data.Count, i);
        }
        yield return null;

        DataSave.SavePostsInfo(posts);

        SetLoadingSettings(0, 0);
    }

    //https://www.instagram.com/graphql/query/?query_id=17888483320059182&id=20021759479&first=20
    public IEnumerator DownloadImagesFromAccount(string account)
    {
        UnityWebRequest IdRequest = UnityWebRequest.Get("https://www.instagram.com/" + account + "/?__a=1");
        yield return IdRequest.SendWebRequest();
        //get account id
        var _accId = JsonConvert.DeserializeObject<Assets.Accounts.RootObject>(IdRequest.downloadHandler.text);

        UnityWebRequest request = UnityWebRequest.Get("https://www.instagram.com/graphql/query/?query_id=17888483320059182&id=" + _accId.graphql.user.id + "&first=20");
        yield return request.SendWebRequest();

        var dyn = JsonConvert.DeserializeObject<Assets.Accounts.LoadImages.RootObject>(request.downloadHandler.text);
        int i = 1;

        posts = new List<PostInfo>();

        foreach (var data in dyn.data.user.edge_owner_to_timeline_media.edges)
        {
            var post_info = new PostInfo();

            post_info.id = i;
            post_info.thumbnail = data.node.thumbnail_src;
            post_info.standard = data.node.display_url;

            if(data.node.edge_media_to_caption.edges.Count > 0)
            post_info.description = data.node.edge_media_to_caption.edges[0].node.text;
            post_info.likes = data.node.edge_media_preview_like.count;
            post_info.comments = data.node.edge_media_to_comment.count;
            post_info.usernameFrom = _accId.graphql.user.username;

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

            yield return t_request.isDone;

            post_info.ThumbnailTexture = ((DownloadHandlerTexture)t_request.downloadHandler).texture;

            posts.Add(post_info);

            // DataSave.SaveImage(post_info.ThumbnailTexture, "t_" + post_info.id, Application.persistentDataPath + "/t_images");
            //  DataSave.SaveImage(post_info.StandartTexture, "s_" + post_info.id, Application.persistentDataPath + "/s_images");

            i++;
            SetLoadingSettings(dyn.data.user.edge_owner_to_timeline_media.edges.Count, i);
        }
        yield return null;

        DataSave.SavePostsInfo(posts);

        SetLoadingSettings(0, 0);
    }
    public void CreatePost(PostInfo data)
    {
        PostInfo post = data;
        posts.Add(post);
    }
}
