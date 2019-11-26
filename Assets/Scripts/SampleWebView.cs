/*
 * Copyright (C) 2012 GREE, Inc.
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class SampleWebView : MonoBehaviour
{
    public string Url;
    public GUIText status;
    WebViewObject webViewObject;

    public GameObject BG;

    public string DebugKey;

    public Text loginText;
    public GameObject[] DeleteLoginButton;
    //20021759479.9f7d92e.e1400359759e4f7b9c7bd99e85e102e4 - ы
    //20021759479.9f7d92e.e1400359759e4f7b9c7bd99e85e102e4
    //20021759479.9f7d92e.e4cf6803ec204e899ce887aab2b88cbf


    //1445481979.9f7d92e.b0c1bd961d644d3aa03ec861a3af7974 - ёра
    //1445481979.9f7d92e.b0c1bd961d644d3aa03ec861a3af7974

    //7050105971.9f7d92e.d92bd5e3730d44d8bf8c7aca48e6ed94 - ноготочки

    public void Login()
    {

#if UNITY_EDITOR
        if (PlayerStats.instance.playerSettings.name != "")
        {
            StartCoroutine(ConvertKeyToId(DebugKey));
            Debug.Log("Started auth");
        }
        else
        {
            Debug.Log("Login clear cookies");
            ClearCookies();
            StartCoroutine(ConvertKeyToId(DebugKey));
        }
#elif UNITY_ANDROID || UNITY_IOS
        Debug.Log("Login()");

        if (PlayerStats.instance.playerSettings.name == "")
        {
            StartCoroutine(loggingIn());

            Debug.Log("Auth in account");
        }
        //else
        //{
        //    ClearCookies();
        //}
#endif
    }

    #region handler
    private void Start()
    {
        PlayerStats.AccountKeyHandler += DisplayLogin;

        DisplayLogin();
    }
    private void OnDestroy() 
    {
        PlayerStats.AccountKeyHandler -= DisplayLogin;
    }
    #endregion

    void DisplayLogin()
    {
        if(PlayerStats.instance.playerSettings.name != "")
        {
            loginText.text = "@" + PlayerStats.instance.playerSettings.name;
            foreach(GameObject ob in DeleteLoginButton)
            ob.SetActive(true);
        }
        else
        {
            loginText.text = "connect your instagram!";
            foreach (GameObject ob in DeleteLoginButton)
                ob.SetActive(false);
        }
    }

    public string GetAccessToken(string myUrl)
    {
        string url1 = myUrl;
        string accessToken = "#access_token";
        string error = "? error";

        if (url1.Contains(accessToken))
        {
            var data = url1.Substring(url1.LastIndexOf("=") + 1);
            Debug.Log("Successfuly authorized. token: " + data);
            AnalyticsEventsController.LogEvent("Authorized");
            //save token here
            StartCoroutine(ConvertKeyToId(data));

            return data;
        }
        return null;
    }

    public IEnumerator ConvertKeyToId(string key)
    {
        Debug.Log("Started converting");
        UnityWebRequest IdRequest = UnityWebRequest.Get("https://api.instagram.com/v1/users/self?access_token=" + key);
        yield return IdRequest.SendWebRequest();
        //get account id
        var _accId = JsonConvert.DeserializeObject<Assets.Accounts.Convert.RootObject>(IdRequest.downloadHandler.text);

        Debug.Log("Converting..");

        PlayerStats.instance.playerSettings = (_accId.data.username, key);

        Destroy(webViewObject.gameObject);
        BG.SetActive(false);

        DisplayLogin();
    }

    public void ClearCookies()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID || UNITY_IOS

        Debug.Log("Clear Cookies()");
        BG.SetActive(true);
        if (webViewObject == null)
        {
            webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
            webViewObject.Init(enableWKWebView: true);
        }
        webViewObject.ClearCookies();

        Destroy(webViewObject.gameObject);
        BG.SetActive(false);
#endif

        PlayerStats.instance.playerSettings = ("", "");
    }

    IEnumerator loggingIn()
    {


        Url = "https://www.instagram.com/oauth/authorize/?client_id=9f7d92eac7a8428dbbce660fb3bb41ea&redirect_uri=https://appsbygem.com/authorization/&response_type=token";

        Debug.Log("LoggingIn");

        BG.SetActive(true);

        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init(
            cb: (msg) =>
            {
                Debug.Log(string.Format("CallFromJS[{0}]", msg));
                //status.text = msg;
                //status.GetComponent<Animation>().Play();
            },
            err: (msg) =>
            {
                Debug.Log(string.Format("CallOnError[{0}]", msg));
                //status.text = msg;
                //status.GetComponent<Animation>().Play();
            },
            started: (msg) =>
            {
                Debug.Log(string.Format("CallOnStarted[{0}]", msg));
            },
            ld: (msg) =>
            {
                Debug.Log(string.Format("CallOnLoaded[{0}]", msg));

                GetAccessToken(msg);


#if UNITY_EDITOR_OSX || !UNITY_ANDROID
                // NOTE: depending on the situation, you might prefer
                // the 'iframe' approach.
                // cf. https://github.com/gree/unity-webview/issues/189
#if true
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        window.location = 'unity:' + msg;
                      }
                    }
                  }
                ");
#else
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        var iframe = document.createElement('IFRAME');
                        iframe.setAttribute('src', 'unity:' + msg);
                        document.documentElement.appendChild(iframe);
                        iframe.parentNode.removeChild(iframe);
                        iframe = null;
                      }
                    }
                  }
                ");
#endif
#endif
                webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
            },
            //ua: "custom user agent string",
            enableWKWebView: true);
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        webViewObject.bitmapRefreshCycle = 1;
#endif
        webViewObject.SetMargins(Screen.width / 4, 100, Screen.width / 4, Screen.height / 4);
        webViewObject.SetVisibility(true);

#if !UNITY_WEBPLAYER
        if (Url.StartsWith("http")) {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        } else {
            var exts = new string[]{
                ".jpg",
                ".js",
                ".html"  // should be last
            };
            foreach (var ext in exts) {
                var url = Url.Replace(".html", ext);
                var src = System.IO.Path.Combine(Application.streamingAssetsPath, url);
                var dst = System.IO.Path.Combine(Application.persistentDataPath, url);
                byte[] result = null;
                if (src.Contains("://")) {  // for Android
                    var www = new WWW(src);
                    yield return www;
                    result = www.bytes;
                } else {
                    result = System.IO.File.ReadAllBytes(src);
                }
                System.IO.File.WriteAllBytes(dst, result);
                if (ext == ".html") {
                    webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
                    break;
                }
            }
        }
#else
        if (Url.StartsWith("http")) {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        } else {
            webViewObject.LoadURL("StreamingAssets/" + Url.Replace(" ", "%20"));
        }
        webViewObject.EvaluateJS(
            "parent.$(function() {" +
            "   window.Unity = {" +
            "       call:function(msg) {" +
            "           parent.unityWebView.sendMessage('WebViewObject', msg)" +
            "       }" +
            "   };" +
            "});");
#endif
        yield break;
    }

//#if !UNITY_WEBPLAYER
//    void OnGUI()
//    {
//        GUI.enabled = webViewObject.CanGoBack();
//        if (GUI.Button(new Rect(10, 10, 80, 80), "<")) {
//            webViewObject.GoBack();
//        }
//        GUI.enabled = true;

//        GUI.enabled = webViewObject.CanGoForward();
//        if (GUI.Button(new Rect(100, 10, 80, 80), ">")) {
//            webViewObject.GoForward();
//        }
//        GUI.enabled = true;

//        GUI.TextField(new Rect(200, 10, 300, 80), "" + webViewObject.Progress());

//        if (GUI.Button(new Rect(600, 10, 80, 80), "*")) {
//            var g = GameObject.Find("WebViewObject");
//            if (g != null) {
//                Destroy(g);
//            } else {
//                StartCoroutine(loggingIn());
//            }
//        }
//        GUI.enabled = true;

//        if (GUI.Button(new Rect(700, 10, 80, 80), "c")) {
//            Debug.Log(webViewObject.GetCookies(Url));
//        }
//        GUI.enabled = true;
//    }
//#endif
}
