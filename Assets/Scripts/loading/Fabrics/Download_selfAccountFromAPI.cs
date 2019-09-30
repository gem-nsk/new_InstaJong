using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Assets.Scripts;

public class Download_selfAccountFromAPI : Iloading
{
    private root_posts _tempPosts;


    public root_posts GetPosts()
    {
        return _tempPosts;
    }

    public bool isContainErrors()
    {
        if (_tempPosts.AccountKey.Contains(DownloadManager.less20Error) || _tempPosts.AccountKey.Contains(DownloadManager.notFoundError))
            return true;
        else
            return false;
    }

    public IEnumerator Loading(string key)
    {
        _tempPosts = new root_posts();

        UnityWebRequest request = UnityWebRequest.Get("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + key);
        yield return request.SendWebRequest();

        var dyn = JsonConvert.DeserializeObject<RootObject>(request.downloadHandler.text);
        int i = 1;

        _tempPosts.AccountKey = dyn.data[0].caption.from.username;

        foreach (var data in dyn.data)
        {
            var post_info = new PostInfo();

            post_info.id = i;
            post_info.thumbnail = data.images.thumbnail.url;
            post_info.standard = data.images.standard_resolution.url;

            if (data.caption != null)
                post_info.description = data.caption.text;
            post_info.likes = data.likes.count;
            post_info.comments = data.comments.count;

            if (data.caption != null)
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

            _tempPosts._p.Add(post_info);

            // DataSave.SaveImage(post_info.ThumbnailTexture, "t_" + post_info.id, Application.persistentDataPath + "/t_images");
            //  DataSave.SaveImage(post_info.StandartTexture, "s_" + post_info.id, Application.persistentDataPath + "/s_images");

            i++;

            //progress
            DownloadManager.ProgressHandler?.Invoke(i, dyn.data.Count);

        }
        yield return null;
    }

}
