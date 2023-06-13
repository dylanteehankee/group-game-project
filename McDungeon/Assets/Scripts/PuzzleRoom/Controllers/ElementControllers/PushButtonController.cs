using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PushButtonController : PuzzleElementController
{

    protected ButtonStateModel myStateModel;

    protected Sprite pressedSprite;

    protected Sprite unpressedSprite;

    protected AudioSource[] audioSource;

    /// <summary>
    /// Initialize the button. 
    /// </summary>
    /// <param name="newElementID">ID of the button. </param>
    /// <param name="pc">PuzzleController to which the button belongs to. </param>
    /// <param name="myModel">Button model that keeps track of the button state. </param>
    /// <param name="pressedSprite">Button pressed sprite. </param>
    /// <param name="unpressedSprite">Button unpressed sprite. </param>
    public void Init(string newElementID, PuzzleController pc, ButtonStateModel myModel, 
        Sprite pressedSprite, Sprite unpressedSprite)
    {
        base.Init(newElementID, pc);
        myStateModel = myModel;
        this.pressedSprite = pressedSprite;
        this.unpressedSprite = unpressedSprite;
        this.audioSource = this.GetComponents<AudioSource>();
    }

    public override void RespondTo(PuzzleStateModel puzzleState, string invoker)
    {
        // Button does not respond to other state changes in the puzzle. 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            this.OnStep();
            audioSource[0].Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            this.OnExit();
        }
    }
    /// <summary>
    /// Player steps on the button. 
    /// </summary>
    public virtual void OnStep()
    {
        // Set state to pressed. 
        if(myStateModel.GetState() != (int) PuzzleButtonState.Pressed)
        {
            myStateModel.SetState((int)PuzzleButtonState.Pressed);
        }
    }
    /// <summary>
    /// Player steps off the button. 
    /// </summary>
    public virtual void OnExit()
    {
        // Set state to unpressed. 
        if(myStateModel.GetState() != (int) PuzzleButtonState.Unpressed)
        {
            myStateModel.SetState((int)PuzzleButtonState.Unpressed);
        }
    }

    void Update()
    {
        if(!hasInitiated)
        { 
            return;
        }
        // Switch sprite based on the button state. 
        PuzzleButtonState buttonState = (PuzzleButtonState) myStateModel.GetState();
        switch(buttonState)
        {
            case PuzzleButtonState.Pressed:
                gameObject.GetComponent<SpriteRenderer>().sprite = pressedSprite;
                break;
            case PuzzleButtonState.Unpressed:
                gameObject.GetComponent<SpriteRenderer>().sprite = unpressedSprite;
                break;
        }
    }

}
