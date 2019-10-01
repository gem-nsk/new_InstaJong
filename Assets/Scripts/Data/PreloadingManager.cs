using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadingManager : MonoBehaviour
{
    public static PreloadingManager instance;
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
        }
    }

   
    
    IEnumerator LoadingProcess(string _key, Iloading type)
    {
        yield return StartCoroutine( DownloadManager.instance.Downloading(_key, type));
        root_posts _loadedData = DownloadManager.instance.GetPosts();
        yield return StartCoroutine(AtlasController.instance.Init(_loadedData));

    }

    public IEnumerator PreloadAccountImages(string key)
    {
        Iloading search = new SearchAccount();

        Debug.Log("searching...");
        yield return StartCoroutine(PreloadingManager.instance.LoadingProcess(key, search));
        if(!search.isContainErrors())
        {
            Debug.Log("Starting load account data");

            Iloading type = new Download_FindAccount();

            yield return StartCoroutine(PreloadingManager.instance.LoadingProcess(key, type));

            DataSave.SavePostsInfo(type.GetPosts());

            yield return new WaitForSeconds(1);

            GameControllerScr.loadGame = false;
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("Account not found");
        }
    }

    public IEnumerator PreloadSelfImages(string key)
    {
        Iloading type = new Download_selfAccountFromAPI();

        yield return StartCoroutine(PreloadingManager.instance.LoadingProcess(key, type));

        DataSave.SavePostsInfo(type.GetPosts());

        yield return new WaitForSeconds(1);

        GameControllerScr.loadGame = false;
        SceneManager.LoadScene("Game");
    }

    public IEnumerator LoadFromCache()
    {
        root_posts posts = DataSave.GetpostsData();

        if(posts != null)
        {
            Iloading load = new Download_FromCache();
            yield return StartCoroutine(load.Loading(null));
            root_posts _loadedData = load.GetPosts();
            yield return StartCoroutine(AtlasController.instance.Init(_loadedData));

            GameControllerScr.loadGame = true;
            SceneManager.LoadScene("Game");
        }
    }
    public IEnumerator PreloadHashtagImages(string key)
    {
        Iloading search = new Download_hashtagImages();

        Debug.Log("searching...");
        yield return StartCoroutine(PreloadingManager.instance.LoadingProcess(key, search));
        if (!search.isContainErrors())
        {
            Debug.Log("Starting load account data");

            DataSave.SavePostsInfo(search.GetPosts());

            yield return new WaitForSeconds(1);

            GameControllerScr.loadGame = false;
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("Account not found");
        }
    }
}
