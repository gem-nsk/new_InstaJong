using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AStarPathfinder;
using genField;
using System.Drawing;

public class ClickButton : MonoBehaviour

{
    int firstClick;
    int secondClick;

    private List<Point> path;

    public Image panel;

    public static List<System.Tuple<int, int>> Buttons;
    public static CellScr first;
    public static CellScr second;

    private UnityEngine.Color normCol;

    private (CellScr first, CellScr second) objects;

    void Start()
    {
        Buttons = new List<System.Tuple<int, int>>();
        panel = GetComponent<Image>();
        //StartCoroutine(Example());
    }  

    void Update()
    {
        
    }

    public void OnClick()
    {
        var Click = gameObject.GetComponent(typeof(CellScr)) as CellScr;
        //normCol = panel.color;
        //panel.color = UnityEngine.Color.yellow;
        if(Click.randomNum != 0)
        {
            if (first == null) first = Click;
            else second = Click;
            var clickedButtons = System.Tuple.Create(Click.id, Click.randomNum);
            Buttons.Add(clickedButtons);
            //Debug.Log(Buttons.Count);
            objects.first = first;
            objects.second = second;

        }
        if (Buttons.Count == 2)
        {
            DeleteIcons(Buttons);
        }

    }

    public int DeleteIcons(List<System.Tuple<int, int>> tuples)
    {
        int idFirstClick = tuples[0].Item1;
        int rNFirstClick = tuples[0].Item2;

        int idSecondClick = tuples[1].Item1;
        int rNSecondClick = tuples[1].Item2;

        
        GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;

        if (rNFirstClick == rNSecondClick && idFirstClick != idSecondClick)
        {

            var firstCoords = gameController.field.findCoordsById(idFirstClick);
            var secondCoords = gameController.field.findCoordsById(idSecondClick);

            bool flg;
            bool find = findPath(gameController.field, firstCoords, secondCoords);
            if (find == false)
            {
                find = findPath(gameController.field, secondCoords, firstCoords);
                if (find == false) flg = false;
                else
                {
                    flg = true;
                }
            }
            else
            {
                flg = true;
            }


            if (flg == true)
            {
                gameController.CreateLine(path);
                
                first.SetState(0);
                second.SetState(0);

                gameController.field.array[firstCoords.i, firstCoords.j].setState(0);
                gameController.field.array[firstCoords.i, firstCoords.j].setRandomNum(0);

                gameController.field.array[secondCoords.i, secondCoords.j].setState(0);
                gameController.field.array[secondCoords.i, secondCoords.j].setRandomNum(0);

                objects.first.randomNum = 0;
                objects.second.randomNum = 0;

                

                panel.color = UnityEngine.Color.white * 0.0F;
                Debug.Log("Delete " + idFirstClick + " and " + idSecondClick);
                if (idFirstClick == gameController.pathParser.path.idFirst
                    || idSecondClick == gameController.pathParser.path.idSecond
                    || idFirstClick == gameController.pathParser.path.idSecond
                    || idSecondClick == gameController.pathParser.path.idFirst)
                {
                    gameController.SearchPath();
                    //Debug.Log("find path");
                }

                //gameController.ResetLine(gameController.LR);

            } 
        } //else panel.color = normCol;

        
        Buttons.Clear();
        first = null;
        second = null;

        return 0;
    }

    private bool findPath(Field field, (int i, int j) firstClick, (int i, int j) secondClick)
    {

        Point start = new Point(firstClick.i, firstClick.j);
        Point finish = new Point(secondClick.i, secondClick.j);

        SettingsField settings = new SettingsField(field, field.widthField, field.heightField);
        path = settings.LittlePathfinder(start, finish);

        if (path == null) {return false; }
        else
        {
            return true;
        }
    }


}