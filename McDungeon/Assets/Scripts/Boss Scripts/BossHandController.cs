using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;

public class BossHandController : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonObject;
    Vector3 initialPosition;
    private GameObject playerObject;
    private float attackTime = 0.75f;
    private float slamSpeed = 15.0f;
    private int damage = 2;
    private bool isAttack = false;
    private bool hitPlayer = false;

    void Start()
    {
        this.initialPosition = this.transform.position;
    }

    void FixedUpdate()
    {
        if(this.transform.position.y >= this.buttonObject.transform.position.y + 0.5)
        {
            this.transform.Translate(new Vector2(0, -slamSpeed) * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.isAttack && !this.hitPlayer && collision.gameObject.tag == "PlayerHitbox")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 600);
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage, EffectTypes.None);
            this.hitPlayer = true;
        }
    }

    public void Execute(GameObject playerObject)
    {
        this.gameObject.SetActive(true);
        this.playerObject = playerObject;
        this.initialPosition = this.transform.position;
        var newPosition = new Vector2(this.buttonObject.transform.position.x, this.transform.position.y);
        this.transform.position = newPosition;
        this.isAttack = true;
        
        StartCoroutine("attackCD");
    }

    private IEnumerator attackCD()
    {
        yield return new WaitForSeconds(this.attackTime);
        this.hitPlayer = false;
        this.isAttack = false;
        this.transform.position = this.initialPosition;
        this.gameObject.SetActive(false);
    }
}
