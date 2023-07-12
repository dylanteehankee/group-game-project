using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject speechBubblePrefab;

    public GameObject puzzleTimeUI;
    private PuzzleTimeController ptc;

    private PauseMenuController pmc;

    private float timeAfterDeath = 0.0f;
    private float deathTime = 1.0f;
    private bool isDead = false;

    [SerializeField] public int coinAmount = 0;

    public GameObject menus;
    public GameObject blackFade;
    public GameObject menuText;
    public GameObject deadPlayer;


    void Start()
    {
        pmc = gameObject.GetComponent<PauseMenuController>();
        GlobalStates.isPaused = false;
        menus.SetActive(false);
        puzzleTimeUI.SetActive(false);
        ptc = puzzleTimeUI.GetComponent<PuzzleTimeController>();
    }

    public void GenerateTextBubble(Transform parent, string text, Vector3 dimensions, Vector3 offset, float fontSize, float duration)
    {
        GameObject newTextBubble = Instantiate(speechBubblePrefab, parent);
        
        newTextBubble.GetComponent<SpeechBubbleController>().Init(
            text: text,
            dimensions: dimensions, 
            offset: offset, 
            fontSize: fontSize, 
            duration: duration
        );
        
    }

    public void DisplayPuzzleTime(float timeElapsed, List<int> rewardCutoff, int knightCutoff)
    {
        puzzleTimeUI.SetActive(true);
        ptc.DisplayPuzzleTime(timeElapsed, rewardCutoff, knightCutoff);
    }

    public void HidePuzzleTime()
    {
        Invoke("HidePuzzleTimeCanvas", 2);
    }
    private void HidePuzzleTimeCanvas()
    {
        puzzleTimeUI.SetActive(false);
    }

    public void GameOver()
    {
        pmc.UnpauseGame();
        isDead = true;
        menus.SetActive(true);
        deadPlayer.SetActive(true);
        menuText.GetComponent<Text>().text = "Game Over";
        menuText.GetComponent<Text>().color = new Color32(255, 0, 0, 255);
        //pauseMenu.GetComponent<PauseMenuController>().Pau
    }

    public void OpenPauseGameUI()
    {
        if(!isDead)
        {
            menus.SetActive(true);
            deadPlayer.SetActive(false);
            blackFade.GetComponent<Image>().color = new Color32(0,0,0,0);
            menuText.GetComponent<Text>().text = "Paused";
            menuText.GetComponent<Text>().color = new Color32(255, 255, 0, 255);
        }

    }
    public void ClosePauseGameUI()
    {
        if(!isDead)
        {
            menus.SetActive(false);
        }
    }

    public void QuiteGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            if(timeAfterDeath <= deathTime)
            {
                timeAfterDeath += Time.deltaTime;
                Color oldColor = blackFade.GetComponent<Image>().color;
                oldColor.a = (timeAfterDeath)/deathTime;
                blackFade.GetComponent<Image>().color = oldColor;
            }
            else
            {
                pmc.PauseGame();
            }
        }
        /*if(Input.GetKeyDown(KeyCode.L))
        {
            GameOver();
        }*/
    }
}
