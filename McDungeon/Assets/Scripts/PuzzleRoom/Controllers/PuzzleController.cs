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
    public ItemFactory itemFactory;
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

        // Another sad quickfix. 
        if(puzzleID == 3)
        {
            startButton.GetComponent<StartButtonController>().startDelay = 0.1f;
        }
        else
        {
            startButton.GetComponent<StartButtonController>().startDelay = 0.8f;
        }

       
        // Sad Quickfix. 
        if(puzzleID == 4)
        {
            startButton.transform.localPosition = new Vector3(puzzleGridWidth - 8, puzzleGridHeight + 2, 0f);
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

        // Add extra height to top wall collider
        topWall.transform.localPosition = new Vector3(puzzleGridWidth, (puzzleGridHeight * 2) + 0.25f, 0);
        topWall.transform.localScale = new Vector3(puzzleGridWidth * 2, 0.5f, 1);

        botWall.transform.localPosition = new Vector3(puzzleGridWidth, 0 - 0.25f, 0);
        botWall.transform.localScale = new Vector3(puzzleGridWidth * 2, 0.5f, 1);

        leftWall.transform.localPosition = new Vector3(0 - 0.25f, puzzleGridHeight, 0);
        leftWall.transform.localScale = new Vector3(puzzleGridHeight * 2, 0.5f, 1);
        leftWall.transform.localRotation = Quaternion.Euler(0, 0, 90);

        rightWall.transform.localPosition = new Vector3((puzzleGridWidth * 2) + 0.25f, puzzleGridHeight, 0);
        rightWall.transform.localScale = new Vector3(puzzleGridHeight * 2, 0.5f, 1);
        rightWall.transform.localRotation = Quaternion.Euler(0, 0, 90);

        topWall.GetComponent<SpriteRenderer>().enabled = false;
        botWall.GetComponent<SpriteRenderer>().enabled = false;
        rightWall.GetComponent<SpriteRenderer>().enabled = false;
        leftWall.GetComponent<SpriteRenderer>().enabled = false;

        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        itemFactory = GameObject.Find("GameManager").GetComponent<ItemFactory>();
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
                    InitPuzzleItems("PuzzleRoom_Tutorial");
                    winCondition = new Dictionary<string, (int state, bool satisfied)>();
                    winCondition.Add("1", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("2", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("3", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("4", ((int)PuzzleTorchState.Lit, false));
                    GameObject.Find("GameManager").GetComponent<UIManager>().GenerateTextBubble(
                        this.gameObject.transform,
                        text:  "Light up all the torches with fireballs to progress. \n"
                            + "Do this quickly and you will be rewarded! Too slow and there will be consequences...",
                        dimensions: new Vector3(10, 2, 0), 
                        offset: new Vector3(7, 10, 0), 
                        fontSize: 4f,
                        duration: 15.0f      
                    );
                    break;
                case 1:
                {
                    InitPuzzleItems("PuzzleRoom_1");
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
                    InitPuzzleItems("PuzzleRoom_2");
                    winCondition = new Dictionary<string, (int state, bool satisfied)>();
                    winCondition.Add("11", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("12", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("13", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("14", ((int)PuzzleTorchState.Lit, false));
                    break;
                case 3:
                    InitPuzzleItems("PuzzleRoom_3");
                    winCondition = new Dictionary<string, (int state, bool satisfied)>();
                    winCondition.Add("10", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("11", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("12", ((int)PuzzleTorchState.Lit, false));
                    winCondition.Add("13", ((int)PuzzleTorchState.Lit, false));
                    break;
                 case 4:
                    InitPuzzleItems("PuzzleRoom_4");
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
        if(puzzleID == 0)
        {
            //Special case for tutorial room
            if((int)timeSinceStarted <= rewardCutoffs[0])
            {
                // Grant rewards
                itemFactory.DropSmallPuzzleRewards(gameObject.transform, new Vector3(puzzleGridWidth, puzzleGridHeight, 0), 1.0f);
                itemFactory.DropHealthPotions(gameObject.transform, 2, new Vector3(puzzleGridWidth, puzzleGridHeight, 0), 1.0f);
            }
            else{
                // Grant some smaller rewards. 
                itemFactory.DropHealthPotions(gameObject.transform, 2, new Vector3(puzzleGridWidth, puzzleGridHeight, 0), 1.0f);
            }
        }
        else
        {
            if((int)timeSinceStarted <= rewardCutoffs[0])
            {
                // Grant rewards
                itemFactory.DropBigPuzzleRewards(gameObject.transform, new Vector3(puzzleGridWidth, puzzleGridHeight, 0), 1.0f);
                itemFactory.DropHealthPotions(gameObject.transform, 2, new Vector3(puzzleGridWidth, puzzleGridHeight, 0), 1.0f);
            }
            else{
                // Grant some smaller rewards. 
                itemFactory.DropSmallPuzzleRewards(gameObject.transform, new Vector3(puzzleGridWidth, puzzleGridHeight, 0), 1.0f);
                itemFactory.DropHealthPotions(gameObject.transform, 2, new Vector3(puzzleGridWidth, puzzleGridHeight, 0), 1.0f);
            }
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

        string filePath = "PuzzleRoomData/" + puzzlePath;
        //FileInfo openFile = new FileInfo("Assets/Resources/PuzzleRoomData/DemoTest.txt");
        // Read in Puzzle Room data as a CSV. 
        string text = Resources.Load<TextAsset>(filePath).text;
        string[] lines = text.Split('\n');
        for(int i = 0 ; i < lines.Length ; i++)
        {
            string[] split = lines[i].Split(',');;
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
                    rewardCutoffs = new List<int>();
                    rewardCutoffs.Add(int.Parse(split[1]));
                    break;
                case "TotalTime":
                    knightCutoff = int.Parse(split[1]);
                    break;
                default:
                    break;
            }
        }
        mobManager.SpawnKnights(locations.ToArray());
    }
}