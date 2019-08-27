using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScr : MonoBehaviour
{
    public int cellCount;

    public List<CellScr> AllCells = new List<CellScr>();

    public genField.Field field;
    public genField.PathParser pathParser;
    public genField.TransformUnity transformUnity;

    public GameObject cellButton;
    public Transform cellGroup;

    public static bool loadGame { get; set; }
    public static bool refresh { get; set; }


    void Start()
    {
        transformUnity = new genField.TransformUnity();
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
            Debug.Log("Enter");
        }
    }

    private void OnDestroy()
    {
        transformUnity.fromUnityToFile(field);
    }


    public void CreateButtonCells()
    {
        field = new genField.Field(20, 13, 36, 4);
        field.initField(true);
        field.generateField();
        placeCells();
        Debug.Log("Enter CreateButtonCells");
    }

    public void loadMap()
    {
        genField.TransformUnity transform = new genField.TransformUnity();
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