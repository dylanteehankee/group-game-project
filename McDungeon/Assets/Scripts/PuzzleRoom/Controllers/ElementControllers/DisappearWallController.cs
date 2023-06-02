using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DisappearWallController : PuzzleElementController
{
    public GameObject topWall;
    public GameObject bottomWall;
    public GameObject leftWall;
    public GameObject rightWall;

    private WallStateModel myStateModel;

    private string buttonTriggerID;

    private bool openWhenPress = false;

    private float transitionTime;
    private float currentTransitionAmount; // 0 is closed, transitionTime is open

    private float delayTime;
    private float currentDelayTime = 0.0f;

    public void Init(string newElementID, PuzzleController pc, WallStateModel myModel, 
        string buttonTriggerID, Vector3 wallScale, float transitionTime, float delayTime)
    {
        base.Init(newElementID, pc);
        myStateModel = myModel;
        this.buttonTriggerID = buttonTriggerID;

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

    private void ChangeColorTransparency(float ratio)
    {
        Color oldColor = gameObject.GetComponent<SpriteRenderer>().color;
        oldColor.a = ratio;
        gameObject.GetComponent<SpriteRenderer>().color = oldColor;

        bool collisionActive = (ratio >= 0.1f); // Fade cutoff value for activity
        if(topWall.activeSelf != collisionActive)
        {
            topWall.SetActive(collisionActive);
            bottomWall.SetActive(collisionActive);
            leftWall.SetActive(collisionActive);
            rightWall.SetActive(collisionActive);
            Debug.Log("Setting the walls active " + collisionActive);
        } 
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
                    ChangeColorTransparency(0.0f);
                    gameObject.GetComponent<Renderer>().enabled = false;
                    break;
                case PuzzleWallState.Opening:
                    gameObject.GetComponent<Renderer>().enabled = true;
                    currentTransitionAmount += timeElapsed;
                    if(currentTransitionAmount > transitionTime)
                    {
                        currentTransitionAmount = transitionTime;
                        myStateModel.SetState((int)PuzzleWallState.Open);
                    }
                    ChangeColorTransparency(1.0f - (currentTransitionAmount / transitionTime));
                    break;
                case PuzzleWallState.Closed: 
                    ChangeColorTransparency(1.0f);
                    gameObject.GetComponent<Renderer>().enabled = true;
                    break;
                case PuzzleWallState.Closing:
                    gameObject.GetComponent<Renderer>().enabled = true;
                    currentTransitionAmount -= timeElapsed;
                    if(currentTransitionAmount < 0)
                    {
                        currentTransitionAmount = 0;
                        myStateModel.SetState((int)PuzzleWallState.Closed);
                    }
                    ChangeColorTransparency(1.0f - (currentTransitionAmount / transitionTime));
                    break;
            }
        }
    }

}
