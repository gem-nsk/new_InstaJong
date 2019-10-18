using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class Download_FindAccount : Iloading
{
    private root_posts posts;

    public root_posts GetPosts()
    {
        return posts;
    }

    public IEnumerator Loading(string key)
    {
        posts = new root_posts();

        UnityWebRequest IdRequest = UnityWebRequest.Get("https://www.instagram.com/" + key + "/?__a=1");
        yield return IdRequest.SendWebRequest();
        //get account id
        var _accId = JsonConvert.DeserializeObject<Assets.Accounts.RootObject>(IdRequest.downloadHandler.text);

        UnityWebRequest request = UnityWebRequest.Get("https://www.instagram.com/graphql/query/?query_id=17888483320059182&id=" + _accId.graphql.user.id + "&first=36");
        yield return request.SendWebRequest();

        var dyn = JsonConvert.DeserializeObject<Assets.Accounts.LoadImages.RootObject>(request.downloadHandler.text);
        int i = 1;

        posts.AccountKey = _accId.graphql.user.username;

        DownloadManager.instance.CreateLoadingBar();

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
            post_info.postLink = data.node.shortcode;

            UnityWebRequest s_request = new UnityWebRequest();
            s_request = UnityWebRequestTexture.GetTexture(post_info.standard, false);
            yield return s_request.SendWebRequest();

            if (s_request.isNetworkError || s_request.isHttpError)
                Debug.Log("Error");

            yield return s_request.isDone;

            post_info.StandartTexture = ((DownloadHandlerTexture)s_request.downloadHandler).texture;

            posts._p.Add(post_info);

            // DataSave.SaveImage(post_info.ThumbnailTexture, "t_" + post_info.id, Application.persistentDataPath + "/t_images");
            //  DataSave.SaveImage(post_info.StandartTexture, "s_" + post_info.id, Application.persistentDataPath + "/s_images");

            DownloadManager.ProgressHandler?.Invoke(i, dyn.data.user.edge_owner_to_timeline_media.edges.Count);

            i++;
        }
        yield return null;
    }
    public bool isContainErrors()
    {
        if (posts.AccountKey.Contains(DownloadManager.less20Error) || posts.AccountKey.Contains(DownloadManager.notFoundError))
            return true;
        else
            return false;
    }
}
