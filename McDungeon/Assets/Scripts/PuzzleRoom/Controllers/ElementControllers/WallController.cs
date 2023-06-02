using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallController : PuzzleElementController
{
    public GameObject topWall;
    public GameObject bottomWall;
    public GameObject leftWall;
    public GameObject rightWall;

    private WallStateModel myStateModel;

    private string buttonTriggerID;

    private bool openWhenPress = false;

    private Vector3 openPosition;
    private Vector3 closedPosition;

    private float transitionTime;
    private float currentTransitionAmount; // 0 is closed, transitionTime is open

    private float delayTime;
    private float currentDelayTime = 0.0f;

    public void Init(string newElementID, PuzzleController pc, WallStateModel myModel, 
        string buttonTriggerID, Vector3 openPosition, Vector3 closedPosition, Vector3 wallScale, float transitionTime, float delayTime)
    {
        base.Init(newElementID, pc);
        myStateModel = myModel;
        this.buttonTriggerID = buttonTriggerID;

        this.openPosition = openPosition;
        this.closedPosition = closedPosition;

        this.transitionTime = transitionTime;
        this.delayTime = delayTime;
        currentTransitionAmount = 0.0f;
        Rescale(wallScale);
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

    private void MoveTo(Vector3 newPosition)
    {
        gameObject.transform.position = newPosition;
    }

    private void Rescale(Vector3 newScale)
    {
        gameObject.transform.localScale = newScale;
        newScale = 10.0f * newScale;
        topWall.transform.localScale = new Vector3(1, 0.1f / newScale.y, 0);
        topWall.transform.localPosition = new Vector3(0, 0.5f - (0.05f/newScale.y), 0);

        bottomWall.transform.localScale = new Vector3(1, 0.1f / newScale.y, 0);
        bottomWall.transform.localPosition = new Vector3(0, (0.5f - (0.05f/newScale.y)) * -1, 0);

        leftWall.transform.localScale = new Vector3(1, 0.1f / newScale.x, 0);
        leftWall.transform.localPosition = new Vector3((0.5f - (0.05f/newScale.x)) * -1, 0, 0);

        rightWall.transform.localScale = new Vector3(1, 0.1f / newScale.x, 0);
        rightWall.transform.localPosition = new Vector3(0.5f - (0.05f/newScale.x), 0, 0);

        topWall.GetComponent<Renderer>().enabled = false;
        bottomWall.GetComponent<Renderer>().enabled = false;
        leftWall.GetComponent<Renderer>().enabled = false;
        rightWall.GetComponent<Renderer>().enabled = false;
    }

    // Could be optional for now. 
    public override void OnChange()
    {

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
    }

}
