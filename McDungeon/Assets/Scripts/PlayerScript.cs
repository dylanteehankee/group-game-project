using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 5f * Input.GetAxis("Horizontal"));
        transform.Translate(Vector3.up * Time.deltaTime * 5f * Input.GetAxis("Vertical"));
    }
}
