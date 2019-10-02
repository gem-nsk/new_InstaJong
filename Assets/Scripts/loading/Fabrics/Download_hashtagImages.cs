using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Download_hashtagImages : Iloading
{

    private root_posts posts;

    public root_posts GetPosts()
    {
        return posts;
    }

    public IEnumerator Loading(string key)
    {
        posts = new root_posts();

        UnityWebRequest IdRequest = UnityWebRequest.Get("https://www.instagram.com/explore/tags/" + key + "/?__a=1");
        yield return IdRequest.SendWebRequest();
        //get account id

        if (IdRequest.downloadHandler.data.Length != 20713)
        {
            var _accId = JsonConvert.DeserializeObject<Assets.Accounts.Hashtag.RootObject>(IdRequest.downloadHandler.text);

            Debug.Log(_accId.graphql.hashtag.edge_hashtag_to_media.edges.Count);

            if (_accId.graphql.hashtag.edge_hashtag_to_media.edges.Count < 20)
            {
                posts.AccountKey = DownloadManager.less20Error;
                yield break;
            }
            int i = 1;

            DownloadManager.instance.CreateLoadingBar();

            posts.AccountKey = key;

            foreach (var data in _accId.graphql.hashtag.edge_hashtag_to_media.edges)
            {
                var post_info = new PostInfo();

                post_info.id = i;
                post_info.thumbnail = data.node.thumbnail_src;
                post_info.standard = data.node.display_url;

                if (data.node.edge_media_to_caption.edges.Count > 0)
                    post_info.description = data.node.edge_media_to_caption.edges[0].node.text;
                post_info.likes = data.node.edge_media_preview_like.count;
                post_info.comments = data.node.edge_media_to_comment.count;
                post_info.usernameFrom = _accId.graphql.hashtag.name;

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

                posts._p.Add(post_info);

                // DataSave.SaveImage(post_info.ThumbnailTexture, "t_" + post_info.id, Application.persistentDataPath + "/t_images");
                //  DataSave.SaveImage(post_info.StandartTexture, "s_" + post_info.id, Application.persistentDataPath + "/s_images");

                DownloadManager.ProgressHandler?.Invoke(i, _accId.graphql.hashtag.edge_hashtag_to_media.edges.Count);

                i++;
                if (i > 20)
                {
                    yield break;
                }
            }
            DownloadManager.instance.DeleteLoadingBar();
            yield return null;
        }
        else
        {
            posts.AccountKey = DownloadManager.notFoundError;
            yield break;
        }
    }
    public bool isContainErrors()
    {
        if (posts.AccountKey.Contains(DownloadManager.less20Error) || posts.AccountKey.Contains(DownloadManager.notFoundError))
            return true;
        else
            return false;
    }
}