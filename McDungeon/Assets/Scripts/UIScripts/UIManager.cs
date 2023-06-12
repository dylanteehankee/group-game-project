using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public GameObject speechBubblePrefab;

    public GameObject puzzleTimeUI;
    private PuzzleTimeController ptc;

    [SerializeField] public int coinAmount = 0;

    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            
        }
    }
}
