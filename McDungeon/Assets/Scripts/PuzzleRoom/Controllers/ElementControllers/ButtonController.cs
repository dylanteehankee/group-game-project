using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonController : PuzzleElementController
{

    protected ButtonStateModel myStateModel;

    protected Sprite pressedSprite;

    protected Sprite unpressedSprite;


    public void Init(string newElementID, PuzzleController pc, ButtonStateModel myModel, Sprite pressedSprite, Sprite unpressedSprite)
    {
        base.Init(newElementID, pc);
        myStateModel = myModel;
        this.pressedSprite = pressedSprite;
        this.unpressedSprite = unpressedSprite;
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
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            this.OnExit();
        }
    }

    public virtual void OnStep()
    {
        if(myStateModel.GetState() != (int) PuzzleButtonState.Pressed)
        {
            myStateModel.SetState((int)PuzzleButtonState.Pressed);
        }
    }

    public virtual void OnExit()
    {
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
