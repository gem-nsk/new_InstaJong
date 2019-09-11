using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePreviewer : MonoBehaviour
{
    public Image img;
    public Animator anim;

    public void Preview(int id)
    {
        LoadPicture(id);
    }

    void LoadPicture(int id)
    {
        Material mat = AtlasController.instance.GetMaterialById(id);
        img.material = mat;
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
}
