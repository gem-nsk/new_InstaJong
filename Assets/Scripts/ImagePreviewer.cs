using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePreviewer : ui_basement
{
    public Image img;
    public RectTransform _ImgSize;
    public Text description;
    public Text username;
    public Text LikeCount;
    public string PostId;

    public override void Activate()
    {
        base.Activate();
        GameControllerScr.instance._Timer.TimerState(true);
    }

    public override void DeActivate()
    {
        GameControllerScr.instance._Timer.TimerState(false);
        base.DeActivate();
    }
    public void Preview(int id)
    {
        LoadPicture(id);
        setCommentField(id);
    }

    void LoadPicture(int id)
    { 
        //Material mat = AtlasController.instance.GetMaterialById(id);

        //img.material = mat;
        //Material mat = new Material(fileStandard);
        //Debug.Log(id);
       // Texture2D spr = AtlasController.instance.posts[id -1].StandartTexture;

        Material mat = AtlasController.instance.GetMaterialById(AtlasController.instance.Atlases[1], id);
        float sizeX = mat.mainTextureScale.x / mat.mainTextureScale.y;
        float sizeY = mat.mainTextureScale.y / mat.mainTextureScale.x;

        float _s = sizeY / sizeX;

        Debug.Log("x: " + sizeX + " y: " + sizeY+ " _s: " + _s);

        Texture2D tex = DownloadManager.instance.GetImageById(id);

        img.sprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), Vector2.zero);
        _ImgSize.sizeDelta *= new Vector2(sizeX, sizeY);
        if (_ImgSize.sizeDelta.x > 900)
        {
            float diff = 900 / _ImgSize.sizeDelta.x;
            _ImgSize.sizeDelta *= new Vector2(diff, diff * 1.5f);
        }
        else if (_ImgSize.sizeDelta.y > 900)
        {
            float diff = 900 / _ImgSize.sizeDelta.y;
            _ImgSize.sizeDelta *= new Vector2(diff * sizeX * 1.5f, diff);
        }
    }

    public void CLose()
    {
        CanvasController.instance.CloseCanvas();
    }
    
    
    public void setDescriprion(int id)
    {
        Debug.Log(id);
        description.text = DownloadManager.instance._tempPosts._p[id-1].description;
    }

    public void setCommentField(int id)
    {
        description.text = DownloadManager.instance._tempPosts._p[id - 1].description;
        username.text = DownloadManager.instance._tempPosts._p[id - 1].usernameFrom;
        LikeCount.text = DownloadManager.instance._tempPosts._p[id - 1].likes.ToString() + " отметок \"нравится\"";
        PostId = DownloadManager.instance._tempPosts._p[id - 1].postLink;
    }

    private void OpenInInstagram()
    {
        string url = "https://www.instagram.com/p/" + PostId;
        Application.OpenURL(url);
    }
}
