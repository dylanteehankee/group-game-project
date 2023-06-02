using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStateModel : PuzzleElementStateModel
{
    public ButtonStateModel(PuzzleController pc, string id) : base((int) PuzzleButtonState.Unpressed, pc, id)
    {

    }
    
}

public enum PuzzleButtonState
{
    Pressed,
    Unpressed
}
