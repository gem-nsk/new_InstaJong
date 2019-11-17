using genField;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class GameControllerScr : MonoBehaviour
{
    public bool Helper = false;

    private const int cellCount = 112;
    public int cellState;
    public int cellStateTMP;
    public int endGameFlag = 0;

    public int firstID;
    public int secondID;
    //private LineRenderer lr;

    //public Material mat;

    public float DelayBeforeDestroy = 0.3f;

    public List<CellScr> AllCells = new List<CellScr>();

    private Camera mainCamera;
    private GameObject[] Line;
    public LineRenderer LR;

    public Field field;
    public TransformUnity transformUnity;
    public MapGenerator mapGenerator;
    public Timer _Timer;

    public GameObject cellButton;
    public Transform cellGroup;

    public static bool loadGame { get; set; }
    public static bool refresh { get; set; }
    public static int numMap;

    public bool searchPath = true;
    public bool isRefreshing = false;

   // public GridLayoutGroup grid;
    public ParticleSystem LikeSystem;

    public GameUI ui;

    public static bool Interactable = true;
    public CellScr blinkImage;

    public GameObject _previewer;
    public GameObject _endGamePreview;

    public string mapLoad;
    public List<string> descriptions { get; set; }

    public static GameStrategy gameStrategy;

    private int countPhotos;

    const string nameButtons = "cellButton";

    public GameObject Tutorial;
    public Canvas canvas;


    [Header("Player stats")]
    public PlayerStats stats;

    private List<string> LEVELS;
    public bool nextLevelFlag = false;

    //ForTutorial
    [Header("Tutorial")]
    public GameObject Refresh_t;
    public GameObject Hint_t;
    public GameObject Time_t;
    public GameObject Timer_t;
    public GameObject Score_t;
    public GameObject Pause_t;
    private List<RectTransform> RTs;
    private string[] messages;

    #region Singleton
    public static GameControllerScr instance;
    

    private void Awake()
    {
        instance = this;

       
    }
    #endregion
    #region MainCode
    public IEnumerator Start()
    {
        gameStrategy = GameStrategy.Normal;
        countPhotos = DownloadManager.instance.GetCount();
        numMap = 1;
        cellStateTMP = 0;
        cellState = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        transformUnity = new TransformUnity();
        mainCamera = Camera.main;
        LR = new LineRenderer();
        Line = GameObject.FindGameObjectsWithTag("Line");

        Application.targetFrameRate = 60;


        stats = PlayerStats.instance;
        if (loadGame)
        {
            stats.LoadData();
            StartCoroutine(loadMap());
        }
        else
        {
            stats.LoadData();
            stats.SetPointsTo(0);
        }


        #region GridLoading
        if (loadGame == false)
        {
            yield return StartCoroutine(CreateButtonCells());
        }
        else
        {
            if (DataSave.GetData() != null)
            {
                Debug.Log("load map");
                yield return StartCoroutine(loadMap());
            }
            else
            {
                yield return StartCoroutine(CreateButtonCells());
                _Timer.AddTime();
            }
        }
        #endregion
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;

        ui.Init();

        if (loadGame)
            _Timer.SetTime(DataSave.GetData().time);
        else
            _Timer.StartTimer();

        //Save();
        if(PlayerPrefs.HasKey("_tut2"))
        {
            ui.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            ui.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (refresh == true)
        {
            //cellStateTMP = cellState;
            placeCells();
            refresh = false;
        }
        if (blinkImage != null)
        {
            blinkImage.img.color = new UnityEngine.Color(1, 1, 1, Mathf.Clamp(Mathf.PingPong(Time.time, 1), 0.5f, 1));
        }
        if (endGameFlag == 1)
        {
            Debug.Log("You are won");
            //OpenEndGamePreview(1);
            endGameFlag = 0;
            ////nextLevelFlag = true;
            //LoadNextLevel();
        }
        if (endGameFlag == 2)
        {
            Debug.Log("You are loose");
            OpenEndGamePreview(2);
            endGameFlag = 0;

            ClearSave();

        }
    }

    public void NextLevel()
    {
        if (gameStrategy < GameStrategy.YCenter)
            gameStrategy++;
        else
            gameStrategy = GameStrategy.Normal;
        numMap++;
        ui.UpdateLevel(numMap);
        GameControllerScr.instance._Timer.UpdateTimeValues();
        StartCoroutine(CreateButtonCells());
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            Save();
        }
    }

    public void Save()
    {
        root r = new root
        {
            height = field.heightField,
            width = field.widthField,
            time = _Timer._time,
            _scellState = cellState,
            _strategy = gameStrategy,
            _Level = numMap
        };

        foreach (CellScr scr in AllCells)
        {
            r.data.Add(scr.settings);
        }

        DataSave.save(r);
    }
    public void ClearSave()
    {
        if(File.Exists(Application.persistentDataPath + "/posts.json"))
        {
            File.Delete(Application.persistentDataPath + "/posts.json");
        }
        if(File.Exists(Application.persistentDataPath + "/grid.json"))
        {
            File.Delete(Application.persistentDataPath + "/grid.json");
        }
    }

    private bool isTutorial = false;
    public IEnumerator CreateButtonCells()
    {
        cellState = 0;
        //grid.enabled = true;
        if(countPhotos == 36)
        {
            field = new Field(14, 8, 36, 2);
        }
        else
        {
            field = new Field(14, 8, 18, 4);
        }
        
        field.initField(true);
        field.generateField();

        placeCells();
        yield return StartCoroutine(SearchPath());
        //yield return StartCoroutine(ShowHelp());
        
        yield return new WaitForEndOfFrame();


        if (!PlayerPrefs.HasKey("_tut2"))
        {
            RTs = new List<RectTransform>();
            RTs.Add((RectTransform)Refresh_t.transform);
            RTs.Add((RectTransform)Refresh_t.transform);
            RTs.Add((RectTransform)Hint_t.transform);
            RTs.Add((RectTransform)Time_t.transform);
            RTs.Add((RectTransform)Timer_t.transform);
            RTs.Add((RectTransform)Timer_t.transform);
            RTs.Add((RectTransform)Score_t.transform);
            RTs.Add((RectTransform)Pause_t.transform);

            string[] _messages = {
            "Вы можете нажать на кнопку, чтобы перемешать поле",
            "Поле перемешается автоматически и бесплатно, если не будет хода",

            "Если не видите ход, можете воспользовать подсказкой",

            "Если время кончается, можете добавить время",

            "У вас есть 4 минуты чтобы пройти уровень",
            "За каждую удаленную пару, будет прибавляться время",

            "За найденную пару вам будут начисляться очки",

            "Нажатием на эту кнопку вы поставите игру на паузу и вернетесь в меню"
        };

            messages = _messages;
            CanvasController.instance.OpenCanvas(Tutorial);

            TutorialMenu_ui.instance.Init(RTs, 8, messages, true, canvas, 1);
            isTutorial = true;
            _Timer._isPaused = true;

            PlayerPrefs.SetInt("_tut2", 1);
        }
        
        //grid.enabled = false;

        //SortHierarchy();
        //Save();
    }

    public void SortHierarchy()
    {
        if (AllCells.Count > 0)
        {
            AllCells.Sort(delegate (CellScr a, CellScr b)
            {
                return (a.GetComponent<CellScr>().settings._randomNum).CompareTo(b.GetComponent<CellScr>().settings._randomNum);
            });
        }

        for (int i = 0; i < AllCells.Count; i++)
        {
            AllCells[i].transform.SetSiblingIndex(i);
        }
    }

    public void SetBlinkingImage(CellScr cell)
    {
        if (blinkImage)
        {
            StandartcolorForFirstCell();
        }
        if (cell.settings._state == 1)
            blinkImage = cell.GetComponent<CellScr>();
    }
    public void StopBlinking()
    {
        blinkImage = null;
    }

    public void StandartcolorForFirstCell()
    {
        if (blinkImage)
        {
            blinkImage.img.color = new UnityEngine.Color(1, 1, 1);
            StopBlinking();
        }
    }

    private GameObject[] helpers = new GameObject[2];
    public IEnumerator ShowHelp()
    {
        if (helpers != null)
        {
            if (helpers[0].GetComponent<CellScr>().GetRandomNum() > 0 &&
           helpers[1].GetComponent<CellScr>().GetRandomNum() > 0)
            {
                helpers[0].GetComponent<Image>().color = Color.yellow;
                helpers[1].GetComponent<Image>().color = Color.yellow;
                yield break;
            }
        }
        yield return StartCoroutine(SearchPath());
        helpers[0].GetComponent<Image>().color = Color.yellow;
        helpers[1].GetComponent<Image>().color = Color.yellow;
    }    

    public IEnumerator HighlightHelpers()
    {
        yield return StartCoroutine(SearchPath());

        helpers[0].GetComponent<Image>().color = Color.yellow;
        helpers[1].GetComponent<Image>().color = Color.yellow;

        float _time = 0;
        float _elapsedTime = 1;

        RectTransform[] rects = GetHelpersRect();

        Debug.Log(rects[0].name);
        Debug.Log(rects[1].name);

        while(_time < _elapsedTime)
        {
            if(rects[0])
            rects[0].localScale = Vector3.Lerp(rects[0].localScale, new Vector3(1.2f, 1.2f, 1), _time / _elapsedTime);
            if(rects[1])
            rects[1].localScale = Vector3.Lerp(rects[1].localScale, new Vector3(1.2f, 1.2f, 1), _time / _elapsedTime);
            _time += Time.deltaTime;

            yield return null;
        }
    }

    public GameObject[] GetHelpers()
    {
        GameObject[] obj = new GameObject[helpers.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i] = helpers[i].gameObject;
        }
        return obj;
    }
    public RectTransform[] GetHelpersRect()
    {
        RectTransform[] obj = new RectTransform[helpers.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i] = helpers[i].GetComponent<RectTransform>();
        }
        return obj;
    }

    public IEnumerator SearchPath()
    {
        var matrix = PikachuPathfinder.CreateMatrix(field);
        Dictionary<int, int> currentPairCellIds;
        currentPairCellIds = PikachuPathfinder.GetAvailablePair(matrix,field);
        if (currentPairCellIds.Count == 0)
        {
            StartCoroutine(Refresh(false));
            StartCoroutine(MakeHint("Ходов нет. Перемешиваем поле", 1.5f));
        }
            
        else
        {
            KeyValuePair<int, int> firstPair = currentPairCellIds.FirstOrDefault();
            Debug.Log("#GameController/SearchPath/" + firstPair.Key + " " + firstPair.Value);
            string IDFirst = nameButtons + firstPair.Key;
            string IDSecond = nameButtons + firstPair.Value;

            firstID = firstPair.Key;
            secondID = firstPair.Value;

            foreach (Transform child in cellGroup)
            {
                if (child.name == IDFirst)
                {  

                    helpers[0] = child.gameObject;
                }
                else if (child.name == IDSecond)
                {
                    helpers[1] = child.gameObject;
                }
            }
            searchPath = false;
        }
        
        yield return null;
    }

    private void ShowPath(List<Point> points)
    {
        List<int> IDs = new List<int>();
        foreach (Point point in points)
        {
            IDs.Add(field.findIdByCoords(point.X, point.Y));
        }
        
        foreach(Transform child in cellGroup)
        {
            foreach(int id in IDs)
            {
                string IDFirst = nameButtons + id;
                if (child.name == IDFirst)
                {
                    var chld = child.gameObject.GetComponent<UnityEngine.UI.Image>();
                    chld.color = Color.green;
                }

            }
        }
        
    }

    private List<Transform> fromPointsToTransform(List<Cell> points)
    {
        List<Transform> pathLine = new List<Transform>();

        for (int i = 0; i < points.Count; i++)
        {
            foreach (Transform child in cellGroup)
            {
                string ID = nameButtons + field.findIdByCoords(points[i].getCoords().i, points[i].getCoords().j);
                if (child.name == ID)
                {
                    pathLine.Add(child);
                }

            }
        }

        return pathLine;
    }

    public IEnumerator Strategy(List<int> IDs)
    {
        
        foreach (int id in IDs)
        {
            GameObject child = cellGroup.transform.Find(nameButtons + id).gameObject;
            child.GetComponent<CellScr>().RemoveSprite();
        }
        yield return null;

        placeCells(true);
        StartCoroutine(SearchPath());
    }


    public IEnumerator loadMap()
    {
        root _data = DataSave.GetData();

        numMap = _data._Level;
        gameStrategy = _data._strategy;

        Debug.Log(_data.height + " - " + _data.width + " list " + _data.data.Count);
        field = new Field(_data.height, _data.width);
        field.initField(true);
        Debug.Log(_data.data.Count);    
        for (int i = 0; i < _data.data.Count; i++)
        {
            var coords = field.findCoordsById(_data.data[i]._id);
            field.array[coords.i, coords.j].setId(_data.data[i]._id);
            field.array[coords.i, coords.j].setRandomNum(_data.data[i]._randomNum);
            field.array[coords.i, coords.j].setState(_data.data[i]._state);
        }

        placeCells();

        yield return new WaitForEndOfFrame();

        cellState = _data._scellState;

        //grid.enabled = false;
       // SortHierarchy();
    }


    public void placeCells()
    {
        clearField();

        for (int i = 0; i < cellCount; i++)
        {
            var coords = field.findCoordsById(i + 1);
            GameObject tmpCell = Instantiate(cellButton);
            tmpCell.transform.SetParent(cellGroup, false);
            tmpCell.name = "cellButton" + (i + 1);
            tmpCell.GetComponent<CellScr>().settings._id = field.array[coords.i, coords.j].getId();
            tmpCell.GetComponent<CellScr>().settings._randomNum = field.array[coords.i, coords.j].getRandomNum();
            tmpCell.GetComponent<CellScr>().SetState(field.array[coords.i, coords.j].getState());

            if (cellStateTMP == 0)
            {
                if (field.array[coords.i, coords.j].getState() == 1) cellState++;
            }

            AllCells.Add(tmpCell.GetComponent<CellScr>());
        }
        cellState = 0;
        CalcCellStates();
    }

    void CalcCellStates()
    {
        foreach (CellScr cell in AllCells)
        {
            if (cell.settings._randomNum != 0)
            {
                cellState++;
            }
        }
    }

    public void placeCells(bool flg)
    {
        foreach (Transform child in cellGroup)
        {
            int _id = int.Parse(child.name.Substring(nameButtons.Length));
            var (i, j) = field.findCoordsById(_id);
            CellJson settings = new CellJson
            {
                _state = field.array[i, j].getState(),
                _id = field.array[i, j].getId(),
                _randomNum = field.array[i, j].getRandomNum(),
                _x = field.array[i, j].getCoords().i,
                _y = field.array[i, j].getCoords().j
            };
            child.GetComponent<CellScr>().SetSettings(settings);
            //if (child.GetComponent<CellScr>().settings._randomNum == field.array[i, j].getRandomNum())
            //{
            //    continue;
            //}
            //child.GetComponent<CellScr>().settings.randomNum = field.array[i, j].getRandomNum();
            //child.GetComponent<CellScr>().SetState(field.array[i, j].getState());

        }

    }


    public void clearField()
    {
        AllCells.Clear();
        foreach (Transform child in cellGroup)
        {
            Destroy(child.gameObject);
        }
    }
    
    public IEnumerator Refresh(bool UseInstaCoins)
    {
        Debug.Log("#Refresh");
        if (isRefreshing)
            yield break;

        if (UseInstaCoins)
        {
            stats.AddInstaCoins(-stats.RefreshPrice);
        }
        
        

        isRefreshing = true;
        int _step = 3;

        StandartcolorForFirstCell();

        foreach (CellScr _cell in AllCells)
        {
            if (_cell.settings._randomNum != 0)
            {
                //_cell.Hide();
                _step--;
                if (_step <= 0)
                {
                    _step = 3;
                }
            }
        }
        //yield return new WaitForSeconds(AllCells[0].LerpTime);

        //grid.enabled = true;

        field = field.refreshField(field);

        refresh = true;
        yield return StartCoroutine(SearchPath());

        yield return new WaitForEndOfFrame();

        //grid.enabled = false;

//        SortHierarchy();

        isRefreshing = false;

    }

    public void OpenImagePreview(int id)
    {
        GameObject _obj = CanvasController.instance.OpenCanvas(_previewer);

        _obj.GetComponent<ImagePreviewer>().Preview(id);
    }

    public void OpenEndGamePreview(int state)
    {
        GameObject _obj = CanvasController.instance.OpenCanvas(_endGamePreview);
        _obj.GetComponent<endGamePreviewer>().Preview(state);
    }

    public void CreateLine(List<Cell> points)
    {
        List<Transform> path = fromPointsToTransform(points);
        WaitForTime wait = Line[0].gameObject.GetComponent<WaitForTime>();
        LR = Line[0].gameObject.GetComponent<LineRenderer>();
        wait.StartTimer();
        LR.material.color = UnityEngine.Color.red;
        LR.sortingOrder = 4;
        LR.sortingLayerName = "Cell";
        LR.useWorldSpace = true;
        LR.startWidth = 0.1f;
        LR.endWidth = 0.1f;

        if (path.Count != 0)
        {
            // Set some positions
            Vector3[] positions = new Vector3[path.Count];
            LR.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                var pointPath = new Vector3(path[i].position.x, path[i].position.y, 100);
                positions[i] = pointPath;

                //Debug.Log(path[i].position);
            }


            LR.SetPositions(positions);



            //ResetLine(LR);
            //waiter();
        }

    }



    public void ResetLine(LineRenderer lr)
    {

        //yield return new WaitForSeconds(5);
        lr.positionCount = 0;
    }

    #endregion

    //public void PlayLikeParticles(Vector3 pos)
    //{
    //    ParticleSystem sys = Instantiate(LikeSystem);
    //    sys.transform.position = new Vector3(pos.x, pos.y, pos.z - 10);
    //    sys.Play();
    //}

    public void StopLoading()
    {
        AtlasController.instance.StopLoading();
    }

    #region DownHint
    //public IEnumerator waiter()
    //{
    //    CreateLine(points);
    //    Wait for 4 seconds
    //    Debug.Log("wait");
    //    yield return new WaitForSeconds(5);

    //}
    public GameObject hintGame;
    public Transform HintOffset;
    private string hintName = "gameHint";
    public Transform BG;
    private Vector2? pos = null;
    private GameObject _CurrentHint;
    public IEnumerator MakeHint(string message)
    {
        
        _CurrentHint = Instantiate(hintGame);
        _CurrentHint.transform.SetParent(BG, false);
        _CurrentHint.name = hintName;
        _CurrentHint.GetComponentInChildren<Text>().text = message;
        
            _CurrentHint.transform.position = HintOffset.position;
            pos = _CurrentHint.transform.position;
            Debug.Log("position: "+ Screen.width / 2);
            
      
        yield return StartCoroutine(AnimatedHintShow(_CurrentHint.GetComponent<RectTransform>()));



    }

    public IEnumerator MakeHint(string message, float delay)
    {

        _CurrentHint = Instantiate(hintGame);
        _CurrentHint.transform.SetParent(BG, false);
        _CurrentHint.name = hintName;
        _CurrentHint.GetComponentInChildren<Text>().text = message;

        _CurrentHint.transform.position = HintOffset.position;
        pos = _CurrentHint.transform.position;
        Debug.Log("position: " + Screen.width / 2);


        yield return StartCoroutine(AnimatedHintShow(_CurrentHint.GetComponent<RectTransform>()));
        yield return new WaitForSeconds(delay);

        Destroy(_CurrentHint);



    }

    public IEnumerator AnimatedHintShow(RectTransform _transform)
    {
        float _time = 0;
        float _elapsedTime = 0.5f;
        _transform.localScale = new Vector2(_transform.localScale.x, 0);

        while(_time < _elapsedTime)
        {
            _transform.localScale = Vector2.Lerp(_transform.localScale, new Vector2(1, 1), _time / _elapsedTime);

            _time += Time.deltaTime;
            yield return null;
        }
    }


    private string[] hints =
    {
        "Нажмите на 2 подсвеченные картинки",
        "Между двумя картинками рисуется линия. У этой линии не должно быть более двух поворотов.",
        "При долгом нажатии на картинку откроется ее описание, попробуйте",
        "Приятной игры!",
    };

    private int hint_id = 0;

    public void StartTutorial()
    {
        StartCoroutine(MakeHint(hints[hint_id],5));
        StartCoroutine(ShowHelp());
        
    }

    public IEnumerator NextHint()
    {
        Destroy(_CurrentHint);
        if (isTutorial)
        {
            _Timer._isPaused = false;
            hint_id++;
            switch (hint_id)
            {
                case 1:
                    {
                        StartCoroutine(MakeHint(hints[hint_id]));
                        StartCoroutine(ShowHelp());
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(MakeHint(hints[hint_id]));
                        break;
                    }
                case 3:
                    {
                        StartCoroutine(MakeHint(hints[hint_id], 1.5f));
                        break;
                    }
            }
        }
        yield return null;
    }
    #endregion
}