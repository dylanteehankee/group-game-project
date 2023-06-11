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
        [SerializeField] private bool unlocked;

        [SerializeField] private PlayerController playerControl;
        
        private bool active;

        void Start()
        {
            active = false;
            button.SetActive(false);
            unlocked = false;
            portal.SetActive(false);
            face.SetActive(false);


            this.playerControl = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
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

        public void Unlock()
        {
            unlocked = true;
            if (special)
            {
                cover.SetActive(false);
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
