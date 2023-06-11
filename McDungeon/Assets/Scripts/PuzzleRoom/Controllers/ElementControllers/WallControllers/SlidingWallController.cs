using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SlidingWallController : StaticWallController
{
    protected bool openWhenPress = false;

    private Vector3 openPosition;
    private Vector3 closedPosition;

    protected float transitionTime;
    protected float currentTransitionAmount; // 0 is closed, transitionTime is open

    protected float delayTime;
    protected float currentDelayTime = 0.0f;

    public virtual void Init(string newElementID, PuzzleController pc, WallStateModel myModel, 
        Sprite blockSprite, Vector3 openPosition, Vector3 closedPosition, Vector3 wallScale, float transitionTime, float delayTime)
    {
        base.Init(newElementID, pc, myModel, blockSprite, wallScale);
       
        this.openPosition = openPosition;
        this.closedPosition = closedPosition;

        this.transitionTime = transitionTime;
        this.delayTime = delayTime;
        currentTransitionAmount = 0.0f;
    }

    public override void RespondTo(PuzzleStateModel puzzleState, string invoker)
    {
        // No specified response behavior yet. 
    }

    protected void MoveTo(Vector3 newPosition)
    {
        gameObject.transform.localPosition = newPosition;
    }

    protected void ReactToState(PuzzleWallState myState, float timeElapsed)
    {
        switch(myState)
        {
            case PuzzleWallState.Open: 
                MoveTo(this.openPosition);
                break;
            case PuzzleWallState.Opening:
                currentTransitionAmount += timeElapsed;
                if(currentTransitionAmount > transitionTime)
                {
                    currentTransitionAmount = transitionTime;
                    myStateModel.SetState((int)PuzzleWallState.Open);
                }
                MoveTo(Vector3.Lerp(closedPosition, openPosition, currentTransitionAmount/transitionTime));
                break;
            case PuzzleWallState.Closed: 
                MoveTo(this.closedPosition);
                break;
            case PuzzleWallState.Closing:
                currentTransitionAmount -= timeElapsed;
                if(currentTransitionAmount < 0)
                {
                    currentTransitionAmount = 0;
                    myStateModel.SetState((int)PuzzleWallState.Closed);
                }
                MoveTo(Vector3.Lerp(closedPosition, openPosition, currentTransitionAmount/transitionTime));
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
