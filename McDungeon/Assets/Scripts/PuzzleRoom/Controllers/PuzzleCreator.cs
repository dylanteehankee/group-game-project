using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleCreator
{
    private PuzzleController puzzleController;

    public PuzzleCreator(PuzzleController puzzleController)
    {
        this.puzzleController = puzzleController;
    }

    public (string, ButtonStateModel, ButtonController) CreateButton(GameObject button, string id, Vector3 position)
    {
        ButtonStateModel buttonModel = new ButtonStateModel(puzzleController, id);
        ButtonController buttonController = button.GetComponent<ButtonController>();
        buttonController.Init(id, puzzleController, buttonModel);
        button.transform.localPosition = position;
        return (id, buttonModel, buttonController);
    }

    public (string, ButtonStateModel, ButtonSwitchController) CreateSwitchButton(GameObject button, string id, Vector3 position)
    {
        ButtonStateModel buttonModel = new ButtonStateModel(puzzleController, id);
        ButtonSwitchController buttonController = button.GetComponent<ButtonSwitchController>();
        buttonController.Init(id, puzzleController, buttonModel);
        button.transform.localPosition = position;
        return (id, buttonModel, buttonController);
    }

    public (string, WallStateModel, WallController) CreateWall(GameObject wall, string id, string buttonTriggerID,
        Vector3 openPosition, Vector3 closedPosition, Vector3 wallScale, float transitionTime, float changePauseTime)
    {
        WallStateModel wallModel = new WallStateModel(puzzleController, id);
        WallController wallController = wall.GetComponent<WallController>();
        wallController.Init(id, puzzleController, wallModel, buttonTriggerID, openPosition, 
            closedPosition, wallScale, transitionTime, changePauseTime);
        wall.transform.localPosition = closedPosition;
        return (id, wallModel, wallController);
    }

    public (string, WallStateModel, DisappearWallController) CreateDisappearWall(GameObject wall, string id, string buttonTriggerID,
        Vector3 wallScale, Vector3 position, float transitionTime, float changePauseTime)
    {
        WallStateModel wallModel = new WallStateModel(puzzleController, id);
        DisappearWallController wallController = wall.GetComponent<DisappearWallController>();
        wallController.Init(id, puzzleController, wallModel, buttonTriggerID, wallScale, transitionTime, changePauseTime);
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
