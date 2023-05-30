using UnityEngine;

namespace McDungeon
{
    public class PortalFaceController : MonoBehaviour
    {
        [SerializeField] private float maxTransparence;
        [SerializeField] private float minTransparence;
        [SerializeField] private float speed;
        private SpriteRenderer renderer;
        private float transparency;
        private float increase;

        void Start()
        {
            renderer = this.gameObject.GetComponent<SpriteRenderer>();
            transparency = minTransparence;
            increase = 1f;
        }

        void Update()
        {
            if (transparency < minTransparence - 0.1)
            {
                increase = 1f;
            }
            else if (transparency > maxTransparence)
            {
                increase = -1f;
            }

            transparency += speed / 4f * increase * Time.deltaTime;
            ChangeTransparency();
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
