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


    public void SetSettings()
    {

    }
   

    public void SetState(int i)
    {
        settings._state = i;
        if (i == 0)
        {
            Hide();
            bg.color = new Color(1, 1, 1, 0);
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
                img.sprite = DownloadManager.instance.GetImageById(settings._randomNum);
            }
            
        }
    }

    public void RemoveSprite()
    {
        Image button = GetComponent<Image>();
        button.sprite = null;
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
        //while(_time <= LerpTime)
        //{
        //    img.color = new Color(1, 1, 1, Mathf.Lerp(hided ? 1 : 0, hided ? 0 : 1, _time / LerpTime));
        //    _time += Time.deltaTime;
        //    yield return null;
        //}
        yield return null;
        img.color = new Color(1, 1, 1, hided ? 0 : 1);

    }

    public int GetRandomNum()
    {
        return settings._randomNum;
    }


        //public int GetRandom(int curentNum)
        //{

        //    for (int j = curentNum; j < mas.Length; j++)
        //    {
        //        randomNum = mas[j];

        //        return randomNum;
        //    }

        //    return randomNum;
        //}

    //private void OnPostRender()
    //{
    //    GL.PushMatrix(); // Понятие не имею что это значит
    //    GL.Begin(GL.LINES);// Тоже понятие не имею что это значит, но тут ты можешь выбрать режим рисование, в данном случаи GL.LINES - линия, между каждыми 2 вершинами он будет рисовать линию, можно написать например GL.TRIANGLES - так он будет рисовать между 3 вершинами треугольник.
    //    mat.SetPass(0);// Материал
    //    GL.Vertex(p2.position);// Самое главное, начало линии 1 вершина 
    //    GL.Vertex(p1.position);// Самое главное, конец линии 2 вершина
    //    GL.End();// Понятие не имею что это значит
    //    GL.PopMatrix();// Понятие не имею что это значит
    //}

}