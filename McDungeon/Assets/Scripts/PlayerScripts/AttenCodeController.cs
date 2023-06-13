using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;

public class AttenCodeController : MonoBehaviour
{
    [SerializeField]
    private float damage = 2.0f;
    [SerializeField]
    private float speed = 500.0f;
    public void Execute()
    {
        Vector2 direction = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        direction.Normalize();
        this.GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "MobHitbox")
        {
            collider.GetComponent<IMobController>().TakeDamage(this.damage, EffectTypes.Slow);
        }
        Destroy(this.gameObject);
    }
}
