using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class SearchAccount : Iloading
{
    root_posts posts;
    

    public root_posts GetPosts()
    {
        return posts;
    }

    public IEnumerator Loading(string key)
    {
        posts = new root_posts();

        UnityWebRequest IdRequest = UnityWebRequest.Get("https://www.instagram.com/" + key + "/?__a=1");
        yield return IdRequest.SendWebRequest();

        Debug.Log("trying get account: " + key);

        if (IdRequest.downloadHandler.data.Length != 20713)
        {
            var _accId = JsonConvert.DeserializeObject<Assets.Accounts.RootObject>(IdRequest.downloadHandler.text);
            UnityWebRequest request = UnityWebRequest.Get("https://www.instagram.com/graphql/query/?query_id=17888483320059182&id=" + _accId.graphql.user.id + "&first=36");
            yield return request.SendWebRequest();

            var dyn = JsonConvert.DeserializeObject<Assets.Accounts.LoadImages.RootObject>(request.downloadHandler.text);

            if (dyn.data.user.edge_owner_to_timeline_media.edges.Count >= 36)
            {
                posts.AccountKey = _accId.graphql.user.username;
            }
            else
            {
                posts.AccountKey = DownloadManager.less20Error;
            }
        }
        else
        {
            posts.AccountKey = DownloadManager.notFoundError;
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
