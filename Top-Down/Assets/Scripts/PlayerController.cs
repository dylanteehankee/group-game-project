using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player movement speed.
    private float playerSpeed = 15.0f;

    // Store rigidbody locally.
    private Rigidbody2D playerBody;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {   
        float timeElapsed = Time.deltaTime;
        MovePlayerFromInput();

        float angle = GetAngleToPoint((Vector2)Camera.main.ScreenToWorldPoint(
            new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * angle));

        CheckUserInput(timeElapsed);

    }

    // Checks for user input and potentially acts on it
    // Parameters:
    //      float timeElapsed - time since last update call in seconds
    private void CheckUserInput(float timeElapsed){
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Bang?!?");
        }
        if (Input.GetButtonDown("Fire2"))
        {
           
        }
    }

    // Move the player based on Input axis. 
    private void MovePlayerFromInput(){
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        

        // Move in direction of the horizontal and vertical input with magnitude speed. 
        playerBody.velocity = Vector2.ClampMagnitude(new Vector2(horizontalInput, verticalInput), 1) * 
            this.playerSpeed;
    }

    // Finds the angle of the gameObject to a point
    // Parameters:
    //      Vector2 pointTo - (x,y) of the point the gameObject should point to
    // Returns:
    //      float - angle to pointTo in radians
    private float GetAngleToPoint(Vector2 pointTo)
    {
        Vector2 relativePos = pointTo - (Vector2)transform.position;
        return (float)Mathf.Atan2(relativePos.y, relativePos.x);
    }
}
