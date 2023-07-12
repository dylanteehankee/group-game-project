using UnityEngine;

namespace McDungeon
{
    public class PortalController : MonoBehaviour
    {
        [SerializeField] private GameObject button;
        [SerializeField] private GameObject portal;
        [SerializeField] private GameObject face;
        [SerializeField] private GameMode mode;
        [SerializeField] private bool special;
        [SerializeField] private bool unlocked = false;
        [SerializeField] private PlayerController playerControl;
        private bool active = false;

        void Start()
        {
            this.playerControl = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
        }

        void Update()
        {
            if (active)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("trigger");
                    if (!special || unlocked)
                    {
                        Debug.Log("trigger2");
                        playerControl.StartUsePortal(this.gameObject.transform.position, mode);
                    }
                }
            }
        }

        private void setStatus(bool status)
        {
            active = status;
            button.SetActive(status);
            portal.SetActive(status);
            face.SetActive(status);
        }

        public void Unlock()
        {
            unlocked = true;
            setStatus(true);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Item collision Enter: " + other.gameObject.name);

            if (other.gameObject.tag == "Player")
            {
                setStatus(true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Item collision Exit: " + other.gameObject.name);

            if (other.gameObject.tag == "Player")
            {
                setStatus(false);
            }
        }
    }
}
