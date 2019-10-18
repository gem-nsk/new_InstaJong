using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Assets.Accounts.Convert.preferAccount
{
    [DataContract]
    public class RootObject
    {
        [DataMember(Name = "id")]
        public string id { get; set; }
    }


    public class PreferAccountLoading : MonoBehaviour
    {
        public Text _text;
        public GameObject searchingUi;
        private string _str;
        private bool isLoaded = false;

        private void OnEnable()
        {
            StartCoroutine(Start());
        }

        IEnumerator Start()
        {

            UnityWebRequest request = UnityWebRequest.Get("https://appsbygem.com/actualacc/");
            yield return request.SendWebRequest();

            if(!request.isNetworkError)
            {
                var data = JsonConvert.DeserializeObject<RootObject>(request.downloadHandler.text);
                _str = data.id;
                _text.text = "@" + _str;
                isLoaded = true;
            }
        }

        public void StartPlay()
        {
            if(isLoaded)
            {
                CanvasController.instance.OpenCanvas(searchingUi);
                History.SaveToHistory(_str, 0, 0);
                PreloadingManager.instance._PreloadAccountImages(_str);

            }
        }
    }
}
