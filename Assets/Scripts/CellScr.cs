using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using Random = System.Random;

[System.Serializable]
public class CellJson
{
    public int _state;
    public int _id;
    public int _randomNum;

    public int _x;
    public int _y;
   
}

public class root
{
    public List<CellJson> data = new List<CellJson>();
    public int width;
    public int height;

    public GameStrategy _strategy;

    public int _scellState;
    public float time;
    public int _Level;
}




public class CellScr : MonoBehaviour
{
    public CellJson settings;

    public Image img;
    public Image bg;

    public float LerpTime = 0;


    public void SetSettings(int s, int i, int r, int x, int y)
    {
        CellJson setIn = new CellJson
        {
            _state = s,
            _id = i,
            _randomNum = r,
            _x = x,
            _y = y
        };

        settings = setIn;
        SetState(setIn._state);
    }

    public void SetSettings(CellJson setIn)
    {
        settings = setIn;
        SetState(setIn._state);
    }
   

    public void SetState(int i)
    {
        settings._state = i;
        if (i == 0)
        {
            bg.color = new Color(1, 1, 1, 0);
            img.color = new Color(1, 1, 1, 0);
            img.sprite = null;
        }

        else if (i == 1)
        {
            //GetComponent<Image>().color = partiesCol;

            

            //old method with Sprite (around 160 batches)

            /*
            String path = "image/file" + randomNum.ToString();
            button.color = new Color32(255, 255, 255, 255);
            button.sprite = Resources.Load<Sprite>(path);
            */

            //new method
            
            if(settings._randomNum != 0 && img.sprite == null)
            {
                Show();
                //Texture2D tex = DownloadManager.instance.GetImageById(settings._randomNum);
                //button.sprite = Sprite.Create(tex, new Rect(0,0,tex.width, tex.height), Vector2.zero); //AtlasController.instance.GetMaterialById(AtlasController.instance.Atlases[0],settings._randomNum);
                img.color = new Color(1, 1, 1, 1);
                img.sprite = DownloadManager.instance.GetImageById(settings._randomNum);
            }
            
        }
    }

    public void RemoveSprite()
    {
        if(settings._randomNum != 0)
        Hide();
        img.sprite = null;
    }

    public void Hide()
    {
        StopCoroutine(CellVisible(false));
        StartCoroutine(CellVisible(true));
    }
    public void Show()
    {
        StopCoroutine(CellVisible(true));
        StartCoroutine(CellVisible(false));
    }

    // true - hided; false - visible
    public IEnumerator CellVisible(bool hided)
    {
        float _time = 0;
        while (_time <= LerpTime)
        {
            img.color = new Color(1, 1, 1, Mathf.Lerp(hided ? 1 : 0, hided ? 0 : 1, _time / LerpTime));
            bg.color = new Color(1, 1, 1, Mathf.Lerp(hided ? 1 : 0, hided ? 0 : 1, _time / LerpTime));
            _time += Time.deltaTime;
            yield return null;
        }
        yield return null;
        img.color = new Color(1, 1, 1, hided ? 0 : 1);
        bg.color = new Color(1, 1, 1, hided ? 0 : 1);
    }

    public int GetRandomNum()
    {
        return settings._randomNum;
    }

}