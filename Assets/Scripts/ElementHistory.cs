using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementHistory : MonoBehaviour
{
    public int id { get; set; }
    public int type { get; set; }
    public string value { get; set; }

    public GameObject searchingUi;

    public void OnClick()
    {
        CanvasController.instance.OpenCanvas(searchingUi);
        switch (type)
        {
            case 0:
                {
                    PreloadingManager.instance._PreloadAccountImages(value);
                    break;
                }
            case 1:
                {
                    PreloadingManager.instance._PreloadHashtagImages(value);
                    break;
                }
        }
        

    }

    public void Delete()
    {

    }
}
