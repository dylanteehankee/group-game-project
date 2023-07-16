using System.Collections;
using System.Collections.Generic;
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
        private bool inProgress = false;

        void Start()
        {
            this.playerControl = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
        }

        void Update()
        {
            if (active && !inProgress)
            {
                if (Input.GetKeyDown(KeyCode.F) &&  !this.playerControl.GetModeLock())
                {
                    if (!special || unlocked)
                    {
                        inProgress = true;
                        StartCoroutine("inProg");
                        playerControl.StartUsePortal(this.gameObject.transform.position, mode);
                    }
                }
            }
        }

        private IEnumerator inProg()
        {
            yield return new WaitForSeconds(8f);
            inProgress = false;
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
            if (other.gameObject.tag == "Player")
            {
                setStatus(true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                setStatus(false);
            }
        }
    }
}
