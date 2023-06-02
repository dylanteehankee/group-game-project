
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class PuzzleElementStateModel
{   

    protected int myState;

    public string myElementID;

    private PuzzleController puzzleController;

    public PuzzleElementStateModel(int initState, PuzzleController pc, string myElementID)
    {
        myState = initState;
        puzzleController = pc;
        this.myElementID = myElementID;
    }

    public void SetState(int newState)
    {
        myState = newState;
        puzzleController.TriggerResponders(this.myElementID);
    }

    public int GetState()
    {
        return myState;
    }

}
