using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using genField;

using Newtonsoft.Json;
using System.IO;
using System.Net;
using System;

using Assets.Scripts;

public class MainMenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinuePressed()
    {
        loadLevel();
    }
    
      
    public void NewGamePressed()
    {

        //DownloadImagesFromInstagram();

        GameControllerScr.loadGame = false;
        SceneManager.LoadScene("Game");
    }

    public void loadLevel()
    {

        //GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;
        //gameController.loadGame = true;
        //gameController.loadMap();
        GameControllerScr.loadGame = true;
        SceneManager.LoadScene("Game");
    }
    
    public void ButtonBack()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Refresh()
    {
        GameControllerScr.instance.StartCoroutine("Refresh");

        /*GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;
        Field field = gameController.field.refreshField(gameController.field);
        gameController.field = field;
        GameControllerScr.refresh = true;*/
    }

    public void DownloadImagesFromInstagram()
    {
        string token = "55595064.dd12fa9.6dc460358d3544e0a1fc2cac28dcff9b";
        WebClient webClient = new WebClient();
        var list = webClient.DownloadString("https://api.instagram.com/v1/users/self/media/recent/?access_token=" + token);
        var dyn = JsonConvert.DeserializeObject<RootObject>(list);
        int i = 1;

        

        foreach (var data in dyn.data)
        {
            string url = data.images.thumbnail.url;
            string urlStandard = data.images.standard_resolution.url;
            string caption_text = data.caption.text;
            Debug.Log(caption_text);
            List<string> descriptions = new List<string>();
            descriptions.Add(caption_text);
            
            using (WebClient client = new WebClient())
            {
                //client.DownloadFileAsync(new Uri(url), @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\image\file"+i+".jpg");
                client.DownloadFileAsync(new Uri(urlStandard), @"D:\workspace\GameDev\new_InstaJong\Assets\Resources\imageStandard\file" + i + ".jpg");
            }
            i++;
        }
    }

    public void Music_button()
    {
        Music.instance.SwitchMusic();
    }

}
