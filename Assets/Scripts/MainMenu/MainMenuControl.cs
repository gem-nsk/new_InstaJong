using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using genField;

public class MainMenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinuePressed()
    {

    }
    
      
    public void NewGamePressed()
    {
        GameControllerScr.loadGame = false;
        SceneManager.LoadScene("Game");
       
       

    }

    public void loadLevel()
    {

        //GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;
        //gameController.loadGame = true;
        GameControllerScr.loadGame = true;
        SceneManager.LoadScene("Game");
    }
    
    public void ButtonBack()
    {
        genField.TransformUnity transformUnity = new genField.TransformUnity();
        GameControllerScr gameController = GameObject.Find("Main Camera").GetComponent(typeof(GameControllerScr)) as GameControllerScr;
        Debug.Log(gameController.field);
        transformUnity.fromUnityToFile(gameController.field);
        SceneManager.LoadScene("Menu");
    }

}
