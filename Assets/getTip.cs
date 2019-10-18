using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getTip : MonoBehaviour
{
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(Tips.GetRandomTip());
        }
    }
}
