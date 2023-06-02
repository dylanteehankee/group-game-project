using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonController : PuzzleElementController
{

    private ButtonStateModel myStateModel;

    public void Init(string newElementID, PuzzleController pc, ButtonStateModel myModel)
    {
        base.Init(newElementID, pc);
        myStateModel = myModel;
    }

    public override void RespondTo(PuzzleStateModel puzzleState, string invoker)
    {
        // Button does not respond to other state changes in the puzzle. 
    }

    // Could be optional for now. 
    public override void OnChange()
    {

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

    public void OnStep()
    {
        if(myStateModel.GetState() != (int) PuzzleButtonState.Pressed)
        {
            myStateModel.SetState((int)PuzzleButtonState.Pressed);
        }
    }

    public void OnExit()
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
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(176,176,176,255);
                break;
            case PuzzleButtonState.Unpressed:
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(100,100,100,255);
                break;
        }
    }

}
