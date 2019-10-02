using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public ui_basement _currentCanvas;

    public GameObject[] canvasList;

    public static CanvasController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject OpenCanvas(GameObject _canvas)
    {
        if (_currentCanvas)
            _currentCanvas.DeActivate();

        _currentCanvas = Instantiate(_canvas).GetComponent<ui_basement>();
        _currentCanvas.Activate();

        return _currentCanvas.gameObject;
    }
    public GameObject OpenCanvas(int id)
    {
        if (_currentCanvas)
            _currentCanvas.DeActivate();

        _currentCanvas = Instantiate(canvasList[id]).GetComponent<ui_basement>();
        _currentCanvas.Activate();

        return _currentCanvas.gameObject;
    }
    public void CloseCanvas()
    {
        _currentCanvas.DeActivate();
        _currentCanvas = null;
    }
    public void CloseCanvas(ui_basement _canvas)
    {
        _canvas.DeActivate();
    }
}
