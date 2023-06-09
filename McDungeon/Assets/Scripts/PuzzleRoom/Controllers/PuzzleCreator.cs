using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PuzzleCreator
{
    private PuzzleController puzzleController;

    private static readonly string puzzleImagePath = "Sprites/Puzzle/";

    private Dictionary<PuzzleElementShapeLink, (Sprite unpressed, Sprite pressed)> buttonSprites;
    // Key is the symbol on the button, whether it is square, circle, or none.
    // Value is tuple of sprites where (buttonUnpressed, buttonPressed). 

    private Dictionary<PuzzleElementShapeLink, Sprite> wallSprites;

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
    }

    public (string, ButtonStateModel, PushButtonController) CreatePushButton(GameObject button, string id, Vector3 position, PuzzleElementShapeLink shape)
    {
        ButtonStateModel buttonModel = new ButtonStateModel(puzzleController, id);
        PushButtonController buttonController = button.GetComponent<PushButtonController>();
        buttonController.Init(id, puzzleController, buttonModel, buttonSprites[shape].pressed, buttonSprites[shape].unpressed);
        button.transform.localPosition = position;
        return (id, buttonModel, buttonController);
    }

    public (string, ButtonStateModel, SwitchButtonController) CreateSwitchButton(GameObject button, string id, Vector3 position, PuzzleElementShapeLink shape)
    {
        ButtonStateModel buttonModel = new ButtonStateModel(puzzleController, id);
        SwitchButtonController buttonController = button.GetComponent<SwitchButtonController>();
        buttonController.Init(id, puzzleController, buttonModel, buttonSprites[shape].pressed, buttonSprites[shape].unpressed);
        button.transform.localPosition = position;
        return (id, buttonModel, buttonController);
    }
    public (string, WallStateModel, SinglyTriggeredSlidingWallController) CreateSlidingWall(GameObject wall, string id, string buttonTriggerID, PuzzleElementShapeLink shape,
        Vector3 openPosition, Vector3 closedPosition, Vector3 wallScale, float transitionTime, float changePauseTime)
    {
        WallStateModel wallModel = new WallStateModel(puzzleController, id);
        SinglyTriggeredSlidingWallController wallController = wall.GetComponent<SinglyTriggeredSlidingWallController>();
        wallController.Init(id, puzzleController, wallModel, buttonTriggerID, wallSprites[shape],
            openPosition, closedPosition, wallScale, transitionTime, changePauseTime);
        wall.transform.localPosition = closedPosition;
        return (id, wallModel, wallController);
    }

    public (string, WallStateModel, SinglyTriggeredDisappearWallController) CreateDisappearWall(GameObject wall, string id, string buttonTriggerID, 
        PuzzleElementShapeLink shape, Vector3 wallScale, Vector3 position, float transitionTime, float changePauseTime)
    {
        WallStateModel wallModel = new WallStateModel(puzzleController, id);
        SinglyTriggeredDisappearWallController wallController = wall.GetComponent<SinglyTriggeredDisappearWallController>();
        wallController.Init(id, puzzleController, wallModel, buttonTriggerID, wallSprites[shape], 
            wallScale, transitionTime, changePauseTime);
        wall.transform.localPosition = position;
        return (id, wallModel, wallController);
    }

    public (string, WallStateModel, StaticWallController) CreateStaticWall(GameObject wall, string id,
        PuzzleElementShapeLink shape, Vector3 wallScale, Vector3 position)
    {
        WallStateModel wallModel = new WallStateModel(puzzleController, id);
        StaticWallController wallController = wall.GetComponent<StaticWallController>();
        wallController.Init(id, puzzleController, wallModel, wallSprites[shape], wallScale);
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
