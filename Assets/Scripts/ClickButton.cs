using Finder;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        genField.Move move = new genField.Move();
        
        GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;

        if(rNFirstClick == rNSecondClick && idFirstClick != idSecondClick)
        {

            var firstCoords = gameController.field.findCoordsById(idFirstClick);
            var secondCoords = gameController.field.findCoordsById(idSecondClick);

            //bool find = move.FindWave(firstCoords.j,firstCoords.i, secondCoords.j, secondCoords.i, gameController.field.array);
            int[,] intArray = gameController.field.toIntArray(0);
            bool find = findPath(intArray, firstCoords, secondCoords);
            if(find == true)
            {
                first.SetState(0);
                second.SetState(0);

                gameController.field.array[firstCoords.i, firstCoords.j].setState(0);
                gameController.field.array[firstCoords.i, firstCoords.j].setRandomNum(0);

                gameController.field.array[secondCoords.i, secondCoords.j].setState(0);
                gameController.field.array[secondCoords.i, secondCoords.j].setRandomNum(0);

                panel.color = Color.white * 0.0F;
                Debug.Log("Delete " + idFirstClick + " and " + idSecondClick);
                if (idFirstClick == gameController.pathParser.path.idFirst
                    && idSecondClick == gameController.pathParser.path.idSecond)
                {
                    gameController.searchPath = true;
                }
            }
        }
        Buttons.Clear();
        first = null;
        second = null;

        return 0;
    }

    private bool findPath(int [,] intArray, (int i, int j) firstClick, (int i, int j) secondClick)
    {
        intArray[firstClick.i, firstClick.j] = (int)Figures.StartPosition;
        intArray[secondClick.i, secondClick.j] = (int)Figures.Destination;
        //Print(my);

        var li = new LeeAlgorithm(intArray);
        //Console.WriteLine(li.PathFound);
        if (li.PathFound)
        {
            foreach (var item in li.Path)
            {
                if (item == li.Path.Last())
                    intArray[item.Item1, item.Item2] = (int)Figures.StartPosition;
                else if (item == li.Path.First())
                    intArray[item.Item1, item.Item2] = (int)Figures.Destination;
                else
                    intArray[item.Item1, item.Item2] = (int)Figures.Path;
            }
            //Print(li.ArrayGraph);
            //Console.WriteLine("Длина " + li.LengthPath);
            Debug.Log("Путь найден");
            return true;
        }
        else
            Debug.Log("Путь не найден");
        return false;
    }
    
}