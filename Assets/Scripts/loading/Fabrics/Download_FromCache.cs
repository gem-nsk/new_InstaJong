using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Download_FromCache : Iloading
{
    root_posts posts;

    public string t_path;
    public string s_path;
    public string PostsPath;

    public root_posts GetPosts()
    {
        return posts;
    }

    public IEnumerator Loading(string key)
    {
        posts = new root_posts();

        t_path = Application.persistentDataPath + "/images/thumbnails/";
        s_path = Application.persistentDataPath + "/images/standart/";
        PostsPath = Application.persistentDataPath + "/posts.json";

        root_posts _posts = DataSave.GetpostsData();

        if(_posts != null)
        {
            string[] tpath = Directory.GetFiles(t_path);
            string[] spath = Directory.GetFiles(s_path);

            for (int i = 0; i < _posts._p.Count; i++)
            {
                byte[] t_imgBytes = File.ReadAllBytes(t_path + "t_" + i + ".png"); // File.ReadAllBytes(tpath[i]);
                byte[] s_imgBytes = File.ReadAllBytes(s_path + "s_" + i + ".png");

                Texture2D t_tex = new Texture2D(2, 2);
                Texture2D s_tex = new Texture2D(2, 2);

                t_tex.LoadImage(t_imgBytes);
                s_tex.LoadImage(s_imgBytes);

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

                posts._p.Add(info);
                DownloadManager.ProgressHandler?.Invoke(i, posts._p.Count);
            }
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
