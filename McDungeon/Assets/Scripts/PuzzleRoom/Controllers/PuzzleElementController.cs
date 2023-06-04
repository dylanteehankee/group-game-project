using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PuzzleElementController : MonoBehaviour
{
    public string myElementID;

    public PuzzleController puzzleController;

    protected bool hasInitiated = false;

    protected void Init(string myElementID, PuzzleController pc)
    {
        this.myElementID = myElementID;
        puzzleController = pc;
        hasInitiated = true;
    }

    public abstract void RespondTo(PuzzleStateModel puzzleState, string invoker);

}
