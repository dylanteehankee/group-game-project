using UnityEngine;

namespace McDungeon
{
    public class CarpetKonamiController : MonoBehaviour
    {
        [SerializeField] private GameObject button;
        [SerializeField] private char[] code;
        private bool active = false;
        private bool hasInput = false;
        private int progress;

        void Start()
        {
            active = false;
            progress = 0;
            code[0] = 'W';
            code[1] = 'S';
            code[2] = 'A';
            code[3] = 'W';
            code[4] = 'S';
            code[5] = 'A';
        }

        void Update()
        {
            if (active)
            {
                char input = '$';
                hasInput = true;
                if (Input.GetKeyUp(KeyCode.W))
                {
                    input = 'W';
                }
                else if (Input.GetKeyUp(KeyCode.S))
                {
                    input = 'S';
                }
                else if (Input.GetKeyUp(KeyCode.A))
                {
                    input = 'A';
                }
                else if (Input.GetKeyUp(KeyCode.D))
                {
                    input = 'D';
                }
                else
                {
                    hasInput = false;
                }


                if (input == code[progress])
                {
                    progress++;
                    Debug.Log(input + " - Progress: " + progress);
                }
                else if (hasInput)
                {
                    progress = 0;
                    Debug.Log(input + " - Progress: " + progress);
                }

                if (progress >= 6)
                {
                    Debug.Log("Konami Code compeleted");
                    progress = 0;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Item collision Enter: " + other.gameObject.name);

            if (other.gameObject.tag == "Player")
            {
                active = true;
                progress = 0;
            }
        }


        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Item collision Exit: " + other.gameObject.name);

            if (other.gameObject.tag == "Player")
            {
                active = false;
                progress = 0;
            }
        }
    }
}
