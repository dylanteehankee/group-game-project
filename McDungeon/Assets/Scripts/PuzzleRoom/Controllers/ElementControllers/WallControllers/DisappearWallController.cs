using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class DisappearWallController : StaticWallController
{
    protected bool openWhenPress = false;

    protected float transitionTime;
    protected float currentTransitionAmount; // 0 is closed, transitionTime is open

    protected float delayTime;
    protected float currentDelayTime = 0.0f;

    public void Init(string newElementID, PuzzleController pc, WallStateModel myModel, 
        Sprite blockSprite, Vector3 wallScale, float transitionTime, 
        float delayTime, bool openWhenPress)
    {
        this.openWhenPress = openWhenPress;
        this.transitionTime = transitionTime;
        this.delayTime = delayTime;
        currentTransitionAmount = 0.0f;
    }
    public void Init(string newElementID, PuzzleController pc, WallStateModel myModel, 
        Sprite blockSprite, Vector3 wallScale, float transitionTime, float delayTime)
    {
        base.Init(newElementID, pc, myModel, blockSprite, wallScale);
        this.transitionTime = transitionTime;
        this.delayTime = delayTime;
        currentTransitionAmount = 0.0f;
    }

    public override void RespondTo(PuzzleStateModel puzzleState, string invoker)
    {
        // No specified response behavior yet. 
    }

    protected void ChangeColorTransparency(float ratio)
    {
        Color oldColor = gameObject.GetComponent<SpriteRenderer>().color;
        if(oldColor.a == ratio)
            return;
        oldColor.a = ratio;
        gameObject.GetComponent<SpriteRenderer>().color = oldColor;
        // Change the main wall's controller to influence the blocks.

        foreach(GameObject block in blockSprites)
        {
            block.GetComponent<SpriteRenderer>().color = oldColor;
        }

        bool collisionActive = (ratio >= 0.1f); // Fade cutoff value for activity
        if(topWall.activeSelf != collisionActive)
        {
            topWall.SetActive(collisionActive);
            bottomWall.SetActive(collisionActive);
            leftWall.SetActive(collisionActive);
            rightWall.SetActive(collisionActive);
            wallInterior.SetActive(collisionActive);
            GetComponent<BoxCollider2D>().enabled = collisionActive;
        } 
    }

    protected virtual void ReactToState(PuzzleWallState myState, float timeElapsed)
    {
        switch(myState)
        {
            case PuzzleWallState.Open: 
                ChangeColorTransparency(0.0f);
                //gameObject.GetComponent<Renderer>().enabled = false;
                break;
            case PuzzleWallState.Opening:
                //gameObject.GetComponent<Renderer>().enabled = true;
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
                //gameObject.GetComponent<Renderer>().enabled = true;
                break;
            case PuzzleWallState.Closing:
                //gameObject.GetComponent<Renderer>().enabled = true;
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
