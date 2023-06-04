using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonSwitchController : ButtonController
{

    public override void OnStep()
    {
        if(myStateModel.GetState() == (int) PuzzleButtonState.Pressed)
        {
            myStateModel.SetState((int)PuzzleButtonState.Unpressed);
        }
        else if(myStateModel.GetState() == (int) PuzzleButtonState.Unpressed)
        {
            myStateModel.SetState((int)PuzzleButtonState.Pressed);
        }
    }

    public override void OnExit()
    {
        
    }
/*
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
*/
}
