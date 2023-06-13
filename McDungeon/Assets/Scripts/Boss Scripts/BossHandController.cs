using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;

public class BossHandController : MonoBehaviour
{
    Vector2 playerLocation;
    private float attackTime;
    private Animator animator;
    private bool isAttack = false;
    private bool hitPlayer = false;

    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.isAttack && !this.hitPlayer && collider.gameObject.tag == "PlayerHitbox")
        {
            collider.gameObject.GetComponent<PlayerController>().TakeDamage(damage, EffectTypes.None);
            this.hitPlayer = true;
        }
    }

    public void attack(GameObject playerObject)
    {
        this.animator.SetBool("Attack", true);
        this.isAttack = true;
        StartCoroutine("attackCD");
    }

    private IEnumerator attackCD()
    {
        yield return new WaitForSeconds(this.attackTime);
        this.animator.SetBool("Attack", false);
        this.hitPlayer = false;
        this.isAttack = false;
    }
}
