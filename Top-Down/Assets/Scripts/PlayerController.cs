using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 15.0f;
    private Rigidbody2D playerBody;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        MovePlayerFromInput();

        float angle = GetAngleToPoint((Vector2)Camera.main.ScreenToWorldPoint(
            new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * angle));
        
    }

    private void MovePlayerFromInput(){
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        playerBody.velocity = Vector2.ClampMagnitude(new Vector2(horizontalInput, verticalInput), 1) * 
            this.playerSpeed;
    }

    private float GetAngleToPoint(Vector2 pointTo)
    {
        Vector2 relativePos = pointTo - (Vector2)transform.position;
        return (float)Mathf.Atan2(relativePos.y, relativePos.x);
    }
}
