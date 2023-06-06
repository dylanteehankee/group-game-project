using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PuzzleCreator
{
    private PuzzleController puzzleController;

    private readonly string puzzleImagePath = "Sprites/Puzzle/";

    private Dictionary<PuzzleElementShapeLink, (Sprite unpressed, Sprite pressed)> buttonSprites;
    // Key is the symbol on the button, whether it is square, circle, or none.
    // Value is tuple of sprites where (buttonUnpressed, buttonPressed). 

    private Dictionary<PuzzleElementShapeLink, Sprite> wallSprites;

    private Dictionary<string, PuzzleElementShapeLink> stringToPuzzleElementShape;

    public PuzzleCreator(PuzzleController puzzleController)
    {
        this.puzzleController = puzzleController;
        buttonSprites = new Dictionary<PuzzleElementShapeLink, (Sprite unpressed, Sprite pressed)>();
        buttonSprites.Add(PuzzleElementShapeLink.Circle, 
            (Resources.Load<Sprite>(puzzleImagePath + "ButtonElement/circle_button_unpressed"), 
            Resources.Load<Sprite>(puzzleImagePath + "ButtonElement/circle_button_pressed"))
        );
        buttonSprites.Add(PuzzleElementShapeLink.Square, 
            (Resources.Load<Sprite>(puzzleImagePath + "ButtonElement/square_button_unpressed"), 
            Resources.Load<Sprite>(puzzleImagePath + "ButtonElement/square_button_pressed"))
        );

        wallSprites = new Dictionary<PuzzleElementShapeLink, Sprite>();
        wallSprites.Add(PuzzleElementShapeLink.Square, 
            Resources.Load<Sprite>(puzzleImagePath + "WallElement/square_wall")
        );
        wallSprites.Add(PuzzleElementShapeLink.Circle, 
            Resources.Load<Sprite>(puzzleImagePath + "WallElement/circle_wall")
        );
        wallSprites.Add(PuzzleElementShapeLink.None, 
            Resources.Load<Sprite>(puzzleImagePath + "WallElement/static_wall")
        );

        stringToPuzzleElementShape = new Dictionary<string, PuzzleElementShapeLink>();
        stringToPuzzleElementShape.Add("Circle", PuzzleElementShapeLink.Circle);
        stringToPuzzleElementShape.Add("Square", PuzzleElementShapeLink.Square);
        stringToPuzzleElementShape.Add("None", PuzzleElementShapeLink.None);
    }

    public List<(string, PuzzleElementStateModel, PuzzleElementController)> GetPuzzleItems(string puzzlePath)
    {
        string filePath = "Assets/Resources/PuzzleRoomData/" + puzzlePath;
        //FileInfo openFile = new FileInfo("Assets/Resources/PuzzleRoomData/DemoTest.txt");
        List<(string, PuzzleElementStateModel, PuzzleElementController)> allElements = 
            new List<(string, PuzzleElementStateModel, PuzzleElementController)>();

        StreamReader fileReader = new StreamReader(File.OpenRead(filePath));
        string line = fileReader.ReadLine();
        
        while(line != null){
            string[] split = line.Split(',');
            Debug.Log(split[0]);
            switch(split[0])
            {
                /*
                case "PushButton":
                    allElements.Add(
                        CreateButton(
                            button: puzzleController.gameObject.Instantiate(puzzleController.buttonPrefab, puzzleController.gameObject.transform), 
                            id: split[1],
                            position: new Vector3(float.Parse(split[2]), float.Parse(split[3]), 0),
                            shape: stringToPuzzleElementShape[split[4]]
                        )
                    );
                    break;
                case "SwitchButton":
                    allElements.Add(
                        CreateSwitchButton(
                            button: puzzleController.gameObject.Instantiate(puzzleController.buttonSwitchPrefab, puzzleController.gameObject.transform), 
                            id: split[1],
                            position: new Vector3(float.Parse(split[2]), float.Parse(split[3]), 0),
                            shape: stringToPuzzleElementShape[split[4]]
                        )
                    );
                    break;
                case "DisappearWall":
                    allElements.Add(
                        CreateDisappearWall(
                            wall: puzzleController.gameObject.Instantiate(puzzleController.disappearWallPrefab, puzzleController.gameObject.transform), 
                            id: split[1], 
                            buttonTriggerID: split[2],
                            shape: stringToPuzzleElementShape[split[3]],
                            wallScale: new Vector3(float.Parse(split[4]), float.Parse(split[5]), 1),
                            position: new Vector3(float.Parse(split[6]), float.Parse(split[7]), 0),
                            transitionTime: float.Parse(split[8]),
                            changePauseTime: float.Parse(split[9])
                        )
                    );
                    break;
                case "SlidingWall":
                case "StaticWall":
                case "Torch":
                    allElements.Add(
                        CreateTorch(
                            torch: puzzleController.gameObject.Instantiate(puzzleController.torchPrefab, puzzleController.gameObject.transform), 
                            id: split[1], 
                            position: new Vector3(float.Parse(split[2]), float.Parse(split[3]), 0),
                            expirable: Boolean.Parse(split[4]), 
                            lightDuration: float.Parse(split[5])
                        )
                    );
                default:
                    break;
                */
            }
            line = fileReader.ReadLine();
        }
        fileReader.Close();
        return allElements;
    }

    public (string, ButtonStateModel, ButtonController) CreateButton(GameObject button, string id, Vector3 position, PuzzleElementShapeLink shape)
    {
        ButtonStateModel buttonModel = new ButtonStateModel(puzzleController, id);
        ButtonController buttonController = button.GetComponent<ButtonController>();
        buttonController.Init(id, puzzleController, buttonModel, buttonSprites[shape].pressed, buttonSprites[shape].unpressed);
        button.transform.localPosition = position;
        return (id, buttonModel, buttonController);
    }

    public (string, ButtonStateModel, ButtonSwitchController) CreateSwitchButton(GameObject button, string id, Vector3 position, PuzzleElementShapeLink shape)
    {
        ButtonStateModel buttonModel = new ButtonStateModel(puzzleController, id);
        ButtonSwitchController buttonController = button.GetComponent<ButtonSwitchController>();
        buttonController.Init(id, puzzleController, buttonModel, buttonSprites[shape].pressed, buttonSprites[shape].unpressed);
        button.transform.localPosition = position;
        return (id, buttonModel, buttonController);
    }

    public (string, WallStateModel, WallController) CreateWall(GameObject wall, string id, string buttonTriggerID, PuzzleElementShapeLink shape,
        Vector3 openPosition, Vector3 closedPosition, Vector3 wallScale, float transitionTime, float changePauseTime)
    {
        WallStateModel wallModel = new WallStateModel(puzzleController, id);
        WallController wallController = wall.GetComponent<WallController>();
        wallController.Init(id, puzzleController, wallModel, buttonTriggerID, wallSprites[shape],
            openPosition, closedPosition, wallScale, transitionTime, changePauseTime);
        wall.transform.localPosition = closedPosition;
        return (id, wallModel, wallController);
    }

    public (string, WallStateModel, DisappearWallController) CreateDisappearWall(GameObject wall, string id, string buttonTriggerID, 
        PuzzleElementShapeLink shape, Vector3 wallScale, Vector3 position, float transitionTime, float changePauseTime)
    {
        WallStateModel wallModel = new WallStateModel(puzzleController, id);
        DisappearWallController wallController = wall.GetComponent<DisappearWallController>();
        wallController.Init(id, puzzleController, wallModel, buttonTriggerID, wallSprites[shape], 
            wallScale, transitionTime, changePauseTime);
        wall.transform.localPosition = position;
        return (id, wallModel, wallController);
    }

    public (string, TorchStateModel, TorchController) CreateTorch(GameObject torch, string id, Vector3 position, bool expirable, float lightDuration)
    {
        TorchStateModel torchModel = new TorchStateModel(puzzleController, id);
        TorchController torchController = torch.GetComponent<TorchController>();
        torchController.Init(id, puzzleController, torchModel, expirable, lightDuration);
        torch.transform.localPosition = position;
        return (id, torchModel, torchController);
    }

}

public enum PuzzleElementShapeLink
{
    None,
    Square, 
    Circle
}
