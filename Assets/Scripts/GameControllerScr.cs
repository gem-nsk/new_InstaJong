using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using genField;
using System.Drawing;

public class GameControllerScr : MonoBehaviour
{
    public int cellCount;
    //private LineRenderer lr;

    //public Material mat;

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


    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        transformUnity = new TransformUnity();
        pathParser = new PathParser();
        mainCamera = Camera.main;
        LR = new LineRenderer();
        Line = GameObject.FindGameObjectsWithTag("Line");
        if (loadGame == false)
        {
            CreateButtonCells();
        }
        else {
            loadMap();
        }
        
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
    }

    private void Update()
    {
        if (refresh == true)
        {
            placeCells();
            refresh = false;
        }
    }

    private void OnDestroy()
    {
        transformUnity.fromUnityToFile(field);
    }

    public void CreateButtonCells()
    {
        //mapGenerator = new MapGenerator();
        //var path = Application.dataPath + "/Resources/";
        //var map = mapGenerator.mapFromFile(path + "map.txt");
        //field = mapGenerator.mapFromString(map.map,map.width,map.height);

        field = new Field(20, 13, 36, 4);
        field.initField(true);
        field.generateField();
        SearchPath();
        placeCells();
        
    }

    public void SearchPath()
    {
        List<Transform> forLine = new List<Transform>();
        
        Debug.Log("Ищу путь...");
        if (pathParser.parse(field) < 0) Refresh();
        else
        {
            if (pathParser.PathExists == true)
            {
                

                string IDFirst = "cellButton" + pathParser.path.idFirst;
                string IDSecond = "cellButton" + pathParser.path.idSecond;
                foreach (Transform child in cellGroup)
                {

                    if (child.name == IDFirst || child.name == IDSecond)
                    {
                        var ChildButton = child.gameObject.GetComponent<Image>();
                        //ChildButton.color = UnityEngine.Color.yellow;
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

    public void loadMap()
    {
        TransformUnity transform = new TransformUnity();
        field  = transform.fromFileToUnity();
        placeCells();
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
            tmpCell.GetComponent<CellScr>().id = field.array[coords.i, coords.j].getId();
            tmpCell.GetComponent<CellScr>().randomNum = field.array[coords.i, coords.j].getRandomNum();
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

    public void Refresh()
    {
        field = field.refreshField(field);
        refresh = true;
        SearchPath();
    }


    public void CreateLine(List<Point> points)
    {
        List<Transform> path = fromPointsToTransform(points);
        WaitForTime wait = Line[0].gameObject.GetComponent<WaitForTime>();
        LR = Line[0].gameObject.GetComponent<LineRenderer>();
        wait.Test();
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

    //public IEnumerator waiter()
    //{
    //    CreateLine(points);
    //    Wait for 4 seconds
    //    Debug.Log("wait");
    //    yield return new WaitForSeconds(5);

    //}

    

}