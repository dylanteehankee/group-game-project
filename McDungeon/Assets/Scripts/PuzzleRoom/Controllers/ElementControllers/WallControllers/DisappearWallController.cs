using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class DisappearWallController : StaticWallController
{
    protected bool openWhenPress = false;

    protected float transitionTime;
    protected float currentTransitionAmount; 

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
    protected void Init(string newElementID, PuzzleController pc, WallStateModel myModel, 
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

    /// <summary>
    /// Change the transparency of the wall element, and all its supporting elements. 
    /// </summary>
    /// <param name="ratio"></param>
    protected void ChangeColorTransparency(float ratio)
    {
        // Change wall transparency value. 
        Color oldColor = gameObject.GetComponent<SpriteRenderer>().color;
        if(oldColor.a == ratio)
            return;
        oldColor.a = ratio;
        gameObject.GetComponent<SpriteRenderer>().color = oldColor;
        
        // Change transparency of all the user facing blocks. 
        foreach(GameObject block in blockSprites)
        {
            block.GetComponent<SpriteRenderer>().color = oldColor;
        }

        // Cutoff of transition value to activate the collisions. 
        bool collisionActive = (ratio >= 0.1f); 
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
    /// <summary>
    /// Respond to change in state. 
    /// </summary>
    /// <param name="myState">Enum of the wall state. </param>
    /// <param name="timeElapsed">Time elapsed since last call. </param>
    protected virtual void ReactToState(PuzzleWallState myState, float timeElapsed)
    {
        switch(myState)
        {
            // If wall is open, set transparent. 
            case PuzzleWallState.Open: 
                ChangeColorTransparency(0.0f);
                break;
            // If wall is opening, increase transition amount by timeElapsed.
            case PuzzleWallState.Opening:
                currentTransitionAmount += timeElapsed;
                // Check if should transition to Open. 
                if(currentTransitionAmount > transitionTime)
                {
                    currentTransitionAmount = transitionTime;
                    myStateModel.SetState((int)PuzzleWallState.Open);
                }
                ChangeColorTransparency(1.0f - (currentTransitionAmount / transitionTime));
                break;
            // If wall is closed, set completely visible. 
            case PuzzleWallState.Closed: 
                ChangeColorTransparency(1.0f);
                break;
            // If wall is closing, decrease transition amount by timeElapsed.
            case PuzzleWallState.Closing:
                currentTransitionAmount -= timeElapsed;
                // Check if should transition to Closed. 
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
