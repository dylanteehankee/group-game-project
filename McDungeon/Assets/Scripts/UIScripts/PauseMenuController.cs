using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private UIManager uiManager;
    public bool isPaused = false;
    void Start()
    {   
        Time.timeScale = 1;
        uiManager = gameObject.GetComponent<UIManager>();
    }
   
    public void RestartGame()
    {
        Time.timeScale = 1;
        //SceneManager.LoadScene( SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        GlobalStates.isPaused = true; 
        this.isPaused = true; 
        uiManager.OpenPauseGameUI();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
            GlobalStates.isPaused = false;
            this.isPaused = false;
        uiManager.ClosePauseGameUI();
    }

    public void TogglePause()
    {
        if(Time.timeScale == 0)
        {
            UnpauseGame();
            
        }
        else
        {
            PauseGame();
  
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
        //RestartGame();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}

public static class GlobalStates
{
    public static bool isPaused = false;
}


