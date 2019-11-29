using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            string date = System.DateTime.Now.ToString();
            date = date.Replace("/", "-");
            date = date.Replace(" ", "_");
            date = date.Replace(":", "-");
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/" + date + ".png");
            Debug.Log(Application.persistentDataPath);
        }
    }
}
