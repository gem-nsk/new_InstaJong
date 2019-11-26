using System.Collections;
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

                GameControllerScr.ButtonTouchDelegateHandler?.Invoke();

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
            GameControllerScr.ButtonHoldDelegateHandler?.Invoke();
        }
        else
        {
            OnClick();
        }
#endif
#if UNITY_ANDROID || UNITY_IOS
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
                GameControllerScr.ButtonHoldDelegateHandler?.Invoke();
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
        //GameControllerScr.instance.PlayLikeParticles(transform.position);
        GameControllerScr.instance.StopBlinking();

        GameControllerScr.PareDelegateHandler?.Invoke();

        //if(step < 2) {
        //    StartCoroutine(GameControllerScr.instance.NextHint());
        //    step++;
        //    GameControllerScr.currentStep = step;
        //}
        
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

                DoPair(firstCoords, secondCoords);

                //first.SetState(0, true);
                //second.SetState(0, true);

                //objects.first.settings._randomNum = 0;
                //objects.second.settings._randomNum = 0;

                
                panel.color = UnityEngine.Color.white * 0.0F;

                gameController.cellState -= 2;
                //Debug.Log("#cellState: "+gameController.cellState);
                //if (gameController.cellState == 0) gameController.endGameFlag = 1;
                if (gameController.cellState == 0)
                {
                    gameController._Timer.AddTime();
                    gameController.NextLevel();
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
               

            }
            else
            {
                StartCoroutine(GameControllerScr.instance.MakeHint("_t_game_more", 0.5f));
            }

        } //else panel.color = normCol;
        
        else
        {
            Debug.Log("Pare not correct!");
            StartCoroutine(GameControllerScr.instance.MakeHint("_t_game_different", 0.5f));
            
            GameControllerScr.instance.StandartcolorForFirstCell();

        }

        //Debug.Log("#ClickButton/OnClick/first:" + Buttons[0]);
        //Debug.Log("#ClickButton/OnClick/second:" + Buttons[1]);
        Buttons.Clear();


        first = null;
        second = null;

        GameControllerScr.Interactable = true;

    }

    private bool findPath(Field field, Cell firstClick, Cell secondClick)
    {

        var matrix = PikachuPathfinder.CreateMatrix(field);
        path = PikachuPathfinder.GetWayBetweenTwoCell(matrix, firstClick, secondClick);
        if (path.Count == 0) return false;
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(TouchHold());
    }

    public void DoPair((int row, int col) cell1, (int row, int col) cell2)
    {
        Debug.Log("Enter DoPair");
        var array = GameControllerScr.instance.field.array;
        //var IDs = GameControllerScr.instance.field.DoPair(cell1, cell2, array);
        var strategy = StrategyFactory.CreateInstance(GameControllerScr.gameStrategy);
        var IDs = strategy.DoPair(cell1,cell2,array);
        StartCoroutine(GameControllerScr.instance.Strategy(IDs));
        
    }

    
}