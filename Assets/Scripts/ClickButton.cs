using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour

{
    int firstClick;
    int secondClick;
    public static int r;
    public Image panel;
    public static int x;
    public static int y;
    public static int idx;
    public static int idy;
    static int flag = 0;
    public static List<System.Tuple<int, int>> Buttons;
    public static CellScr first;
    public static CellScr second;

    void Start()
    {
        Buttons = new List<System.Tuple<int, int>>();
        panel = GetComponent<Image>();
    }  

    void Update()
    {
        //if(flag ==1)
        //{
        //    x = 0;
        //    y = 0;
        //    flag++;
        //}
        //Delete();
        //Reset();
        
    }



    public int Reset()
    {
        if (x!=y)
        {
        
            flag--;
            
        }

        return 0;
    }

    public int Delete()
    {
        var C = gameObject.GetComponent(typeof(CellScr)) as CellScr;
        GameControllerScr T = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;
        

        if (x == y && idx != idy)
        {
            if (x == firstClick)
            {
                //var P = gameObject.GetComponent(typeof(ReadXmlData)) as ReadXmlData; 
                //P.SaveArray(idx); 
                //var Z = gameObject.GetComponent(typeof(ReadXmlData)) as ReadXmlData;
                //Z.SaveArray(idy);
                var coords = T.field.findCoordsById(idx);
                T.field.array[coords.i, coords.j].setState(0);
                Debug.Log(T.field.array[coords.i, coords.j].getState());
                panel.color = Color.white * 0.0f;
                C.state = 0;


                return 0;
            }

            else if (y == secondClick)
            {
                //var P = gameObject.GetComponent(typeof(ReadXmlData)) as ReadXmlData;
                //P.SaveArray(idx);
                //var Z = gameObject.GetComponent(typeof(ReadXmlData)) as ReadXmlData;
                //Z.SaveArray(idy);
                var coords = T.field.findCoordsById(idy);
                T.field.array[coords.i, coords.j].setState(0);
                panel.color = Color.white * 0.0f;
                C.state = 0;


                return 0;
            }

            

            return 0;
        }
        return 0;
    }






    public void Foo()
    {

        if (r == 0)
        {

            var A = gameObject.GetComponent(typeof(CellScr)) as CellScr;
            firstClick = A.randomNum;
            x = firstClick;
            Debug.Log("Click1 " + firstClick);
            var V = gameObject.GetComponent(typeof(CellScr)) as CellScr;
            idx = V.id;
            r++;
        }

        else if (firstClick == 0 && r == 1)
        {

            var B = gameObject.GetComponent(typeof(CellScr)) as CellScr;
            secondClick = B.randomNum;
            y = secondClick;
            var M = gameObject.GetComponent(typeof(CellScr)) as CellScr;
            idy = M.id;

            Debug.Log("Click2 " + secondClick);
            //if (x == y && idx != idy)
            //{              
            //    panel.color = Color.white * 0.0f;
            //    var C = gameObject.GetComponent(typeof(CellScr)) as CellScr;
            //    C.state = 0;
            //    if (firstClick == x)
            //    {
            //        panel.color = Color.white * 0.0f;
            //        C.state = 0;

            //    }

            //}
            flag++;
            r--;
        }
    }


    public void OnClick()
    {
        //Foo();
        var Click = gameObject.GetComponent(typeof(CellScr)) as CellScr;
        if (first == null) first = Click;
        else second = Click;
        var clickedButtons = System.Tuple.Create(Click.id, Click.randomNum);
        Buttons.Add(clickedButtons);
        Debug.Log(Buttons.Count);
        if(Buttons.Count == 2)
        {
            DeleteIcons(Buttons);
        }
    }

    public void DeleteIcons(List<System.Tuple<int, int>> tuples)
    {
        int idFirstClick = tuples[0].Item1;
        int rNFirstClick = tuples[0].Item2;

        int idSecondClick = tuples[1].Item1;
        int rNSecondClick = tuples[1].Item2;

        genField.Move move = new genField.Move();
        
        GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;

        if(rNFirstClick == rNSecondClick && idFirstClick != idSecondClick)
        {

            var firstCoords = gameController.field.findCoordsById(idFirstClick);
            var secondCoords = gameController.field.findCoordsById(idSecondClick);

            bool find = move.FindWave(firstCoords.j,firstCoords.i, secondCoords.j, secondCoords.i, gameController.field.array);
            if(find == true)
            {
                first.SetState(0);
                second.SetState(0);

                panel.color = Color.white * 0.0F;
                Debug.Log("Delete " + idFirstClick + " and " + idSecondClick);
            }
        }
        Buttons.Clear();
        first = null;
        second = null;
    }
}