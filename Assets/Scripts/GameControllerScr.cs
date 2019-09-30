using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using genField;
using System.Drawing;
using System.IO;
using System;
using UnityEngine.Networking;
using Image = UnityEngine.UI.Image;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class GameControllerScr : MonoBehaviour
{

    public bool Helper = false;

    public int cellCount;
    public int cellState;
    public int cellStateTMP;
    public int endGameFlag = 0;
    //private LineRenderer lr;

    //public Material mat;

    public float DelayBeforeDestroy = 0.3f;

    public List<CellScr> AllCells = new List<CellScr>();

    private Camera mainCamera;
    private GameObject[] Line;
    public LineRenderer LR;

    public Field field;
    public PathParser pathParser;
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

    public GridLayoutGroup grid;
    public ParticleSystem LikeSystem;

    public GameUI ui;

    public static bool Interactable = true;
    public UnityEngine.UI.Image blinkImage;

    public GameObject _previewer;
    public GameObject _endGamePreview;

    public string  mapLoad;
    public List<string> descriptions { get; set; }


    [Header("Player stats")]
    public PlayerStats stats;

    private List<string> LEVELS;
    public bool nextLevelFlag = false;

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
        LEVELS = new List<string>();
        LEVELS.Add("map");
        LEVELS.Add("map1");
        LEVELS.Add("map2");
        LEVELS.Add("map3");
        LEVELS.Add("map4");
        LEVELS.Add("map5");
        mapLoad =LEVELS[0];
        numMap = 0;
        cellStateTMP = 0;
        cellState = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        transformUnity = new TransformUnity();
        pathParser = new PathParser();
        mainCamera = Camera.main;
        LR = new LineRenderer();
        Line = GameObject.FindGameObjectsWithTag("Line");


        stats = PlayerStats.instance;
        if(loadGame)
        {
            stats.LoadData();
        }
        else
        {
            stats.LoadData();
            stats.SetPointsTo(0);
        }

        yield return StartCoroutine(AtlasController.instance.Init());


        #region GridLoading
        if (loadGame == false)
        {
           yield return StartCoroutine( CreateButtonCells());
        }
        else {
            if(DataSave.GetData().Item2 != null)
            {
                yield return StartCoroutine(loadMap());
            }
            else
            {
                yield return StartCoroutine( CreateButtonCells());
                _Timer.SetDefaultTime();
            }
        }
        #endregion
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;

        ui.Init();

        if (loadGame)
            _Timer.LoadTime();
        else
            _Timer.SetDefaultTime();


    }

    private void Update()
    {
        if (refresh == true)
        {
            cellStateTMP = cellState;
            placeCells();
            refresh = false;
        }
        if(blinkImage != null)
        {
            blinkImage.color = new UnityEngine.Color(1,1,1, Mathf.Clamp(Mathf.PingPong(Time.time, 1), 0.5f, 1));
        }
        if(endGameFlag == 1)
        {
            Debug.Log("You are won");
            OpenEndGamePreview(1);
            endGameFlag = 0;
        }
        if(endGameFlag == 2)
        {
            Debug.Log("You are loose");
            OpenEndGamePreview(2);
            endGameFlag = 0;
        }
        if(nextLevelFlag == true)
        {
            //Debug.Log(numMap);
            nextLevelFlag = false;
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        if(numMap < 5)
        {
            numMap++;
            
        }
        mapLoad = LEVELS[numMap];
        StartCoroutine(CreateButtonCells());
    }
    
    public void Save()
    {
        DataSave.save(AllCells, (field.heightField, field.widthField));
        _Timer.SaveTime();
    }



    public IEnumerator CreateButtonCells()
    {
        grid.enabled = true;
        mapGenerator = new MapGenerator();
        string filePath = Path.Combine(Application.streamingAssetsPath, "map.txt"); 
#if UNITY_ANDROID
        //Android

        if (Application.platform == RuntimePlatform.Android)
        {
            TextAsset s = (TextAsset)Resources.Load(mapLoad);
            string str = s.text;

            var map = mapGenerator.mapFromFile(str);
            field = mapGenerator.mapFromString(map.map, map.width, map.height);
        }

#endif
#if UNITY_EDITOR
        //Editor
        else
        {

            TextAsset s = (TextAsset)Resources.Load(mapLoad);
            Debug.Log(s.text);

            String str = s.text;


            var map = mapGenerator.mapFromFile(str);

            field = mapGenerator.mapFromString(map.map, map.width, map.height);
        }
#endif


        //field = new Field(20, 13, 20, 4);
        //field.initField(true);
        //field.generateField();
        yield return StartCoroutine( SearchPath());
        placeCells();


        yield return new WaitForEndOfFrame();
        grid.enabled = false;

        SortHierarchy();
        Save();
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
        if(blinkImage)
        {
            StandartcolorForFirstCell();   
        }
        if (cell.settings._state == 1)
        blinkImage = cell.GetComponent<UnityEngine.UI.Image>();
    }
    public void StopBlinking()
    {
        blinkImage = null;
    }

    public void StandartcolorForFirstCell()
    {
        if(blinkImage)
        {
            blinkImage.color = new UnityEngine.Color(1, 1, 1);
            StopBlinking();
        }
    }

    private Image[] helpers = new Image[2];
    public IEnumerator ShowHelp()
    {
        yield return StartCoroutine(SearchPath());

        helpers[0].color = Color.yellow;
        helpers[1].color = Color.yellow;

    }

    public IEnumerator SearchPath()
    {
        List<Transform> forLine = new List<Transform>();
        
        Debug.Log("Ищу путь...");
        if (pathParser.parse(field) < 0) StartCoroutine( Refresh(false));
        else
        {
            if (pathParser.PathExists == true)
            {
                yield return null;

                string IDFirst = "cellButton" + pathParser.path.idFirst;
                string IDSecond = "cellButton" + pathParser.path.idSecond;
                foreach (Transform child in cellGroup)
                {

                    if (child.name == IDFirst)
                    {
                        helpers[0] = child.gameObject.GetComponent<UnityEngine.UI.Image>();
                    }
                    else if (child.name == IDSecond)
                    {
                        helpers[1] = child.gameObject.GetComponent<UnityEngine.UI.Image>();
                    }
                    
                }

                searchPath = false;
                Debug.Log(pathParser.path);
                //CreateLine(pathParser.points);
            }
        }
    }

    private List<Transform> fromPointsToTransform(List<Point> points)
    {
        List<Transform> pathLine = new List<Transform>();

        for(int i = 0; i < points.Count; i++)
        {
            foreach (Transform child in cellGroup)
            {
                string ID = "cellButton" + field.findIdByCoords(points[i].X, points[i].Y);
                if (child.name == ID)
                {
                    pathLine.Add(child);
                }

            }
        }

        return pathLine;
    }


    public IEnumerator loadMap()
    {
        ((int height, int width), List<CellJson> _list) _data = DataSave.GetData();

        field = new Field(_data.Item1.width, _data.Item1.height);
        field.initField(true);

        for (int i = 0; i < _data._list.Count; i++)
        {
            var coords = field.findCoordsById(_data._list[i]._id);
            field.array[coords.i, coords.j].setId(_data._list[i]._id);
            field.array[coords.i, coords.j].setRandomNum(_data._list[i]._randomNum);
            field.array[coords.i, coords.j].setState(_data._list[i]._state);
        }

     

        placeCells();

        yield return new WaitForEndOfFrame();

        grid.enabled = false;
        SortHierarchy();
    }

   
    public void placeCells()
    {
        clearField();


        for (int i = 0; i < cellCount; i++)
        {
            var coords = field.findCoordsById(i + 1);
            GameObject tmpCell = Instantiate(cellButton);
            tmpCell.transform.SetParent(cellGroup, false);
            tmpCell.name = "cellButton" + (i+1);
            tmpCell.GetComponent<CellScr>().settings._id = field.array[coords.i, coords.j].getId();
            tmpCell.GetComponent<CellScr>().settings._randomNum = field.array[coords.i, coords.j].getRandomNum();
            tmpCell.GetComponent<CellScr>().SetState(field.array[coords.i, coords.j].getState());

            if(cellStateTMP == 0)
            {
                if (field.array[coords.i, coords.j].getState() == 1) cellState++;
            }
            else
            {
                cellState = cellStateTMP;
            }
            
            AllCells.Add(tmpCell.GetComponent<CellScr>());
        }
    }

    public void clearField()
    {
        AllCells.Clear();
        foreach (Transform child in cellGroup)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public IEnumerator Refresh(bool UseInstaCoins)
    {

        if (isRefreshing)
            yield break;

        if(UseInstaCoins)
        {
            stats.AddInstaCoins(-stats.RefreshPrice);
        }

        isRefreshing = true;
        int _step = 3;

        StandartcolorForFirstCell();

        foreach (CellScr _cell in AllCells)
        {
            if(_cell.settings._randomNum != 0)
            {
                _cell.Hide();
                _step--;
                if(_step <= 0)
                {
                    yield return new WaitForEndOfFrame();
                    _step = 3;
                }
            }
        }
        yield return new WaitForSeconds(AllCells[0].LerpTime);

        grid.enabled = true;

        field = field.refreshField(field);

        
        refresh = true;
        yield return StartCoroutine( SearchPath());

        yield return new WaitForEndOfFrame();

        grid.enabled = false;
        
        SortHierarchy();

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

    public void CreateLine(List<Point> points)
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
        Debug.Log("clear");
        //yield return new WaitForSeconds(5);
        lr.positionCount = 0;
    }

    #endregion

    public void PlayLikeParticles(Vector3 pos)
    {
        ParticleSystem sys = Instantiate(LikeSystem);
        sys.transform.position = new Vector3(pos.x, pos.y, pos.z - 10);
        sys.Play();
    }

    public void StopLoading()
    {
        AtlasController.instance.StopLoading();
    }


    //public IEnumerator waiter()
    //{
    //    CreateLine(points);
    //    Wait for 4 seconds
    //    Debug.Log("wait");
    //    yield return new WaitForSeconds(5);

    //}
}