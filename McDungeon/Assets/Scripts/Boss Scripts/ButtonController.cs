using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;
public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    private float inactiveTime = 2.0f;
    private bool isFollowing = false;
    private float pushTime = 1.0f;

    void FixedUpdate()
    {
        if (isFollowing)
        {
            this.transform.position = playerObject.transform.position;
        }
    }

    public void Execute(GameObject playerObject)
    {
        this.gameObject.SetActive(true);
        this.playerObject = playerObject;
        this.GetComponent<Animator>().SetTrigger("Start");
        StartCoroutine("followPlayer");
    }

    private IEnumerator followPlayer()
    {
        this.isFollowing = true;
        yield return new WaitForSeconds(inactiveTime);
        this.isFollowing = false;
        this.GetComponent<Animator>().SetTrigger("Active");
        yield return new WaitForSeconds(this.pushTime);
        this.gameObject.SetActive(false);
    }

    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if (collider.gameObject.tag == "BossHand")
    //     {
    //         Debug.Log("PRESSBUTTON");
    //         this.GetComponent<Animator>().SetTrigger("Press");
    //     }
    // }
}
