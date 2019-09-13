using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

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

    public string AccountId;
    

    public List<PostInfo> posts;

    public _atlas[] Atlases = new _atlas[2];


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

    public IEnumerator Init()
    {
        yield return StartCoroutine(DownloadImagesFromInstagram());
        Pack(Atlases[0]);
        Pack(Atlases[1]);
        CreateMaterials(Atlases[0]);
        CreateMaterials(Atlases[1]);
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
    public IEnumerator DownloadImagesFromInstagram()
    {

        
        string token = AccountId;
        UnityWebRequest request = UnityWebRequest.Get("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + token);
        yield return request.SendWebRequest();

        //WebClient webClient = new WebClient();
        //var list = webClient.DownloadString("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + token);
        var dyn = JsonConvert.DeserializeObject<RootObject>(request.downloadHandler.text);
        int i = 1;

        posts = new List<PostInfo>();

        foreach (var data in dyn.data)
        {
            var post_info = new PostInfo();
            string new_str;
            post_info.id = i;
            post_info.thumbnail = data.images.thumbnail.url;
            post_info.standard = data.images.standard_resolution.url;

            //if (data.caption.text.Length > 20)
            //    new_str = data.caption.text.Remove(50) + "...";
            //else new_str = data.caption.text;

            new_str = data.caption.text;
            if (new_str.Length > 50) new_str = new_str.Remove(50) + "...";
            post_info.description = new_str;
            post_info.likes = data.likes.count;
            post_info.comments = data.comments.count;
            post_info.usernameFrom = data.caption.from.username;


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
        }
        yield return null;
        Debug.Log( Directory.GetFiles(Application.streamingAssetsPath + "/file/").Length);
    }

}
