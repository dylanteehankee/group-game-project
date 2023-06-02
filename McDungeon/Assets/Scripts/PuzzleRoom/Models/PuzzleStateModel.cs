
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PuzzleStateModel
{   
    public Dictionary<string, PuzzleElementStateModel> allStates;

    public PuzzleStateModel()
    {
        allStates = new Dictionary<string, PuzzleElementStateModel>();
    }

}
