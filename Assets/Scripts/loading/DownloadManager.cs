using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//for different loading types
public interface Iloading
{
    IEnumerator Loading(string key);
    root_posts GetPosts();
    bool isContainErrors();
}

public class DownloadManager : MonoBehaviour
{
    #region Singleton
    public static DownloadManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ProgressHandler += ProgressDebug;
        }
    }
    #endregion
    public delegate void _DownloadingProgress(int value, int Total);
    public static _DownloadingProgress ProgressHandler;

    public delegate void ErrorMessage(string msg);
    public static ErrorMessage ErrorHandler;

    public delegate void SuccessfulMessage(string msg);
    public static SuccessfulMessage SuccessfullHandler;

    public const string less20Error = "photos less than 20";
    public const string notFoundError = "account not found";

    public root_posts _tempPosts;

    void ProgressDebug(int value, int Total)
    {
        Debug.Log("value - " + value + " Total - " + Total);
    }

    public IEnumerator Downloading(string key, Iloading _loading)
    {
        Iloading loading = _loading;

        //checking witch account to load
        yield return StartCoroutine(loading.Loading(key));

        //handlers
        if (loading.isContainErrors())
        {
            ErrorHandler?.Invoke(loading.GetPosts().AccountKey);
        }
        else
        {
            _tempPosts = loading.GetPosts();
            SuccessfullHandler?.Invoke("Account successful loaded");
        }

    }

    public root_posts GetPosts()
    {
        return _tempPosts;
    }
}
