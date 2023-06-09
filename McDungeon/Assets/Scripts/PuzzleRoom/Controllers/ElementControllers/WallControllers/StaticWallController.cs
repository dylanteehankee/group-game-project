using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StaticWallController : PuzzleElementController
{
    // Collider walls on four sides. 
    public GameObject topWall;
    public GameObject bottomWall;
    public GameObject leftWall;
    public GameObject rightWall;
    
    /// Interior wall used to destroy fireball prefabs. 
    public GameObject wallInterior;

    // User facing block sprites that are layered across the wall.  
    public GameObject blockPrefab;
    protected List<GameObject> blockSprites;
    protected Sprite blockSprite;

    // Wall model associated with this wall that stoers info about the wall. 
    protected WallStateModel myStateModel;

    /// <summary>
    /// Initialize parameters of a static, unmoving wall controller. 
    /// </summary>
    /// <param name="newElementID">ID of the puzzle element/controller pair.</param>
    /// <param name="pc">Reference to the main puzzle controller.</param>
    /// <param name="myModel">The associated wall state model.</param>
    /// <param name="blockSprite">The user facing sprite of the blocks to overlay this wall. </param>
    /// <param name="wallScale">The scale of the wall, where 2 units is one block, so x and y should be multiples of two. </param>
    public void Init(string newElementID, PuzzleController pc, WallStateModel myModel, 
         Sprite blockSprite, Vector3 wallScale)
    {
        base.Init(newElementID, pc);
        myStateModel = myModel;

        this.blockSprite = blockSprite;
        blockSprites = new List<GameObject>();
        Rescale(wallScale);
    }

    public override void RespondTo(PuzzleStateModel puzzleState, string invoker)
    {
       // Does not respond to any changes in other puzzle element states. 
    }

    /// <summary>
    /// Rescale the wall element and all its supporting elements to the desired scale.
    /// </summary>
    /// <param name="newScale">New scale of the wall element. </param>
    protected void Rescale(Vector3 newScale)
    {
        // Modify main wall object's scale. 
        gameObject.transform.localScale = newScale;
        gameObject.GetComponent<Renderer>().enabled = false;
        
        // Calculate width and height of the wall in terms of blocks. 
        int blockWidth = ((int)newScale.x) / 2;
        int blockHeight = ((int)newScale.y) / 2;
        // Create individual block sprites to lay over the wall. 
        for(int i = 0 ; i <  blockWidth; i++)
        {
            for(int j = 0 ; j < blockHeight ; j++)
            {
                // Create new block with the correct sprite. 
                GameObject newBlock = Instantiate(blockPrefab, gameObject.transform);
                newBlock.GetComponent<SpriteRenderer>().sprite = blockSprite;
                // Set the local scale and position appropriately. 
                // Local scale is inverse of the main wall object's scale such that the scale is 1, 1, 0 relative to main wall object. 
                newBlock.transform.localScale = new Vector3(1.0f / ((float)blockWidth), 1.0f / ((float)blockHeight), 0);
                // Set the new position based on i and j indices. 
                newBlock.transform.localPosition = new Vector3(((-0.5f * (blockWidth - 1)) + i) / ((float)blockWidth), 
                    ((-0.5f * (blockHeight - 1)) + j) / ((float)blockHeight ), 0);
                blockSprites.Add(newBlock);
            }
        }
        // Modify the scale by 10x for fireball collider wall calculations. 
        newScale = 10.0f * newScale;
        topWall.transform.localScale = new Vector3(1, 0.1f / newScale.y, 0);
        topWall.transform.localPosition = new Vector3(0, 0.5f - (0.05f/newScale.y), 0);

        bottomWall.transform.localScale = new Vector3(1, 0.1f / newScale.y, 0);
        bottomWall.transform.localPosition = new Vector3(0, (0.5f - (0.05f/newScale.y)) * -1, 0);

        leftWall.transform.localScale = new Vector3(1, 0.1f / newScale.x, 0);
        leftWall.transform.localPosition = new Vector3((0.5f - (0.05f/newScale.x)) * -1, 0, 0);

        rightWall.transform.localScale = new Vector3(1, 0.1f / newScale.x, 0);
        rightWall.transform.localPosition = new Vector3(0.5f - (0.05f/newScale.x), 0, 0);

        // Hide the collider walls from user, note that they still collide properly. 
        topWall.GetComponent<Renderer>().enabled = false;
        bottomWall.GetComponent<Renderer>().enabled = false;
        leftWall.GetComponent<Renderer>().enabled = false;
        rightWall.GetComponent<Renderer>().enabled = false;

        // Hide the interior wall and rescale it to fit properly within the main wall object. 
        wallInterior.GetComponent<Renderer>().enabled = false; // Set to true for destroy fireball debugging. 
        wallInterior.transform.localScale = new Vector3(1 - (5.0f / newScale.x), 1 - (5.0f / newScale.y), 0);
    }

    /// <summary>
    /// Nothing to update as the static wall simply exists statically. 
    /// </summary>
    void Update()
    {
    }

}
