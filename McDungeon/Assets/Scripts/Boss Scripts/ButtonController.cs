using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;
public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    private float followTime = 2.25f;
    private bool isFollowing = false;
    private float pushTime = 1.5f;

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
        yield return new WaitForSeconds(this.followTime);
        this.isFollowing = false;
        yield return new WaitForSeconds(this.pushTime);
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "BossHand")
        {
            this.GetComponent<Animator>().SetTrigger("Press");
        }
    }
}
