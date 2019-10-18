﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using genField;
using System.Drawing;
using Image = UnityEngine.UI.Image;

public class ClickButton : MonoBehaviour , IPointerDownHandler

{
    

    private List<Cell> path;

    public float InteractionTime = 1f;
    public Image panel;

    public static List<System.Tuple<int, int>> Buttons;
    public static CellScr first;
    
    public static CellScr second;

    private UnityEngine.Color normCol;

    private (CellScr first, CellScr second) objects;
    private CellScr Click;

    void Start()
    {
        path = new List<Cell>();
        Buttons = new List<System.Tuple<int, int>>();
        panel = GetComponent<Image>();
        Click = gameObject.GetComponent(typeof(CellScr)) as CellScr;

        //StartCoroutine(Example());
    }


    public void OnClick()
    {
        if (GameControllerScr.Interactable)
        {


            //normCol = panel.color;
            //panel.color = UnityEngine.Color.yellow;
            if (Click.settings._randomNum != 0)
            {
                if (first == null)
                {
                    first = Click;
                    GameControllerScr.instance.SetBlinkingImage(first);
                }
                else
                {
                    second = Click;
                }
                var clickedButtons = System.Tuple.Create(Click.settings._id, Click.settings._randomNum);
                Buttons.Add(clickedButtons);
                //Debug.Log(Buttons.Count);
                objects.first = first;
                objects.second = second;

            }
            if (Buttons.Count == 2)
            {

                StartCoroutine(DeleteIcons(Buttons));
            }

        }
    }

    public IEnumerator TouchHold()
    {
        if (Click.settings._randomNum == 0)
            yield break;

        Debug.Log("Touched");
        float _time = 0;

#if UNITY_EDITOR
        while (Input.GetMouseButton(0) && _time < InteractionTime)
        {
            _time += Time.deltaTime;

            yield return null;
        }
        if(Input.GetMouseButton(0))
        {
            GameControllerScr.instance.OpenImagePreview(GetComponent<CellScr>().settings._randomNum);
            //GameControllerScr.instance.OpenEndGamePreview(1);
        }
        else
        {
            OnClick();
        }
#endif
#if UNITY_ANDROID
        if(Input.touchCount > 0)
        {
            while (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary && _time < InteractionTime)
            {
                _time += Time.deltaTime;
                yield return null;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                GameControllerScr.instance.OpenImagePreview(GetComponent<CellScr>().settings._randomNum);
            }
            else
            {
                OnClick();
            }
        }
#endif
    }

    public void SuccessfulPare()
    {
        GameControllerScr.instance.PlayLikeParticles(transform.position);
        GameControllerScr.instance.StopBlinking();

        //points
        GameControllerScr.instance.stats.AddPoints(15);
        GameControllerScr.instance._Timer.AddTime(3);
        
    }

    public IEnumerator DeleteIcons(List<System.Tuple<int, int>> tuples)
    {
        GameControllerScr.Interactable = false;

        int idFirstClick = tuples[0].Item1;
        int rNFirstClick = tuples[0].Item2;

        int idSecondClick = tuples[1].Item1;
        int rNSecondClick = tuples[1].Item2;


        GameControllerScr gameController = GameControllerScr.instance;

        if (rNFirstClick == rNSecondClick && idFirstClick != idSecondClick)
        {

            var firstCoords = gameController.field.findCoordsById(idFirstClick);
            var firstPoint = gameController.field.array[firstCoords.i, firstCoords.j];
            var secondCoords = gameController.field.findCoordsById(idSecondClick);
            var secondPoint = gameController.field.array[secondCoords.i, secondCoords.j];

            bool find = findPath(gameController.field, firstPoint, secondPoint);

            if (find == true)
            {
                gameController.CreateLine(path);

                yield return new WaitForSeconds(gameController.DelayBeforeDestroy);

                first.SetState(0);
                second.SetState(0);

                objects.first.settings._randomNum = 0;
                objects.second.settings._randomNum = 0;

                gameController.field.array[firstCoords.i, firstCoords.j].setState(0);
                gameController.field.array[firstCoords.i, firstCoords.j].setRandomNum(0);

                gameController.field.array[secondCoords.i, secondCoords.j].setState(0);
                gameController.field.array[secondCoords.i, secondCoords.j].setRandomNum(0);
                panel.color = UnityEngine.Color.white * 0.0F;
                gameController.cellState-=2;
                Debug.Log(gameController.cellState);
                //if (gameController.cellState == 0) gameController.endGameFlag = 1;
                if (gameController.cellState == 0)
                {
                    gameController._Timer.AddTime();
                    GameControllerScr.instance.LoadNextLevel();
                    GameControllerScr.instance.OpenEndGamePreview(1);
                }

                Debug.Log("Delete " + idFirstClick + " and " + idSecondClick);

                if (idFirstClick == gameController.firstID
                    || idSecondClick == gameController.secondID
                    || idFirstClick == gameController.secondID
                    || idSecondClick == gameController.firstID)
                {
                    if (gameController.cellState >= 2)
                    {
                        StartCoroutine(GameControllerScr.instance.SearchPath());

                        Debug.Log("#find path");
                    }



                }

                gameController.ResetLine(gameController.LR);

                SuccessfulPare();
                GameControllerScr.instance.Save();

            }

        } //else panel.color = normCol;
        
        else
        {
            Debug.Log("Pare not correct!");
            GameControllerScr.instance.StandartcolorForFirstCell();

        }

        Debug.Log("#ClickButton/OnClick/first:" + Buttons[0]);
        Debug.Log("#ClickButton/OnClick/second:" + Buttons[1]);
        Buttons.Clear();


        first = null;
        second = null;

        GameControllerScr.Interactable = true;

    }

    private bool findPath(Field field, Cell firstClick, Cell secondClick)
    {

        //Point start = new Point(firstClick.i, firstClick.j);
        //Point finish = new Point(secondClick.i, secondClick.j);

        //SettingsField settings = new SettingsField(field);
        //path = settings.LittlePathfinder(start, finish);

        //if (path == null) {return false; }
        //else
        //{
        //    return true;
        //}
        var matrix = PikachuPathfinder.CreateMatrix(field);
        path = PikachuPathfinder.GetWayBetweenTwoCell(matrix, firstClick, secondClick);
        if (path.Count == 0) return false;
        else return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(TouchHold());
    }
}