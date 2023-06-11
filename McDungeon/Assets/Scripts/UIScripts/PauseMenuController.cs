using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    void Start()
    {   



    }
   
    public void RestartGame()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
    }

    void Update()
    {
       
    }
}


