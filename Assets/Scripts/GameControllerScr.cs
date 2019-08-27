using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using genField;

public class GameControllerScr : MonoBehaviour
{
    public int cellCount;

    public List<CellScr> AllCells = new List<CellScr>();

    public Field field;
    public PathParser pathParser;
    public TransformUnity transformUnity;
    public MapGenerator mapGenerator;

    public GameObject cellButton;
    public Transform cellGroup;

    public static bool loadGame { get; set; }
    public static bool refresh { get; set; }

    


    void Start()
    {
        transformUnity = new TransformUnity();
        if(loadGame == false)
        {
            CreateButtonCells();
        }
        else {
            loadMap();
        }
    }

    private void Update()
    {
        if(refresh == true)
        {
            placeCells();
            refresh = false;
        }
        if(pathParser.PathFound == true)
        {
            
            pathParser.parse(field);
            Debug.Log(pathParser.path);
            Debug.Log("Есть путь? " + pathParser.PathExists);
            Debug.Log("Пользователь нашел этот путь? " + pathParser.PathFound);
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
        placeCells();
        pathParser = new PathParser();
        pathParser.parse(field);
        Debug.Log(pathParser.path);
        Debug.Log("Есть путь? "+pathParser.PathExists);
        Debug.Log("Пользователь нашел этот путь? "+pathParser.PathFound);
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
        //field.refreshField(field);
        Debug.Log(field.array);
        placeCells();
    }

}