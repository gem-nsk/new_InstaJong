using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadFromAccount : LoadType
{
    List<PostInfo> posts = new List<PostInfo>();

    public LoadFromAccount(string accountId)
    {
    }

    public void GetPosts()
    {
    }

    public int GetProgress()
    {
        return 0;
    }

    public void Load(int args)
    {
    }

    public IEnumerator Loading(string args)
    {
        UnityWebRequest IdRequest = UnityWebRequest.Get("https://www.instagram.com/" + args + "/?__a=1");
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

            if (data.node.edge_media_to_caption.edges.Count > 0)
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
        }
        yield return null;

        DataSave.SavePostsInfo(posts);
    }
}
