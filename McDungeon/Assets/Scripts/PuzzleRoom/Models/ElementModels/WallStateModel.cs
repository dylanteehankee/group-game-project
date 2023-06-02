using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStateModel : PuzzleElementStateModel
{
    public WallStateModel(PuzzleController pc, string id) : base((int) PuzzleWallState.Closed, pc, id)
    {

    }

}
public enum PuzzleWallState
{
    Open,
    Closed,
    Opening,
    Closing
}