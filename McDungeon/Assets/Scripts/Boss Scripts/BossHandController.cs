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
    private float attackTime = 1.0f;
    private int damage = 2;
    private bool isAttack = false;
    private bool hitPlayer = false;

    void Start()
    {
        this.initialPosition = this.transform.position;
    }

    void FixedUpdate()
    {
        if(this.transform.position.y != this.buttonObject.transform.position.y)
        {
            float deltaY = this.buttonObject.transform.position.y - this.transform.position.y;
            deltaY += .2f;
            this.transform.Translate(new Vector2(0, deltaY) / 0.25f * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (this.isAttack && !this.hitPlayer && collider.gameObject.tag == "PlayerHitbox")
        {
            Debug.Log("HIT");
            collider.gameObject.GetComponent<PlayerController>().TakeDamage(damage, EffectTypes.None);
            this.hitPlayer = true;
        }
    }

    public void Execute(GameObject playerObject)
    {
        this.gameObject.SetActive(true);
        this.playerObject = playerObject;
        Debug.Log("HANDATTACK");
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
