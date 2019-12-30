using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Assets.Accounts.Convert.preferAccount;

namespace Assets.Accounts.Convert.preferAccount
{
    [DataContract]
    public class RootObject
    {
        [DataMember(Name = "keys")]
        public List<Key> keys { get; set; }

    }
    [DataContract]
    public class Key
    {
        [DataMember(Name = "id")]
        public string id { get; set; }
        [DataMember(Name = "category")]
        public string category { get; set; }
    }
}


public class PreferAccountLoading : ui_basement
{
    public GameObject searchingUi;

    public GameObject ElementPrefab;
    public RectTransform Conteiner;

    private string _str;
    private bool isLoaded = false;

    public override void Activate()
    {
        base.Activate();
        StartCoroutine(LoadAccounts());
    }

    IEnumerator LoadAccounts()
    {
        UnityWebRequest request;

        switch ( LocalizationManager.instance.GetLanguage())
        {
            case "ru":
                request = UnityWebRequest.Get("https://appsbygem.com/actualacc-ru/");

                break;
            default:
            case "eng":
                request = UnityWebRequest.Get("https://appsbygem.com/actualacc/");
                break;
        }
        yield return request.SendWebRequest();

        //write new Deserialization

        if (!request.isNetworkError)
        {
            var data = JsonConvert.DeserializeObject<RootObject>(request.downloadHandler.text);
            Debug.Log("this is working fine!");

            foreach(var el in data.keys)
            {
                GameObject obj = Instantiate(ElementPrefab, Conteiner.transform);
                obj.GetComponent<PreferAccountElement>().Setup(el.category, el.id);
            }
            Debug.Log("finded " + data.keys.Count + " accounts");
            Conteiner.sizeDelta = new Vector2(Conteiner.sizeDelta.x, 280 * data.keys.Count);
            isLoaded = true;
        }
    }

    public void Play(string id)
    {
        if (isLoaded)
        {
            CanvasController.instance.OpenCanvas(searchingUi);
            //History.SaveToHistory(id, 0, 0);
            PreloadingManager.instance._PreloadAccountImages(id);
            AnalyticsEventsController.LogEvent("GameMode_Prefer");
        }
    }
}
