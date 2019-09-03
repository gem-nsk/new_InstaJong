using Finder;
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

    public Image panel;

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
        
    }

    public void OnClick()
    {
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

    public int DeleteIcons(List<System.Tuple<int, int>> tuples)
    {
        int idFirstClick = tuples[0].Item1;
        int rNFirstClick = tuples[0].Item2;

        int idSecondClick = tuples[1].Item1;
        int rNSecondClick = tuples[1].Item2;

        //genField.Move move = new genField.Move();
        
        GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;

        if(rNFirstClick == rNSecondClick && idFirstClick != idSecondClick)
        {

            var firstCoords = gameController.field.findCoordsById(idFirstClick);
            var secondCoords = gameController.field.findCoordsById(idSecondClick);
            int[,] intArray = gameController.field.toIntArray(0);
            //bool find = move.FindWave(firstCoords.j,firstCoords.i, secondCoords.j, secondCoords.i, gameController.field.array);

            //bool flg = false;

            //if(find == false)
            //{
            //    intArray = gameController.field.toIntArray(0);
            //    find = findPath(intArray, secondCoords, firstCoords);
            //    if (find == false) flg = false;
            //    else
            //    {
            //        flg = true;
            //    }
            //}
            //else
            //{
            //    flg = true;
            //}
            bool find = findPath(gameController.field, firstCoords, secondCoords);
            if (find == true)
            {
                first.SetState(0);
                second.SetState(0);

                gameController.field.array[firstCoords.i, firstCoords.j].setState(0);
                gameController.field.array[firstCoords.i, firstCoords.j].setRandomNum(0);

                gameController.field.array[secondCoords.i, secondCoords.j].setState(0);
                gameController.field.array[secondCoords.i, secondCoords.j].setRandomNum(0);

                panel.color = UnityEngine.Color.white * 0.0F;
                Debug.Log("Delete " + idFirstClick + " and " + idSecondClick);
                //if (idFirstClick == gameController.pathParser.path.idFirst
                //    && idSecondClick == gameController.pathParser.path.idSecond)
                //{
                //    gameController.searchPath = true;
                //}
            }
        }
        Buttons.Clear();
        first = null;
        second = null;

        return 0;
    }

    private bool findPath(Field field, (int i, int j) firstClick, (int i, int j) secondClick)
    {
        //intArray[firstClick.i, firstClick.j] = (int)Figures.StartPosition;
        //intArray[secondClick.i, secondClick.j] = (int)Figures.Destination;
        //Print(my);

        //var li = new LeeAlgorithm(intArray);
        //Console.WriteLine(li.PathFound);

        Point start = new Point(firstClick.i, firstClick.j);
        Point finish = new Point(secondClick.i, secondClick.j);

        SettingsField settings = new SettingsField(field, field.widthField, field.heightField);
        var path = settings.LittlePathfinder(start, finish);

        if (path == null) { Debug.Log("Нет пути"); return false; }
        else
        {
            Debug.Log("Есть путь");
            //settings.PrintField(1);
            for (int i = 0; i < path.Count(); i++)
            {
                Debug.Log(path[i]);
            }
            return true;
        }
    }
    
}