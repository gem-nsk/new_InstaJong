﻿using System.Collections;
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

    public float time;
}




public class CellScr : MonoBehaviour
{
    public CellJson settings;

    public Image img;

    public float LerpTime = 1;


    public void SetSettings()
    {

    }
   

    public void SetState(int i)
    {
        settings._state = i;
        if (i == 0)
        {
            Hide();
        }

        else if (i == 1)
        {
            //GetComponent<Image>().color = partiesCol;

            Image button = GetComponent<Image>();

            //old method with Sprite (around 160 batches)

            /*
            String path = "image/file" + randomNum.ToString();
            button.color = new Color32(255, 255, 255, 255);
            button.sprite = Resources.Load<Sprite>(path);
            */

            //new method
            if(settings._randomNum != 0)
            {
                Show();
                button.material = AtlasController.instance.GetMaterialById(AtlasController.instance.Atlases[0],settings._randomNum);
            }
            
        }
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
        while(_time <= LerpTime)
        {
            img.color = new Color(1, 1, 1, Mathf.Lerp(hided ? 1 : 0, hided ? 0 : 1, _time / LerpTime));
            _time += Time.deltaTime;
            yield return null;
        }
        img.color = new Color(1, 1, 1, hided ? 0 : 1);
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