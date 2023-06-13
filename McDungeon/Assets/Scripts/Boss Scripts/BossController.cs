using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;
public class BossController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private StatusEffects statusEffects;
    [SerializeField]
    private GameObject handLeft;
    [SerializeField]
    private GameObject handRight;
    [SerializeField]
    private float bossHealth = 100.0f;
    private float laserDamage = 2.0f;
    private float attackSpeed = 5.0f;
    private float attackCD = 0.0f;
    private int attackCycle = 0;
    private bool isAblaze = false;
    private GameObject ablazeObject;
    private float deathAnimationTime = 5.0f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (this.attackCD < this.attackSpeed)
        {
            this.attackCD += Time.fixedDeltaTime;
        }
        else
        {
            this.attackPlayer();
        }
    }

    private void attackPlayer()
    {
        if (this.attackCD >= this.attackSpeed)
        {
            if (attackCycle > 2)
            {
                attackCycle += 1;
                int hand = Random.Range(0, 2);
                if (hand == 0)
                {
                    this.handLeft.GetComponent<BossHandController>().Attack(playerObject);
                }
                else
                {
                    this.handRight.GetComponent<BossHandController>().Attack(playerObject);
                }
            }
            else
            {
                this.attackLaser();
            }
        }
    }

    private void attackLaser()
    {
        //LASER
    }
    public virtual void TakeDamage(float damage, EffectTypes type)
    {
        this.bossHealth -= damage;
        this.death();
        this.status(type);
    }

    private IEnumerator damageEffect()
    {
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.5f);
            // Change color
        }
    }
    private void death()
    {
        if (this.bossHealth <= 0)
        {
            deathAnimation();
        }
    }

    private IEnumerator deathAnimation()
    {
        //Death animation
        yield return new WaitForSeconds(deathAnimationTime);
        Destroy(this.gameObject);
    }

    private void status(EffectTypes type)
    {
        if (type == EffectTypes.Ablaze)
        {
            if (!this.ablazeObject)
            {
                this.ablazeObject = this.statusEffects.Ablaze(this.transform, Vector2.one, Vector2.zero);
                this.isAblaze = true;
            }
            StopCoroutine("ablazeStatus");
            StartCoroutine("ablazeStatus");
        }
    }

    private IEnumerator ablazeStatus()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(this.statusEffects.GetAblazeDuration() / 4);
            this.bossHealth -= this.statusEffects.GetAblazeDamage();
            this.death();
        }
        this.isAblaze = false;
        Destroy(this.ablazeObject);
    }
}
