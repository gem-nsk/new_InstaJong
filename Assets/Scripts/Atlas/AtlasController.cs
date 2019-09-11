using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AtlasController : MonoBehaviour
{

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
    }

    public List<PostInfo> posts;

    public Texture2D[] Sprites;

    public Rect[] rect;
    public Material mat;

    public List<Material> _CreatedMaterials = new List<Material>();
    public List<string> descriptions;

    #region Singleton
    public static AtlasController instance;
    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        Pack();
        CreateMaterials();
        DownloadImagesFromInstagram();    

    }
    #endregion;
  
    public void CreateMaterials()
    {
        foreach(Rect _rect in rect)
        {
            Material _mat = new Material(mat);

            _mat.SetTextureScale("_MainTex", new Vector2(_rect.width, _rect.height));
            _mat.SetTextureOffset("_MainTex", new Vector2(_rect.x, _rect.y));


            _CreatedMaterials.Add(_mat);
        }
    }
    //returning offset by image id
    public Material GetMaterialById(int id)
    {
        return _CreatedMaterials[id - 1];
    }
    

    public void Pack()
    {
        Texture2D atlas = new Texture2D(2048, 2048);
        rect = atlas.PackTextures(Sprites, 2, 4096);
        mat.SetTexture("_MainTex", atlas);
    }

    public void DownloadImagesFromInstagram()
    {

        
        string token = "55595064.dd12fa9.6dc460358d3544e0a1fc2cac28dcff9b";
        WebClient webClient = new WebClient();
        var list = webClient.DownloadString("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + token);
        var dyn = JsonConvert.DeserializeObject<RootObject>(list);
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

            posts.Add(post_info);

            using (WebClient client = new WebClient())
            {
                //client.DownloadFileAsync(new Uri(url), @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\image\file"+i+".jpg");
                client.DownloadFileAsync(new System.Uri(post_info.standard),
                    @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\imageStandard\file" + i + ".jpg");
            }
            i++;

        }
    }
}
