using UnityEngine;

namespace McDungeon
{
    public class McCurtainController : MonoBehaviour
    {
        private float timer = 10f;
        private bool unlocked = false;
        private bool disabledAnimator = false;
        private SpriteRenderer renderer;
        private float transparency;

        void Start()
        {
            renderer = this.gameObject.GetComponent<SpriteRenderer>();

        }
        void Update()
        {
            if (unlocked)
            {
                if (timer > 0f)
                {
                    timer -= Time.deltaTime;
                }

                if (timer <= 5f)
                {
                    if (!disabledAnimator)
                    {
                        this.gameObject.GetComponent<Animator>().enabled = false;
                        disabledAnimator = true;
                    }

                    transparency -= 255f / 10f * Time.deltaTime;
                    ChangeTransparency();
                }

                if (timer <= 0f)
                {
                    transparency = 0f;
                    ChangeTransparency();
                    unlocked = false; // Stop Checking
                }
            }
        }

        public void Unlock()
        {
            unlocked = true;
            timer = 10f;
            this.gameObject.GetComponent<Animator>().enabled = true;
            this.transform.parent.GetComponent<PortalController>().Unlock();
        }

        private void ChangeTransparency()
        {
            Color color = renderer.material.color;
            if (transparency < 0f)
            {

                color.a = 0f;
            }
            else
            {
                color.a = transparency;

            }
            renderer.material.color = color;
        }

    }
}
