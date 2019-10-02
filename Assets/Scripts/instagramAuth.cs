using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using System.Web;

public class User
{
    public string id { get; set; }
    public string username { get; set; }
    public string profile_picture { get; set; }
    public string full_name { get; set; }
    public string bio { get; set; }
    public string website { get; set; }
    public bool is_business { get; set; }
}

public class RootObjectUser
{
    public string access_token { get; set; }
    public User user { get; set; }
}

class MyWebClient : WebClient
{
    Uri _responseUri;

    public Uri ResponseUri
    {
        get { return _responseUri; }
    }

    protected override WebResponse GetWebResponse(WebRequest request)
    {
        WebResponse response = base.GetWebResponse(request);
        _responseUri = response.ResponseUri;
        return response;
    }
}

public class instagramAuth : MonoBehaviour
{
    // Start is called before the first frame update

    public string instagramClientId;
    public string instagramClientSecret;
    private string instagramTokenUrl;
    public string instagramRedirectUrl;
    private string instagramAccessToken;
<<<<<<< HEAD
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

=======
    #region NewTest
    public WebViewObject webViewObject;
    public GameObject webViewUi;

    public SampleWebView webView;
    public bool authComplete;

    public static string url;


    public void OpenWebView()
    {
        webViewUi.SetActive(true);
    }
    public void GetAccessToken(string myUrl)
    {
        string url1 = myUrl;
        string accessToken = "#access_token";
        string error = "? error";

        if (url1.Contains(accessToken))
        {
            var data = url1.Substring(url1.LastIndexOf("=") + 1);

        }
    }

    #endregion


>>>>>>> whynotworking
    public void OnClick()
    {
        string result = LoginAsync();
        var UserToken = JsonConvert.DeserializeObject<RootObjectUser>(result);
        Debug.Log(UserToken.access_token);
    }

    public string LoginAsync()
    {

        //string instagramTokenUrl = "https://api.instagram.com/oauth/authorize/?client_id="
        //    + instagramClientId + "&redirect_uri=" 
        //    + instagramRedirectUrl + "&response_type=code";

        string code_token = "https://api.instagram.com/oauth/authorize/?client_id=9f7d92eac7a8428dbbce660fb3bb41ea&redirect_uri=http://localhost/&response_type=code";


        MyWebClient my = new MyWebClient();
        
        var result_code = my.DownloadString(code_token);
        var res = my.ResponseUri;

        Debug.Log(res);

        try
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("client_id", instagramClientId);
            parameters.Add("client_secret", instagramClientSecret);
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("redirect_uri", instagramRedirectUrl);
            parameters.Add("code", "e11f1369897848c099924d6536824e8f");

            WebClient client = new WebClient();
            var result = client.UploadValues("https://api.instagram.com/oauth/access_token", "POST", parameters);

            return Encoding.Default.GetString(result);
        }
        catch (WebException ex)
        {
            StreamReader reader = new StreamReader(ex.Response.GetResponseStream());
            string line;
            StringBuilder result = new StringBuilder();
            while ((line = reader.ReadLine()) != null)
            {
                result.Append(line);
            }
            return result.ToString();
        }
    }



   
}
