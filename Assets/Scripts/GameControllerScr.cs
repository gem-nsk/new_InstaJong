using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using genField;

public class GameControllerScr : MonoBehaviour
{
    public int cellCount;
    //private LineRenderer lr;

    //public Material mat;

    public List<CellScr> AllCells = new List<CellScr>();

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
        transformUnity = new TransformUnity();
        pathParser = new PathParser();
        if (loadGame == false)
        {
            CreateButtonCells();
        }
        else {
            loadMap();
        }
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
        mapGenerator = new MapGenerator();
        var path = Application.dataPath + "/Resources/";
        Debug.Log(path);
        var map = mapGenerator.mapFromFile(path + "map.txt");
        field = mapGenerator.mapFromString(map.map,map.width,map.height);

        //field = new Field(20, 13, 36, 4);
        //field.initField(true);
        //field.generateField();
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
                        ChildButton.color = UnityEngine.Color.yellow;
                        forLine.Add(child);
                    }
                }

                searchPath = false;
                Debug.Log(pathParser.path);
            }
        }
        Debug.Log(forLine.Count);
        if (forLine.Count == 2)
        {
            CreateLine(forLine[0], forLine[1]);
            CreateLine(forLine[1], forLine[0]);
            forLine.Clear();
        }
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


    private void CreateLine(Transform p1, Transform p2)
    {
        var lr = p1.gameObject.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = Color.red;
        lr.sortingOrder = 4;
        lr.sortingLayerName = "UI";
        lr.useWorldSpace = false;
        lr.SetWidth(0.5f, 0.5f);
        // Set some positions
        Vector3[] positions = new Vector3[2];
        positions[0] = p1.position;
        positions[1] = p2.position;
        //lr.positionCount = positions.Length;
        lr.SetPositions(positions);
    }

}