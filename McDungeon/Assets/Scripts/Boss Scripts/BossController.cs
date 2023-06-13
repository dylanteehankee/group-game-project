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
    private GameObject buttonObject;
    [SerializeField]
    private GameObject laserObject;
    [SerializeField]
    private float bossHealth = 100.0f;
    private float moveSpeed = 0.5f;
    private float laserDamage = 2.0f;
    private float attackSpeed = 5.0f;
    private float attackCD = 0.0f;
    private int attackCycle = 0;
    private bool isAttack = false;
    private float handAttackTime = 3.0f;
    private float laserAttackTime = 3.5f;
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
        if (this.attackCD < this.attackSpeed && !isAttack)
        {
            this.attackCD += Time.fixedDeltaTime;
            this.moveTowardPlayer();
        }
        else if (!isAttack)
        {
            this.attackPlayer();
        }
    }

    private void moveTowardPlayer()
    {
        Vector2 location = this.transform.position;
        Vector2 playerLocation = this.playerObject.transform.position;
        Vector2 deltaLocation = playerLocation - location;
        if (deltaLocation.x > 0)
        {
            this.transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
            if (deltaLocation.x < 0.05)
            {
                this.animator.SetBool("Moving", false);
            }
            else
            {
                this.animator.SetBool("Moving", true);
                this.spriteRenderer.flipX = true;
            }
        }
        else if (deltaLocation.x < 0)
        {
            this.transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
            if (deltaLocation.x > -0.05)
            {
                this.animator.SetBool("Moving", false);
            }
            else
            {
                this.animator.SetBool("Moving", true);
                this.spriteRenderer.flipX = false;
            }
        }
        else
        {
            this.animator.SetBool("Moving", false);
        }
    }

    private void attackPlayer()
    {
        if (!isAttack)
        {
            isAttack = true;
            this.animator.SetBool("Attack", true);
            if (attackCycle < 2)
            {
                attackCycle += 1;
                Vector2 location = this.transform.position;
                Vector2 playerLocation = this.playerObject.transform.position;
                Vector2 deltaLocation = playerLocation - location;
                this.buttonObject.GetComponent<ButtonController>().Execute(this.playerObject);
                BossHandController handControl;
                if (deltaLocation.x >= 0)
                {
                    handControl = this.handRight.GetComponent<BossHandController>();
                    Debug.Log("ATTACKRIGHT");
                }
                else
                {
                    handControl = this.handLeft.GetComponent<BossHandController>();
                    Debug.Log("ATTACKLEFT");
                }
                StartCoroutine(attackHand(handControl));
            }
            else
            {
                attackCycle = 0;
                StartCoroutine("attackLaser");
            }
        }
    }

    private IEnumerator attackLaser()
    {
        this.animator.SetBool("Laser", true);
        yield return new WaitForSeconds(0.25f);
        this.laserObject.GetComponent<LaserController>().Execute();
        yield return new WaitForSeconds(this.laserAttackTime);
        this.animator.SetBool("Laser", false);
        this.animator.SetBool("Attack", false);
        this.isAttack = false;
        this.attackCD = 0;
    }

    private IEnumerator attackHand(BossHandController handControl)
    {
        yield return new WaitForSeconds(2.5f);
        handControl.Execute(this.playerObject);
        yield return new WaitForSeconds(1f);
        this.isAttack = false;
        this.animator.SetBool("Attack", false);
        this.attackCD = 0;
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
