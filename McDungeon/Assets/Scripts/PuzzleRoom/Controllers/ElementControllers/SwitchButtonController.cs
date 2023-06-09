using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwitchButtonController : PushButtonController
{

    /// <summary>
    /// Player steps on the button. 
    /// </summary>
    public override void OnStep()
    {
        //Flip from pressed to unpressed, or vice versa. 
        if(myStateModel.GetState() == (int) PuzzleButtonState.Pressed)
        {
            myStateModel.SetState((int)PuzzleButtonState.Unpressed);
        }
        else if(myStateModel.GetState() == (int) PuzzleButtonState.Unpressed)
        {
            myStateModel.SetState((int)PuzzleButtonState.Pressed);
        }
    }

    /// <summary>
    /// Remove behvior when player steps off of the button. 
    /// </summary>
    public override void OnExit()
    {
        
    }
}
