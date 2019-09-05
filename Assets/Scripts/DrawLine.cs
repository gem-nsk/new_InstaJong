using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = Color.red;
        lr.sortingOrder = 4;
        lr.sortingLayerName = "UI";
        lr.useWorldSpace = true;
        lr.SetWidth(0.1f, 0.1f);
        //Set some positions
       Vector3[] positions = new Vector3[3];
        positions[0] = new Vector3(-2.0f, -2.0f, 0.0f);
        positions[1] = new Vector3(0.0f, 2.0f, 0.0f);
        positions[2] = new Vector3(2.0f, -2.0f, 0.0f);
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);
    }


}
