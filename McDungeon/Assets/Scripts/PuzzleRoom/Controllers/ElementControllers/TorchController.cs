using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class TorchController : PuzzleElementController
{
    private TorchStateModel myStateModel;

    public Sprite animationStartSprite;
    public Sprite unlitTorchSprite;

    private Animator myAnimator;
    private SpriteRenderer myRenderer;

    // Torch flickering when its close to expiring. 
    private float flickerTimeCutoff = 5f;
    private float flickerFrequency = 0.25f;

    // Keeps track of time since lit for expiring.   
    private float timeSinceLit = 0.0f;

    // Light duration in seconds and if the torch will expire. 
    private float lightDuration = 10.0f;
    private bool lightExpires = false;

    // Torch flickering colors. 
    private Color32 litColor = new Color32(255,255,255,255);
    private Color32 unlitColor = new Color32(150,150,150,255);

    /// <summary>
    /// Initialize torch. 
    /// </summary>
    /// <param name="newElementID"> ID of the new torch. </param>
    /// <param name="pc"> The puzzle controller of the puzzle torch belongs to. </param>
    /// <param name="myModel">Torch model that keeps track of torch state. </param>
    /// <param name="expirable">True if torch light expires after period of time. </param>
    /// <param name="lightDuration">Duration in seconds that the torch will stay light for. </param>
    public void Init(string newElementID, PuzzleController pc, TorchStateModel myModel, bool expirable, float lightDuration)
    {
        base.Init(newElementID, pc);
        myStateModel = myModel;
        myAnimator = gameObject.GetComponent<Animator>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.lightDuration = lightDuration;
        this.lightExpires = expirable;
    }

    public override void RespondTo(PuzzleStateModel puzzleState, string invoker)
    {
        // Torch does not respond to other state changes in the puzzle. 
    }
    
    /// <summary>
    /// Handles behaviors when touched by a fireball. 
    /// </summary>
    public void OnFireballTouch()
    {
        myStateModel.SetState((int)PuzzleTorchState.Lit);
        if(lightExpires == true)
        {
            timeSinceLit = 0.0f;
        }
    }

    /// <summary>
    /// Handles collisions with the torch, only triggered by fireball. 
    /// </summary>
    /// <param name="collision"> Collision object info </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect collission with the Fireball. 
        if(collision.name.Equals("FIreBall(Clone)"))
        {
            this.OnFireballTouch();
        }
    }

    void Update()
    {   
        if(!hasInitiated)
        { 
            return;
        }
        // Check if the torch light is meant to expire, and if its currently alight. 
        if(lightExpires == true && (PuzzleTorchState)myStateModel.GetState() == PuzzleTorchState.Lit)
        {
            // Increment lit duration.
            float timeElapsed = Time.deltaTime;
            timeSinceLit += timeElapsed;
            
            // Check if timeSinceLit exceeds how long it should be alight for. 
            if(timeSinceLit > lightDuration)
            {
                // Reset the torch state to unlit. 
                timeSinceLit = 0.0f;
                myStateModel.SetState((int)PuzzleTorchState.Unlit);
            }
        }
        
        // Switch behaviors based on current state. 
        switch((PuzzleTorchState)myStateModel.GetState())
        {
            case PuzzleTorchState.Lit:
                // Check if torch should flicker. 
                if(lightDuration - timeSinceLit < flickerTimeCutoff)
                {
                    // Oscillate by switching every flickerFrequency seconds. 
                    if( ((int)((lightDuration - timeSinceLit) / flickerFrequency) % 2) == 0)
                    {
                        myRenderer.color = litColor;
                    }
                    else
                    {
                        myRenderer.color = unlitColor; 
                    }
                }
                else
                {
                    myRenderer.color = litColor;
                }
                
                // Restart animator and set sprite if not already. 
                if(myAnimator.enabled == false)
                {
                    myAnimator.enabled = true;
                    myRenderer.sprite = animationStartSprite;
                }
                break;
            case PuzzleTorchState.Unlit:
                // Disable animator and set sprite if not already. 
                if(myAnimator.enabled == true)
                {
                    myAnimator.enabled = false;
                    myRenderer.sprite = unlitTorchSprite;
                }
                
                break;
        }
        
    }

}
