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
