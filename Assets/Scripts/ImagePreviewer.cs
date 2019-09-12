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

    public void Preview(int id)
    {
        LoadPicture(id);
        setDescriprion(id);
        setUsername(id);
    }

    void LoadPicture(int id)
    {
        //Material mat = AtlasController.instance.GetMaterialById(id);

        //img.material = mat;
        //Material mat = new Material(fileStandard);
        //Debug.Log(id);
        Sprite spr = Resources.Load<Sprite>("imageStandard/file" + id);


        img.sprite = spr;
        _ImgSize.sizeDelta = new Vector2(spr.textureRect.width, spr.textureRect.height);
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

    public void setUsername(int id)
    {
        username.text = AtlasController.instance.posts[id - 1].usernameFrom;
    }
}
