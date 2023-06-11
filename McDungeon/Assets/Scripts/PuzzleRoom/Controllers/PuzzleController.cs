using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using McDungeon;
/// <summary>
/// The PuzzleController should be an object directly under the puzzle room object. 
/// It should be positioned in the bototm left corner of the puzzle room. 
/// </summary>
public class PuzzleController : MonoBehaviour
{
    public UIManager uiManager;
    public MobManager mobManager;

    private Dictionary<string, PuzzleElementController> elementControllers;

    private Dictionary<string, List<string>> elementTriggers; 

    private PuzzleStateModel puzzleState;

    private PuzzleCreator puzzleCreator;

    private Dictionary<string, (int state, bool satisfied)> winCondition;

    private GameObject startButton;

    private float timeSinceStarted;
    private List<int> rewardCutoffs;
    private int knightCutoff; 

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
    [SerializeField] public GameObject staticWallPrefab;

    
    private Dictionary<string, PuzzleElementShapeLink> stringToPuzzleElementShape; 

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
        timeSinceStarted = 0.0f;

        // Create start button at room center. 
        startButton = Instantiate(startButtonPrefab, gameObject.transform);
        startButton.GetComponent<StartButtonController>().pc = this;
        // Sad Quickfix. 
        if(puzzleID == 5)
        {
            startButton.transform.localPosition = new Vector3(puzzleGridWidth - 2, puzzleGridHeight + 2, 0f);
        }
        else
        {
            startButton.transform.localPosition = new Vector3(puzzleGridWidth, puzzleGridHeight, 0f);
        }

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

        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        mobManager = GameObject.Find("MobSpawner").GetComponent<MobManager>(); 
    }

    public PuzzleRoomState GetPuzzleRoomState()
    {
        return puzzleState.roomState;
    }

    public void StartPuzzleRoom()
    {
        if(puzzleState.roomState == PuzzleRoomState.NotStarted)
        {      
            switch(puzzleID)
            {
                case 0:
                    InitPuzzleItems("PuzzleRoom_Tutorial.csv");
                    winCondition = new Dictionary<string, (int state, bool satisfied)>();
                    winCondition.Add("1", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("2", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("3", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("4", ((int)PuzzleTorchState.Lit, false));
                    break;
                case 1:
                {
                    InitPuzzleItems("PuzzleRoom_1.csv");
                    winCondition = new Dictionary<string, (int state, bool satisfied)>();
                    winCondition.Add("8", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("9", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("10", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("11", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("12", ((int)PuzzleTorchState.Lit, false));
                    break;
                }
                case 2:
                
                    //    InitPuzzle3();
                    InitPuzzleItems("PuzzleRoom_2.csv");
                    winCondition = new Dictionary<string, (int state, bool satisfied)>();
                    winCondition.Add("11", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("12", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("13", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("14", ((int)PuzzleTorchState.Lit, false));
                    break;
                case 3:
                    InitPuzzleItems("PuzzleRoom_3.csv");
                    winCondition = new Dictionary<string, (int state, bool satisfied)>();
                    winCondition.Add("10", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("11", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("12", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("13", ((int)PuzzleTorchState.Lit, false));
                    break;
                 case 4:
                    InitPuzzleItems("PuzzleRoom_4.csv");
                    winCondition = new Dictionary<string, (int state, bool satisfied)>();
                    winCondition.Add("7", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("8", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("9", ((int)PuzzleTorchState.Lit, false));
                    break;
                default:
                    break;
            }
        }
        startButton.SetActive(false);
        
        puzzleState.roomState = PuzzleRoomState.InProgress;
    }



    public void EndPuzzleRoom()
    {
        if(puzzleState.roomState == PuzzleRoomState.InProgress)
        {
            uiManager.HidePuzzleTime();
            puzzleState.roomState = PuzzleRoomState.Completed;
            foreach(KeyValuePair<string, PuzzleElementController> kvp in elementControllers)
            {
                Destroy(kvp.Value.gameObject);
                // Don't reset puzzle state.
                //elementControllers = null;
                //elementTriggers = null;
                //winCondition = null;
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
                    WinPuzzleRoom();
                }
            }
            else
            {
                winCondition[invokedID] = (winCondition[invokedID].state, false);
            }
        }
    }

    private void WinPuzzleRoom()
    {
        EndPuzzleRoom();
        List<GameObject> knights = mobManager.GetMobs();
        foreach(GameObject knight in knights)
        {
            knight.GetComponent<KnightController>().DestroyKnight();
        }
        if((int)timeSinceStarted <= rewardCutoffs[0])
        {
            // Grant some big rewards. 
        }
        else{
            // Grant some smaller rewards. 
        }
        // Winning sequence.
    }

    private void LosePuzzleRoom()
    {   
        EndPuzzleRoom();
        // Losing sequence, start the knights
        List<GameObject> knights = mobManager.GetMobs(); // Should only be knights. 
        foreach(GameObject knight in knights)
        {
            knight.GetComponent<KnightController>().ActivateKnight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleState.roomState == PuzzleRoomState.InProgress && rewardCutoffs != null)
        {
            timeSinceStarted += Time.deltaTime;
            uiManager.DisplayPuzzleTime(timeSinceStarted, rewardCutoffs, knightCutoff);
            if((int)timeSinceStarted >= knightCutoff)
            {    
                LosePuzzleRoom();
            }
        }
    }   

    
    private void InitPuzzleItems(string puzzlePath)
    {
        stringToPuzzleElementShape = new Dictionary<string, PuzzleElementShapeLink>();
        stringToPuzzleElementShape.Add("Circle", PuzzleElementShapeLink.Circle);
        stringToPuzzleElementShape.Add("Square", PuzzleElementShapeLink.Square);
        stringToPuzzleElementShape.Add("None", PuzzleElementShapeLink.None);
        
        // Initiate Knights for Testing
        
        Vector3 basePosition = this.gameObject.transform.position;
        List<Vector2> locations = new List<Vector2>();

        string filePath = "Assets/Resources/PuzzleRoomData/" + puzzlePath;
        //FileInfo openFile = new FileInfo("Assets/Resources/PuzzleRoomData/DemoTest.txt");

        // Read in Puzzle Room data as a CSV. 
        StreamReader fileReader = new StreamReader(File.OpenRead(filePath));
        string line = fileReader.ReadLine();
        
        while(line != null){
            string[] split = line.Split(',');
            switch(split[0])
            {
                case "PushButton":
                    AddObjectToPuzzle(puzzleCreator
                        .CreatePushButton(
                            button: Instantiate(buttonPrefab, gameObject.transform), 
                            id: split[1],
                            position: new Vector3(float.Parse(split[2]), float.Parse(split[3]), 0),
                            shape: stringToPuzzleElementShape[split[4]]
                        )
                    );
                    break;
                case "SwitchButton":
                    AddObjectToPuzzle(puzzleCreator
                        .CreateSwitchButton(
                            button: Instantiate(buttonSwitchPrefab, gameObject.transform), 
                            id: split[1],
                            position: new Vector3(float.Parse(split[2]), float.Parse(split[3]), 0),
                            shape: stringToPuzzleElementShape[split[4]]
                        )
                    );
                    break;
                case "DisappearWall":
                    AddObjectToPuzzle(puzzleCreator
                        .CreateDisappearWall(
                            wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                            id: split[1], 
                            buttonTriggerID: split[2],
                            shape: stringToPuzzleElementShape[split[3]],
                            wallScale: new Vector3(float.Parse(split[4]), float.Parse(split[5]), 1),
                            position: new Vector3(float.Parse(split[6]), float.Parse(split[7]), 0),
                            transitionTime: float.Parse(split[8]),
                            changePauseTime: float.Parse(split[9])
                        )
                    );
                    AddItemTrigger(
                        responderID: split[1], 
                        triggerID: split[2]
                    );
                    break;
                case "SlidingWall":
                     AddObjectToPuzzle(puzzleCreator
                        .CreateSlidingWall(
                            wall: Instantiate(wallPrefab, gameObject.transform), 
                            id: split[1], 
                            buttonTriggerID: split[2],
                            shape: stringToPuzzleElementShape[split[3]],
                            openPosition: new Vector3(float.Parse(split[6]), float.Parse(split[7]), 0),
                            closedPosition: new Vector3(float.Parse(split[8]), float.Parse(split[9]), 0),
                            wallScale: new Vector3(float.Parse(split[4]), float.Parse(split[5]), 1),
                            transitionTime: float.Parse(split[10]),
                            changePauseTime: float.Parse(split[11])
                        )
                    );
                    AddItemTrigger(
                        responderID: split[1], 
                        triggerID: split[2]
                    );
                    break;
                case "StaticWall": 
                    AddObjectToPuzzle(puzzleCreator
                        .CreateStaticWall(
                            wall: Instantiate(staticWallPrefab, gameObject.transform), 
                            id: split[1], 
                            shape: stringToPuzzleElementShape[split[2]],
                            wallScale: new Vector3(float.Parse(split[3]), float.Parse(split[4]), 1),
                            position: new Vector3(float.Parse(split[5]), float.Parse(split[6]), 0)
                        )
                    );
                    break;
                case "Torch":
                    AddObjectToPuzzle(puzzleCreator
                        .CreateTorch(
                            torch: Instantiate(torchPrefab, gameObject.transform), 
                            id: split[1], 
                            position: new Vector3(float.Parse(split[2]), float.Parse(split[3]), 0),
                            //expirable: bool.Parse(split[4]), 
                            expirable: true, 
                            lightDuration: float.Parse(split[5])
                        )
                    );
                    break;
                case "Knight":
                    locations.Add(new Vector2(basePosition.x + float.Parse(split[1]), basePosition.y + float.Parse(split[2]))); 
                    break;
                case "RewardTime":
                Debug.Log("found reward times");
                    rewardCutoffs = new List<int>();
                    rewardCutoffs.Add(int.Parse(split[1]));
                    break;
                case "TotalTime":
                Debug.Log("total time");
                    knightCutoff = int.Parse(split[1]);
                    break;
                default:
                    break;
            }
            line = fileReader.ReadLine();
        }
        mobManager.SpawnKnights(locations.ToArray());
        fileReader.Close();
    }

    public void InitPuzzle3()
    {
        AddObjectToPuzzle(puzzleCreator
            .CreatePushButton(
                button: Instantiate(buttonPrefab, gameObject.transform), 
                id: "1",
                position: new Vector3(5.0f, 19.0f, 0),
                shape: PuzzleElementShapeLink.Square
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreatePushButton(
                button: Instantiate(buttonPrefab, gameObject.transform), 
                id: "2",
                position: new Vector3(13.0f, 7.0f, 0),
                shape: PuzzleElementShapeLink.Circle
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "3", 
                buttonTriggerID: "1",
                shape: PuzzleElementShapeLink.Square,
                wallScale: new Vector3(6.0f, 6.0f, 1),
                position: new Vector3(13.0f, 21.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "4", 
                buttonTriggerID: "2",
                shape: PuzzleElementShapeLink.Circle,
                wallScale: new Vector3(6.0f, 6.0f, 1),
                position: new Vector3(5.0f, 5.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "5", 
                buttonTriggerID: "1",
                shape: PuzzleElementShapeLink.Square,
                wallScale: new Vector3(2.0f, 2.0f, 1),
                position: new Vector3(1.0f, 7.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "6", 
                buttonTriggerID: "2",
                shape: PuzzleElementShapeLink.Circle,
                wallScale: new Vector3(2.0f, 2.0f, 1),
                position: new Vector3(9.0f, 19.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "7", 
                buttonTriggerID: null,
                shape: PuzzleElementShapeLink.None,
                wallScale: new Vector3(6.0f, 2.0f, 1),
                position: new Vector3(5.0f, 21.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "8", 
                buttonTriggerID: null,
                shape: PuzzleElementShapeLink.None,
                wallScale: new Vector3(2.0f, 2.0f, 1),
                position: new Vector3(3.0f, 19.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "9", 
                buttonTriggerID: null,
                shape: PuzzleElementShapeLink.None,
                wallScale: new Vector3(2.0f, 2.0f, 1),
                position: new Vector3(7.0f, 19.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "10", 
                buttonTriggerID: null,
                shape: PuzzleElementShapeLink.None,
                wallScale: new Vector3(6.0f, 2.0f, 1),
                position: new Vector3(13.0f, 5.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "11", 
                buttonTriggerID: null,
                shape: PuzzleElementShapeLink.None,
                wallScale: new Vector3(2.0f, 2.0f, 1),
                position: new Vector3(11.0f, 7.0f, 0),
                transitionTime: 1.0f,
                changePauseTime: 0.2f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateDisappearWall(
                wall: Instantiate(disappearWallPrefab, gameObject.transform), 
                id: "12", 
                buttonTriggerID: null,
                shape: PuzzleElementShapeLink.None,
                wallScale: new Vector3(2.0f, 2.0f, 1),
                position: new Vector3(15.0f, 7.0f, 0),
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
        AddItemTrigger(
            responderID: "5", 
            triggerID: "1"
        );
        AddItemTrigger(
            responderID: "6", 
            triggerID: "2"
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "13", 
                position: new Vector3(9.0f, 25.8f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "14", 
                position: new Vector3(17.0f, 25.8f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );
        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "15", 
                position: new Vector3(7.0f, 0.2f, 0),
                expirable: true, 
                lightDuration: 15.0f
            )
        );

        AddObjectToPuzzle(puzzleCreator
            .CreateTorch(
                torch: Instantiate(torchPrefab, gameObject.transform), 
                id: "16", 
                position: new Vector3(0.2f, 3.0f, 0),
                expirable: true, 
                lightDuration: 20.0f
            )
        );
        
        winCondition = new Dictionary<string, (int state, bool satisfied)>();
       
        winCondition.Add("13", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("14", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("15", ((int)PuzzleTorchState.Lit, false));
        winCondition.Add("16", ((int)PuzzleTorchState.Lit, false));
    }
}
