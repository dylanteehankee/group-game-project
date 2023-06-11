
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PuzzleStateModel
{   
    public Dictionary<string, PuzzleElementStateModel> allStates;

    public PuzzleRoomState roomState;

    public PuzzleStateModel()
    {
        allStates = new Dictionary<string, PuzzleElementStateModel>();
        roomState = PuzzleRoomState.NotStarted;
    }

}

public enum PuzzleRoomState
{
    NotStarted,
    InProgress,
    Completed
}
