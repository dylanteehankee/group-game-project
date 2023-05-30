using UnityEngine;

namespace McDungeon
{
    public class PortalController : MonoBehaviour
    {
        [SerializeField] private GameObject button;
        [SerializeField] private GameObject portal;
        [SerializeField] private GameObject face;
        [SerializeField] private GameObject cover;
        [SerializeField] private bool special;
        

        private bool active;
        private bool unlocked;

        void Start()
        {
            active = false;
            button.SetActive(false);
            unlocked = false;
            portal.SetActive(false);
            face.SetActive(false);
        }

        void Update()
        {
            if (active)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // Interaction Happened.
                    Debug.Log("Interacted with this portal");

                    if(special)
                    {
                        if (!unlocked)
                        {
                            Debug.Log("Portal Locked");

                            return;
                        }
                    }


                    // Non-special or unlocked.
                    Debug.Log("Activated Portal");

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
                portal.SetActive(true);
                face.SetActive(true);
            }
        }


        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Item collision Exit: " + other.gameObject.name);

            if (other.gameObject.tag == "Player")
            {
                active = false;
                button.SetActive(false);
                portal.SetActive(false);
                face.SetActive(false);
            }
        }
    }
}
