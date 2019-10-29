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

    public GameObject LoadingCanvas;

    public delegate void _DownloadingProgress(int value, int Total);
    public static _DownloadingProgress ProgressHandler;

    public delegate void ErrorMessage(string msg);
    public static ErrorMessage ErrorHandler;

    public delegate void SuccessfulMessage(string msg);
    public static SuccessfulMessage SuccessfullHandler;

    public const string less20Error = "photos less than 20";
    public const string notFoundError = "not found";

    public root_posts _tempPosts;

    private List<Sprite> sprites = new List<Sprite>();

    void ProgressDebug(int value, int Total)
    {
        Debug.Log("value - " + value + " Total - " + Total);
    }

    public IEnumerator Downloading(string key, Iloading _loading)
    {
        Resources.UnloadUnusedAssets();
        _tempPosts = new root_posts();

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
            ConvertTexturesToSprites ();
            yield return new WaitForEndOfFrame();
            SuccessfullHandler?.Invoke("Account successful loaded");
        }

    }

    public int GetCount()
    {
        return sprites.Count;
    }

    private void ConvertTexturesToSprites()
    {
        foreach(PostInfo postInfo in _tempPosts._p)
        {
            sprites.Add(Sprite.Create(postInfo.StandartTexture, new Rect(0, 0, postInfo.StandartTexture.width, postInfo.StandartTexture.height), Vector2.zero));
        }
    }

    public void StopLoading()
    {
        StopAllCoroutines();
        _tempPosts = new root_posts();
        Debug.Log("Stop loading");
    }

    public root_posts GetPosts()
    {
        return _tempPosts;
    }

    public Sprite GetImageById(int id)
    {
        Debug.Log("id - " + id);
        //return _tempPosts._p[id - 1].StandartTexture;
        return sprites[id-1];
    }

    public void ClearPosts()
    {
        foreach(PostInfo p in _tempPosts._p)
        {
            p.StandartTexture = null;
        }
        _tempPosts._p.Clear();
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    GameObject _bar;
    public void CreateLoadingBar()
    {
       _bar = CanvasController.instance.OpenCanvas(LoadingCanvas);
    }
    public void DeleteLoadingBar()
    {
        if(_bar)
        {
            _bar.GetComponent<ui_basement>().CanvasControllerClose();
        }
    }
}
