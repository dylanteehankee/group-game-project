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
                isPaused = false;
            }
            else
            {
                Time.timeScale = 0;
                isPaused = true; 
            }
        }
    }
}


