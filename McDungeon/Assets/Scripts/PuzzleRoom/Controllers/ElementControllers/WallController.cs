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
    public GameObject wallInterior;

    public GameObject blockPrefab;
    private List<GameObject> blockSprites;
    public Sprite blockSprite;

    protected WallStateModel myStateModel;

    protected string buttonTriggerID;

    protected bool openWhenPress = false;

    private Vector3 openPosition;
    private Vector3 closedPosition;

    protected float transitionTime;
    protected float currentTransitionAmount; // 0 is closed, transitionTime is open

    protected float delayTime;
    protected float currentDelayTime = 0.0f;

    public virtual void Init(string newElementID, PuzzleController pc, WallStateModel myModel, 
        string buttonTriggerID, Sprite blockSprite, Vector3 openPosition, Vector3 closedPosition, Vector3 wallScale, float transitionTime, float delayTime)
    {
        base.Init(newElementID, pc);
        myStateModel = myModel;
        this.buttonTriggerID = buttonTriggerID;

        this.openPosition = openPosition;
        this.closedPosition = closedPosition;

        this.transitionTime = transitionTime;
        this.delayTime = delayTime;
        currentTransitionAmount = 0.0f;
        blockSprites = new List<GameObject>();
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
        gameObject.transform.localPosition = newPosition;
    }

    protected void Rescale(Vector3 newScale)
    {
        gameObject.transform.localScale = newScale;
 
        int blockWidth = ((int)newScale.x) / 2;
        int blockHeight = ((int)newScale.y) / 2;
        for(int i = 0 ; i <  blockWidth; i++)
        {
            for(int j = 0 ; j < blockHeight ; j++)
            {
                GameObject newBlock = Instantiate(blockPrefab, gameObject.transform);
                newBlock.GetComponent<SpriteRenderer>().sprite = blockSprite;
                newBlock.transform.localScale = new Vector3(1.0f / ((float)blockWidth), 1.0f / ((float)blockHeight ), 0);
                newBlock.transform.localPosition = new Vector3(((-0.5f * (blockWidth - 1)) + i) / ((float)blockWidth), 
                    ((-0.5f * (blockHeight - 1)) + j) / ((float)blockHeight ), 0);
                blockSprites.Add(newBlock);
            }
        }

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

        wallInterior.GetComponent<Renderer>().enabled = false; // Set to true for destroy fireball debugging. 

        wallInterior.transform.localScale = new Vector3(1 - (5.0f / newScale.x), 1 - (5.0f / newScale.y), 0);
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
