using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class TorchController : PuzzleElementController
{

    //public GameObject fireBallPrefab;

    private TorchStateModel myStateModel;

    public Sprite animationStartSprite;
    public Sprite unlitTorchSprite;

    private Animator myAnimator;
    private SpriteRenderer myRenderer;

    private float flickerTimeCutoff = 4f;
    private float flickerFrequency = 0.5f;

    private float timeSinceLit = 0.0f;
    private float lightDuration = 10.0f;
    private bool lightExpires = false;

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
    
    public void OnFireballTouch()
    {
        Debug.Log("Lit torch");
        myStateModel.SetState((int)PuzzleTorchState.Lit);
        if(lightExpires == true)
        {
            timeSinceLit = 0.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.Equals("FIreBall(Clone)"))
        {
            this.OnFireballTouch();
        }
        /*
        else
        {
            Debug.Log("Not equal for whatever reason");
            Debug.Log("|" + collision.name + "| not equal to |" + "FireBall(Clone)" + "|");
        }
        */
        
        /*
        Debug.Log("Check if prefabs are equal");
        if(PrefabUtility.GetCorrespondingObjectFromSource(collision.gameObject) == fireBallPrefab)
        {
            this.OnFireballTouch();
        }
        */
    }

    void Update()
    {   
        if(!hasInitiated)
        { 
            return;
        }
        if(lightExpires == true && (PuzzleTorchState)myStateModel.GetState() == PuzzleTorchState.Lit)
        {
            float timeElapsed = Time.deltaTime;
            timeSinceLit += timeElapsed;

            if(timeSinceLit > lightDuration)
            {
                timeSinceLit = 0.0f;
                myStateModel.SetState((int)PuzzleTorchState.Unlit);
            }
        }
        
        switch((PuzzleTorchState)myStateModel.GetState())
        {
            case PuzzleTorchState.Lit:
                myRenderer.color = new Color32(255,255,255,255);
                if(lightDuration - timeSinceLit < flickerTimeCutoff)
                {
                    if( ((int)((lightDuration - timeSinceLit) / flickerFrequency) % 2) == 0)
                    {
                        myRenderer.color = new Color32(255,255,255,255);
                    }
                    else
                    {
                        myRenderer.color = new Color32(150,150,150,255);
                    }
                }
                else
                {
                    myRenderer.color = new Color32(255,255,255,255);
                }
                
                if(myAnimator.enabled == false)
                {
                    myAnimator.enabled = true;
                    myRenderer.sprite = animationStartSprite;
                }
                break;
            case PuzzleTorchState.Unlit:
                //gameObject.GetComponent<SpriteRenderer>().color = new Color32(100,100,100,255);
                if(myAnimator.enabled == true)
                {
                    myAnimator.enabled = false;
                    myRenderer.sprite = unlitTorchSprite;
                }
                
                break;
        }
        
    }

}
