using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace McDungeon
{
    public class CarpetKonamiController : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        private char[] code = new char[] { 'S', 'W', 'A', 'S', 'W', 'A' };
        private GameObject[] codeParts;
        private GameObject[] elementParts;
        private GameObject[] elementParts_compeleted;
        private GameObject carpet_compeleted;
        private GameObject carpet_animation;
        private bool entered = false;
        private bool active = false;
        private bool compeleted = false;
        private bool animationFinished = false;
        private bool hasInput = false;
        private int progress;
        private float steps_interval = 0.5f;
        private float timer = 0.5f;
        private Light2D carpetLight;

        void Awake()
        {
            entered = false;
            active = false;
            progress = 0;
            codeParts = new GameObject[6];
            elementParts = new GameObject[4];
            elementParts_compeleted = new GameObject[4];

            for (int i = 0; i < 6; i++)
            {
                codeParts[i] = this.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < 4; i++)
            {
                elementParts[i] = this.transform.GetChild(i + 6).gameObject;
            }

            for (int i = 0; i < 4; i++)
            {
                elementParts_compeleted[i] = this.transform.GetChild(i + 10).gameObject;
            }

            carpet_compeleted = this.transform.GetChild(14).gameObject;
            carpet_animation = this.transform.GetChild(15).gameObject;
            carpetLight = this.gameObject.GetComponent<Light2D>();
        }

        void Update()
        {
            if (entered && !active)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    active = true;
                    progress = 0;
                    Debug.Log("Konami Code activated");
                }
            }

            if (active && !compeleted)
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
                    codeParts[progress].SetActive(true);
                    showInput(input);
                    progress++;
                    Debug.Log(input + " - Progress: " + progress);
                }
                else if (hasInput)
                {
                    falseInput();
                    Debug.Log(input + " - Progress: " + progress);
                }

                if (progress >= 6)
                {
                    progress = 0;
                    active = false;
                    compeleted = true;
                    Debug.Log("Konami Code compeleted");
                }
            }

            if (compeleted && !animationFinished)
            {
                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    executeStep(progress);
                    progress++;
                    timer = steps_interval;
                }
            }
        }

        private void showInput(char input)
        {
            for (int i =0; i <4; i++)
            {
                elementParts[i].SetActive(false);
            }

            switch (input)
            {
                case 'W':
                    elementParts[0].SetActive(true);
                    break;
                case 'D':
                    elementParts[1].SetActive(true);
                    break;
                case 'S':
                    elementParts[2].SetActive(true);
                    break;
                case 'A':
                    elementParts[3].SetActive(true);
                    break;

            }
            
        }

        private void falseInput()
        {
            while (progress > 0)
            {
                progress--;
                codeParts[progress].SetActive(false);
            }
            showInput('$');
            active = false;
            Debug.Log("Konami Code failed");
        }

        private void executeStep(int progress)
        {
            switch (progress)
            {
                case 0:
                    showInput('$');
                    elementParts[progress].SetActive(true);
                    break;
                case 1:
                    elementParts[progress-1].SetActive(false);
                    elementParts[progress].SetActive(true);
                    break;
                case 2:
                    elementParts[progress - 1].SetActive(false);
                    elementParts[progress].SetActive(true);
                    break;
                case 3:
                    elementParts[progress - 1].SetActive(false);
                    elementParts[progress].SetActive(true);
                    break;
                case 4:
                    // fire - 0
                    elementParts[progress - 1].SetActive(false);
                    elementParts[progress - 4].SetActive(true);
                    break;
                case 5:
                    // water - 1
                    elementParts[progress - 4].SetActive(true);
                    break;
                case 6:
                    // ice - 1
                    elementParts[progress - 4].SetActive(true);
                    break;
                case 7:
                    // lightning - 1
                    elementParts[progress - 4].SetActive(true);
                    break;
                case 8:
                    carpet_animation.SetActive(true);
                    break;
                case 12:
                    FinishCarpetAnimation();
                    break;
                default:
                    break;
            }
        }

        public void FinishCarpetAnimation()
        {

            for (int i = 0; i < 4; i++)
            {
                elementParts[i].SetActive(false);
                elementParts_compeleted[i].SetActive(true);
            }

            carpet_animation.SetActive(false);
            carpet_compeleted.SetActive(true);
            animationFinished = true;
        }

        void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Player Enter");
                entered = true;
                active = false;
                progress = 0;
                carpetLight.enabled = true;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {

            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Player Exit");
                entered = false;
                active = false;
                progress = 0;

                if (!compeleted)
                {
                    carpetLight.enabled = false;
                }
            }
        }
    }
}
