using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePreviewer : MonoBehaviour
{
    public Image img;
    public Animator anim;
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
        img.sprite = Resources.Load<Sprite>("imageStandard/file"+id);
    }
    public void Close()
    {
        StartCoroutine(CloseAnim());
    }

    IEnumerator CloseAnim()
    {
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }


    public void setCommentField(int id)
    {
        description.text = AtlasController.instance.posts[id - 1].description;
        username.text = AtlasController.instance.posts[id - 1].usernameFrom;
        likesCount.text = AtlasController.instance.posts[id - 1].likes.ToString() + " отметок \"нравится\"";
    }

    
}
