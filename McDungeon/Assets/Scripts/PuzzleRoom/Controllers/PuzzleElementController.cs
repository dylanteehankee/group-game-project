using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PuzzleElementController : MonoBehaviour
{
    // ID of the puzzle element.
    protected string myElementID;

    // PuzzleController that the elmeent belongs to.
    protected PuzzleController puzzleController;

    // Bool if the element has finished initiating. 
    protected bool hasInitiated = false;

    protected void Init(string myElementID, PuzzleController pc)
    {
        this.myElementID = myElementID;
        puzzleController = pc;
        hasInitiated = true;
    }
    /// <summary>
    /// Respond to change in the puzzle state, from a registered invoker. 
    /// </summary>
    /// <param name="puzzleState">States of the puzzle and all its elements. </param>
    /// <param name="invoker">ID of the registered invoker whose state change triggered the method. </param>
    public abstract void RespondTo(PuzzleStateModel puzzleState, string invoker);

}
