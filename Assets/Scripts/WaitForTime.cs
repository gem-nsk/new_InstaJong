using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTime : MonoBehaviour
{

    public LineRenderer rend;

    void Start()
    {
        StartTimer();
    }
    public void StartTimer()
    {
        StartCoroutine(DestroyTimer());
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(GameControllerScr.instance.DelayBeforeDestroy);
        rend.positionCount = 0;
        Debug.Log("Wait is over");
    }
}
