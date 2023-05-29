using UnityEngine;

namespace McDungeon
{
    public class InteractiveItemController : MonoBehaviour
    {
        [SerializeField] private GameObject button;
        [SerializeField] private int maxUse;
        [SerializeField] private int used = 0;
        private bool active;

        void Start()
        {
            active = false;
            button.SetActive(false);

            if (maxUse <= 0)
            {
                maxUse = 1;
            }
        }

        void Update()
        {
            if (active)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // Interaction Happened.
                    Debug.Log("Interacted with the item");
                    ++used;
                    if (used >= maxUse)
                    {
                        active = false;
                        button.SetActive(false);
                    }
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Item collision Enter: " + other.gameObject.name);

            if (other.gameObject.tag == "Player")
            {
                active = true;
                button.SetActive(true);
            }
        }


        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Item collision Exit: " + other.gameObject.name);

            if (other.gameObject.tag == "Player")
            {
                active = false;
                button.SetActive(false);
            }
        }
    }
}
