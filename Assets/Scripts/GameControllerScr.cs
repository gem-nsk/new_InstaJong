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



public class GameControllerScr : MonoBehaviour
{

    public bool Helper = false;

    public int cellCount;
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

    public GameObject cellButton;
    public Transform cellGroup;

    public static bool loadGame { get; set; }
    public static bool refresh { get; set; }

    public bool searchPath = true;
    public bool isRefreshing = false;

    public GridLayoutGroup grid;
    public ParticleSystem LikeSystem;

    public GameUI ui;

    public static bool Interactable = true;
    public UnityEngine.UI.Image blinkImage;

    public GameObject _previewer;

    public List<string> descriptions { get; set; }


    [Header("Player stats")]
    public PlayerStats stats;

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
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        transformUnity = new TransformUnity();
        pathParser = new PathParser();
        mainCamera = Camera.main;
        LR = new LineRenderer();
        Line = GameObject.FindGameObjectsWithTag("Line");


        stats = PlayerStats.instance;
        stats.LoadData();

        yield return StartCoroutine(AtlasController.instance.Init());

        if (loadGame == false)
        {
           yield return StartCoroutine( CreateButtonCells());
        }
        else {
           yield return StartCoroutine(  loadMap());
        }
        
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;

        ui.Init();

    }

    private void Update()
    {
        if (refresh == true)
        {
            placeCells();
            refresh = false;
        }
        if(blinkImage != null)
        {
            blinkImage.color = new UnityEngine.Color(1,1,1, Mathf.Clamp(Mathf.PingPong(Time.time, 1), 0.5f, 1));
        }
    }
    
    private void OnDestroy()
    {

        DataSave.save(AllCells, (field.heightField, field.widthField));
    }
    


    public IEnumerator CreateButtonCells()
    {
        mapGenerator = new MapGenerator();
        string filePath = Path.Combine(Application.streamingAssetsPath, "map.txt");
#if UNITY_ANDROID
        //Android

        if (Application.platform == RuntimePlatform.Android)
        {
            TextAsset s = (TextAsset)Resources.Load("map");
            string str = s.text;

            var map = mapGenerator.mapFromFile(str);
            field = mapGenerator.mapFromString(map.map, map.width, map.height);
        }

#endif
#if UNITY_EDITOR
        //Editor
        else
        {

            TextAsset s = (TextAsset)Resources.Load("map");
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

                    if (child.name == IDFirst || child.name == IDSecond)
                    {
                        var ChildButton = child.gameObject.GetComponent<UnityEngine.UI.Image>();
                        //helper
                        if(Helper)
                        {
                            ChildButton.color = UnityEngine.Color.yellow;
                        }
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

       // TransformUnity transform = new TransformUnity();

        //ids
        //string path = Application.temporaryCachePath;
        //string[] data = new string[3];

        //UnityWebRequest request = UnityWebRequest.Get(path + "/IDs.txt");
        //yield return request.SendWebRequest();

       // data[0] = request.downloadHandler.text;
        //rnd

       // request = UnityWebRequest.Get(path + "/RandomNums.txt");
        //yield return request.SendWebRequest();

       // data[1] = request.downloadHandler.text;
        //states

       // request = UnityWebRequest.Get(path + "/States.txt");
       // yield return request.SendWebRequest();

        //data[2] = request.downloadHandler.text;

       // foreach(string s in data)
       // {
       //     Debug.Log(s);
       // }

       // field = transform.fromFileToUnity(data[0],data[1],data[2]);

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
        LikeSystem.transform.position = new Vector3(pos.x, pos.y, pos.z - 10);
        LikeSystem.Play();
    }
    
    //public IEnumerator waiter()
    //{
    //    CreateLine(points);
    //    Wait for 4 seconds
    //    Debug.Log("wait");
    //    yield return new WaitForSeconds(5);

    //}
}