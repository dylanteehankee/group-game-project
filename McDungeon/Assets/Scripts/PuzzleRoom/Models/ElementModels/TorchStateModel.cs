using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchStateModel : PuzzleElementStateModel
{
    public TorchStateModel(PuzzleController pc, string id) : base((int) PuzzleTorchState.Unlit, pc, id)
    {

    }
    
}

public enum PuzzleTorchState
{
    Lit,
    Unlit
}
