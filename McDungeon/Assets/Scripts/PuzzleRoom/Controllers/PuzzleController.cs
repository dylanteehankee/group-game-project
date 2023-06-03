using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleController : MonoBehaviour
{
    private Dictionary<string, PuzzleElementController> elementControllers;

    private Dictionary<string, List<string>> elementTriggers; 

    private PuzzleStateModel puzzleState;

    private PuzzleCreator puzzleCreator;

    private Dictionary<string, (int state, bool satisfied)> winCondition;

    private GameObject startButton;

    [SerializeField] public GameObject startButtonPrefab;

    [SerializeField] public GameObject torchPrefab;
    [SerializeField] public GameObject buttonPrefab;   
    [SerializeField] public GameObject wallPrefab;
    [SerializeField] public GameObject disappearWallPrefab;

    // Start is called before the first frame update
    void Start()
    {
       
        elementControllers = new Dictionary<string, PuzzleElementController>();
        elementTriggers = new Dictionary<string, List<string>>();
        puzzleState = new PuzzleStateModel();

        puzzleCreator = new PuzzleCreator(this);
        // InvokePuzzle1 for demo puzzle, InvokePuzzle3 for another puzzle
        //InitPuzzle2();
        startButton = Instantiate(startButtonPrefab, gameObject.transform);
        startButton.GetComponent<StartButtonController>().pc = this;
        startButton.transform.localPosition = new Vector3(-0.5f, 1.5f, 0f);
    }

    public void StartPuzzleRoom()
    {
        if(puzzleState.roomState == PuzzleRoomState.NotStarted)
        {
            puzzleState.roomState = PuzzleRoomState.InProgress;
            InitPuzzle2();
        }
        startButton.SetActive(false);
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
                position: new Vector3(8.8f, 4f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "2", 
                position: new Vector3(0.2f, 4f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "3", 
                position: new Vector3(8.8f, 1f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "4", 
                position: new Vector3(0.2f, 1f, 0),
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
            .CreateButton(
                button: Instantiate(buttonPrefab, gameObject.transform), 
                id: "1",
                position: new Vector3(-0.5f, 0.5f, 0) //(-0.5,+1.5)
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateButton(
                button: Instantiate(buttonPrefab, gameObject.transform), 
                id: "2",
                position: new Vector3(-0.5f, 2.5f, 0)
            )
        );
         AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "3", 
                buttonTriggerID: "1",
                wallScale: new Vector3(0.5f, 5, 1),
                position: new Vector3(2.5f, 1.5f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "4", 
                buttonTriggerID: "2",
                wallScale: new Vector3(0.5f, 5, 1),
                position: new Vector3(-3.5f, 1.5f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "5", 
                buttonTriggerID: null,
                wallScale: new Vector3(2f, 0.5f, 1),
                position: new Vector3(3f, 2.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "6", 
                buttonTriggerID: null,
                wallScale: new Vector3(0.5f, 1.5f, 1),
                position: new Vector3(-3f, -0.25f, 0),
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
                position: new Vector3(3.8f, -0.5f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "8", 
                position: new Vector3(3.8f, 1.25f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "9", 
                position: new Vector3(-4.8f, 3f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "10", 
                position: new Vector3(3.8f, 3f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "11", 
                position: new Vector3(-4.8f, -0.5f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "12", 
                position: new Vector3(-4.8f, -0.5f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "13", 
                position: new Vector3(-4.8f, 2.0f, 0),
                expirable: true, 
                lightDuration: 15.0f
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
