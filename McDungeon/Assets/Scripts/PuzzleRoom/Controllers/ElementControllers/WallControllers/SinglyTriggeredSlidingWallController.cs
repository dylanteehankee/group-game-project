using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SinglyTriggeredSlidingWallController : SlidingWallController
{
    private string buttonTriggerID;

    public virtual void Init(string newElementID, PuzzleController pc, WallStateModel myModel, string buttonTriggerID,
        Sprite blockSprite, Vector3 openPosition, Vector3 closedPosition, Vector3 wallScale, float transitionTime, float delayTime)
    {
        base.Init(newElementID, pc, myModel, blockSprite, openPosition, closedPosition, wallScale, transitionTime, delayTime);
        this.buttonTriggerID = buttonTriggerID;
    }

    public override void RespondTo(PuzzleStateModel puzzleState, string invoker)
    {
        ButtonStateModel triggerState = (ButtonStateModel) puzzleState.allStates[buttonTriggerID];
        PuzzleWallState myState = (PuzzleWallState) myStateModel.GetState();
        switch((PuzzleButtonState) triggerState.GetState())
        {
            case PuzzleButtonState.Unpressed:
                if(myState == PuzzleWallState.Open || myState == PuzzleWallState.Opening)
                {
                    myStateModel.SetState((int)PuzzleWallState.Closing);
                    currentDelayTime = 0.0f;
                }
                break;

            case PuzzleButtonState.Pressed:
                if(myState == PuzzleWallState.Closed || myState == PuzzleWallState.Closing)
                {
                    myStateModel.SetState((int)PuzzleWallState.Opening);
                    currentDelayTime = 0.0f;
                }
                break;
        }
    }


    void Update()
    {
        if(!hasInitiated)
        { 
            return;
        }
        PuzzleWallState myState = (PuzzleWallState) myStateModel.GetState();
        float timeElapsed = Time.deltaTime;
        currentDelayTime += timeElapsed;
        if(currentDelayTime > delayTime)
        {
            ReactToState(myState, timeElapsed);
        }
    }

}
