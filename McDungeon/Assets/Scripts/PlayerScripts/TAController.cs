using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using McDungeon;
public class TAController : MonoBehaviour
{  
    [SerializeField]
    private GameObject spawner;
    [SerializeField]
    private GameObject attenCodePrefab;
    private List<GameObject> mobList;
    private bool isBlackHole;
    private const float BLACKHOLEDURATION = 2.0f;
    private float elapsedTime = 0.0f;
    private float blackHoleEffect = 2.0f;
    private bool isSpinning = false;
    private const float SPINDURATION = 1.0f;

    void Start()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("MobSpawner");
        if (targets.Length > 0)
        {
            spawner = targets[0];
            this.mobList = spawner.GetComponent<MobManager>().GetMobs();
        }
    }

    void FixedUpdate()
    {
        if (elapsedTime < BLACKHOLEDURATION)
        {
            elapsedTime += Time.fixedDeltaTime;
            Vector2 location = this.transform.position;
            foreach (GameObject mob in mobList)
            {
                Vector2 mobLocation = mob.transform.position;
                Vector2 deltaLocation = location - mobLocation;
                deltaLocation.Normalize();
                mob.transform.Translate(deltaLocation * blackHoleEffect * Time.fixedDeltaTime);
            }
        }
        else if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine("spainWithoutTheA");
        }
    }

    private IEnumerator spainWithoutTheA()
    {
        this.GetComponent<Animator>().SetTrigger("Spin");
        for (int i = 0; i < 4; i++){
            for (int j = 0; i < 5; i++)
            {
                var attenCode = Instantiate(attenCodePrefab);
                attenCode.GetComponent<AttenCodeController>().Execute();
                attenCode.transform.position = this.transform.position;
                Destroy(attenCode, 2.0f);
            }
            yield return new WaitForSeconds(SPINDURATION / 4);
        }
        //PLAY EMAIL ME
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
