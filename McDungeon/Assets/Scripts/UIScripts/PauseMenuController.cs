using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    public bool isPaused = false;
    void Start()
    {   



    }
   
    public void RestartGame()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            RestartGame();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
                GlobalStates.isPaused = false;
                this.isPaused = false;
            }
            else
            {
                Time.timeScale = 0;
                GlobalStates.isPaused = true; 
                this.isPaused = true; 
            }
        }
    }
}

public static class GlobalStates
{
    public static bool isPaused = false;
}


