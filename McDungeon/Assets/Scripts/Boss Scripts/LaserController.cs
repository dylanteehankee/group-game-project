using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;
public class LaserController : MonoBehaviour
{
    private GameObject playerObject;
    private float attackTime = 4.0f;
    private int damage = 1;
    // Start is called before the first frame update
    public void Execute()
    {
        this.gameObject.SetActive(true);
        this.GetComponent<Animator>().SetTrigger("Start");
        StartCoroutine("laserCD");
    }
    
    private IEnumerator laserCD()
    {
        yield return new WaitForSeconds(3.0f);
        this.GetComponent<Animator>().SetTrigger("Dissipate");
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "PlayerHitbox")
        {
            Vector2 location = this.transform.position;
            Vector2 playerLocation = collider.transform.position;
            var deltaX = playerLocation.x - location.x;
            if (deltaX > 0)
            {
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 50);
            }
            else
            {
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 50);
            }
            Debug.Log("HIT");
            collider.gameObject.GetComponent<PlayerController>().TakeDamage(damage, EffectTypes.None);
        }
    }
}
