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
    public Text likesCount;
    public Text timeStamp;

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
        Texture2D spr = AtlasController.instance.posts[id].StandartTexture;

        img.material = AtlasController.instance.GetMaterialById(AtlasController.instance.Atlases[1], id);

        //_ImgSize.sizeDelta = new Vector2(spr.textureRect.width, spr.textureRect.height);
    }
    public override void DeActivate()
    {
        base.DeActivate();
    }
    
    
    public void setDescriprion(int id)
    {
        Debug.Log(id);
        description.text = AtlasController.instance.posts[id-1].description;
    }

    public void setCommentField(int id)
    {
        description.text = AtlasController.instance.posts[id - 1].description;
        username.text = AtlasController.instance.posts[id - 1].usernameFrom;
        likesCount.text = AtlasController.instance.posts[id - 1].likes.ToString() + " отметок \"нравится\"";
    }

    
}
