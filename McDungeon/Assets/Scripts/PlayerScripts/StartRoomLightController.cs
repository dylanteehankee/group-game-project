using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace McDungeon
{
    public class StartRoomLightController : MonoBehaviour
    {

        private Light2D[] lights;

        void Start()
        {
            lights = new Light2D[6];
            
            for (int i =0; i < 6; i++)
            {
                lights[i] = this.transform.GetChild(i).gameObject.GetComponent<Light2D>();
            }
        }

        public void UpdateLight(float intensity)
        {
            for (int i = 0; i < 6; i++)
            {
                lights[i].intensity = intensity;
            }
            
        }
    }

}