using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// The PuzzleController should be an object directly under the puzzle room object. 
/// It should be positioned in the bototm left corner of the puzzle room. 
/// </summary>
public class PuzzleController : MonoBehaviour
{
    private Dictionary<string, PuzzleElementController> elementControllers;

    private Dictionary<string, List<string>> elementTriggers; 

    private PuzzleStateModel puzzleState;

    private PuzzleCreator puzzleCreator;

    private Dictionary<string, (int state, bool satisfied)> winCondition;

    private GameObject startButton;

    [SerializeField] public int puzzleID;
    [SerializeField] public int puzzleGridWidth;
    [SerializeField] public int puzzleGridHeight;

    [SerializeField] public GameObject bouncyWallPrefab;
    [SerializeField] public GameObject startButtonPrefab;

    [SerializeField] public GameObject torchPrefab;
    [SerializeField] public GameObject buttonPrefab;   
    [SerializeField] public GameObject buttonSwitchPrefab;  
    [SerializeField] public GameObject wallPrefab;
    [SerializeField] public GameObject disappearWallPrefab;

    void Start()
    {
       Init();
    }

    /// <summary>
    /// Init() initializes fields used by the puzzle controller, as well as the start puzzle button. 
    /// </summary>
    private void Init()
    {
        // Instantiate supporting objects and data structures. 
        elementControllers = new Dictionary<string, PuzzleElementController>();
        elementTriggers = new Dictionary<string, List<string>>();
        puzzleState = new PuzzleStateModel();
        puzzleCreator = new PuzzleCreator(this);

        // Create start button at room center. 
        startButton = Instantiate(startButtonPrefab, gameObject.transform);
        startButton.GetComponent<StartButtonController>().pc = this;
        startButton.transform.localPosition = new Vector3(puzzleGridWidth, puzzleGridHeight, 0f);

        // Create invisible bouncy walls for the fireball. 
        GameObject topWall = Instantiate(bouncyWallPrefab, gameObject.transform);
        GameObject botWall = Instantiate(bouncyWallPrefab, gameObject.transform);
        GameObject rightWall = Instantiate(bouncyWallPrefab, gameObject.transform);
        GameObject leftWall = Instantiate(bouncyWallPrefab, gameObject.transform);

        topWall.transform.localPosition = new Vector3(puzzleGridWidth, puzzleGridHeight * 2, 0);
        topWall.transform.localScale = new Vector3(puzzleGridWidth * 2, 0.1f, 1);

        botWall.transform.localPosition = new Vector3(puzzleGridWidth, 0, 0);
        botWall.transform.localScale = new Vector3(puzzleGridWidth * 2, 0.1f, 1);

        leftWall.transform.localPosition = new Vector3(0, puzzleGridHeight, 0);
        leftWall.transform.localScale = new Vector3(puzzleGridHeight * 2, 0.1f, 1);
        leftWall.transform.localRotation = Quaternion.Euler(0, 0, 90);

        rightWall.transform.localPosition = new Vector3(puzzleGridWidth * 2, puzzleGridHeight, 0);
        rightWall.transform.localScale = new Vector3(puzzleGridHeight * 2, 0.1f, 1);
        rightWall.transform.localRotation = Quaternion.Euler(0, 0, 90);

        topWall.GetComponent<SpriteRenderer>().enabled = false;
        botWall.GetComponent<SpriteRenderer>().enabled = false;
        rightWall.GetComponent<SpriteRenderer>().enabled = false;
        leftWall.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void StartPuzzleRoom()
    {
        if(puzzleState.roomState == PuzzleRoomState.NotStarted)
        {
            puzzleState.roomState = PuzzleRoomState.InProgress;
            switch(puzzleID)
            {
                case 1:
                    InitPuzzle1();
                    break;
                case 2:
                    InitPuzzle2();
                    break;
                default:
                    break;
            }
        }
        startButton.SetActive(false);
    }

    public void EndPuzzleRoom()
    {
        if(puzzleState.roomState == PuzzleRoomState.InProgress)
        {
            puzzleState.roomState = PuzzleRoomState.Completed;
            foreach(KeyValuePair<string, PuzzleElementController> kvp in elementControllers)
            {
                Destroy(kvp.Value.gameObject);
                puzzleCreator = null;
                elementControllers = null;
                elementTriggers = null;
                puzzleState = null;
                winCondition = null;
            }
        }
    }

    public void AddItemTrigger(string responderID, string triggerID)
    {
        elementTriggers[triggerID].Add(responderID);
    }

    public void AddObjectToPuzzle((string id, PuzzleElementStateModel stateModel, PuzzleElementController controller) inputTuple)
    {
        elementControllers.Add(inputTuple.id, inputTuple.controller);
        elementTriggers.Add(inputTuple.id, new List<string>());
        puzzleState.allStates.Add(inputTuple.id, inputTuple.stateModel);
    }

    public void TriggerResponders(string invokedID)
    {
        // Call the respondTo method of registered responders. 
        List<string> triggeredElements = elementTriggers[invokedID];
        for(int i = 0 ; i < triggeredElements.Count ; i++)
        {   
            elementControllers[triggeredElements[i]].RespondTo(puzzleState, invokedID);
        }
        // Check if a win condition has been satisfied. 
        if(winCondition.ContainsKey(invokedID))
        {
            if(winCondition[invokedID].state == puzzleState.allStates[invokedID].GetState())
            {
                winCondition[invokedID] = (winCondition[invokedID].state, true);
                // If a win condition has been satisfied, check if all the win conditions are satisfied. 
                bool wonGame = true;
                foreach(KeyValuePair<string, (int state, bool satisfied)> kvp in winCondition)
                {
                    if(kvp.Value.satisfied == false)
                    {
                        wonGame = false;
                        break;
                    }
                }
                if(wonGame)
                {
                    EndPuzzleRoom();
                    Debug.Log("You won the game");
                }
            }
            else
            {
                winCondition[invokedID] = (winCondition[invokedID].state, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            StartPuzzleRoom();
        }
    }   

    public void InitPuzzle1()
    {
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "1", 
                position: new Vector3(13.8f, 3f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "2", 
                position: new Vector3(0.2f, 5f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "3", 
                position: new Vector3(13.8f, 7f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "4", 
                position: new Vector3(0.2f, 9f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        
        winCondition = new Dictionary<string, (int state, bool satisfied)>();
        winCondition.Add("1", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("2", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("3", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("4", ((int)PuzzleTorchState.Lit, false));
    }

    public void InitPuzzle2()
    {
        AddObjectToPuzzle(puzzleCreator
            .CreateSwitchButton(
                button: Instantiate(buttonSwitchPrefab, gameObject.transform), 
                id: "1",
                position: new Vector3(11.0f, 5.0f, 0) 
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateButton(
                button: Instantiate(buttonPrefab, gameObject.transform), 
                id: "2",
                position: new Vector3(11.0f, 11.0f, 0)
            )
        );
         AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "3", 
                buttonTriggerID: "1",
                wallScale: new Vector3(2.0f, 14.0f, 1),
                position: new Vector3(19.0f, 7.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "4", 
                buttonTriggerID: "2",
                wallScale: new Vector3(2.0f, 14.0f, 1),
                position: new Vector3(3.0f, 7.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "5", 
                buttonTriggerID: null,
                wallScale: new Vector3(6.0f, 2.0f, 1),
                position: new Vector3(19.0f, 9.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "6", 
                buttonTriggerID: null,
                wallScale: new Vector3(2.0f, 4.0f, 1),
                position: new Vector3(5.0f, 3.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );

        AddItemTrigger(
            responderID: "3", 
            triggerID: "1"
        );
        AddItemTrigger(
            responderID: "4", 
            triggerID: "2"
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "7", 
                position: new Vector3(21.8f, 5.0f, 0),
                expirable: true, 
                lightDuration: 20.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "8", 
                position: new Vector3(21.8f, 7.0f, 0),
                expirable: true, 
                lightDuration: 20.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "9", 
                position: new Vector3(21.8f, 11.0f, 0),
                expirable: true, 
                lightDuration: 20.0f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "10", 
                position: new Vector3(0.2f, 3.0f, 0),
                expirable: true, 
                lightDuration: 20.0f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "11", 
                position: new Vector3(0.2f, 5.0f, 0),
                expirable: true, 
                lightDuration: 20.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "12", 
                position: new Vector3(0.2f, 9.0f, 0),
                expirable: true, 
                lightDuration: 20.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "13", 
                position: new Vector3(0.2f, 11.0f, 0),
                expirable: true, 
                lightDuration: 20.0f
            )
        );
        
        winCondition = new Dictionary<string, (int state, bool satisfied)>();
        winCondition.Add("7", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("8", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("9", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("10", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("11", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("12", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("13", ((int)PuzzleTorchState.Lit, false));
    }
}
