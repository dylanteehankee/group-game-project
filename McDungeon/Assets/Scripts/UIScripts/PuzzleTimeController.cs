using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTimeController : MonoBehaviour
{
    private int lastKnightCutoff;
    private List<int> lastRewardCutoff;

    public GameObject text_puzzleTime;
    public GameObject text_rewardTime;
    public GameObject text_failTime;

    private Color32 white;
    private Color32 red;
    private Color32 green; 
    private Color32 grey;

    void Start()
    {   
        white = new Color32(255, 255, 255, 255);
        green = new Color32(20, 255, 0, 255);
        grey = new Color32(160, 160, 160, 160);
        red = new Color32(255, 0, 34, 255);
    }
    public void DisplayPuzzleTime(float timeElapsed, List<int> rewardCutoff, int knightCutoff)
    {
        text_puzzleTime.GetComponent<Text>().color = white;
        if(rewardCutoff != lastRewardCutoff)
        {
            text_rewardTime.GetComponent<Text>().text = ":" + (knightCutoff - rewardCutoff[0]).ToString() + "s";
            lastRewardCutoff = rewardCutoff;
        }
        if(knightCutoff != lastKnightCutoff)
        {
            text_failTime.GetComponent<Text>().text = ": 0s";
            lastKnightCutoff = knightCutoff;
        }
        if((int)timeElapsed <= rewardCutoff[0])
        {
            text_rewardTime.GetComponent<Text>().color = green;
            text_failTime.GetComponent<Text>().color = white;
        }
        else if(timeElapsed < knightCutoff)
        {
            text_rewardTime.GetComponent<Text>().color = grey;
            text_failTime.GetComponent<Text>().color = green;
        }
        else{
            text_puzzleTime.GetComponent<Text>().color = red;
        }
        int displayTime = (knightCutoff - (int)timeElapsed);
        text_puzzleTime.GetComponent<Text>().text = displayTime.ToString() + "s";
    }

    void Update()
    {
       
    }
}


