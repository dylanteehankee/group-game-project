using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McDungeon
{
    public class BlizzardController : MonoBehaviour
    {
        [SerializeField] private float generationRate;
        [SerializeField] private float lifeTime;
        [SerializeField] private float scale;
        [SerializeField] private bool willMove;
        [SerializeField] private Vector3 direction;
        [SerializeField] private float speed;
        [SerializeField] private GameObject prefab_iceSharp;
        private float timeSinceBorn;
        private int generatedAmount;
        private float generationInterval;
        private float unitHeight = 5f; // Distance between Cloud and GroundRange.
        private float unitRadiusX = 2f; // Shape of range.
        private float unitRadiusY = 1.5f;
        private float cloudRatio= 1.17f;

        void Start()
        {
            Vector3 unitVec = new Vector3(1f, 1f, 1f);
            this.transform.localScale = unitVec * scale;
            unitHeight = unitHeight * scale;
            unitRadiusX = unitRadiusX * scale;
            unitRadiusY = unitRadiusY * scale;

            timeSinceBorn = 0f;
            generatedAmount = 0;
            generationInterval = 1 / generationRate;
        }


        public void Config(float generationRate, float lifeTime, float scale, bool willMove, Vector3 direction, float speed = 0f)
        {
            this.generationRate = generationRate;
            this.lifeTime = lifeTime;
            this.scale = scale;
            this.willMove = willMove;
            this.direction = direction;
            this.speed = speed;

            Vector3 unitVec = new Vector3(1f, 1f, 1f);
            this.transform.localScale = unitVec * scale;
            unitHeight = unitHeight * scale;
            unitRadiusX = unitRadiusX * scale;
            unitRadiusY = unitRadiusY * scale;

            timeSinceBorn = 0f;
            generatedAmount = 0;
            generationInterval = 1 / generationRate;
        }

        void Update()
        {
            if (willMove)
            {
                // Move the Blizzard.
                this.transform.position = this.transform.position + speed * direction * Time.deltaTime;
            }

            if (timeSinceBorn > generationInterval * (generatedAmount + 1) && timeSinceBorn < lifeTime)
            {
                // Select a random point within a circle.
                float ratioToCenter = Random.Range(0.05f, 1f);
                ratioToCenter -= ratioToCenter % 0.02f;
                Vector2 randomPoint = Random.insideUnitCircle.normalized * ratioToCenter;
                // Debug.Log(randomPoint);

                // Generate corresponded start/end position for ice-sharp.
                Vector3 startPos = new Vector3(randomPoint.x * unitRadiusX * cloudRatio, unitHeight + randomPoint.y * unitRadiusY * cloudRatio, 0f);
                Vector3 endPos = new Vector3(randomPoint.x * unitRadiusX, randomPoint.y * unitRadiusY, 0f);

                startPos = startPos + this.transform.position;
                endPos = endPos + this.transform.position;

                float targetScale = Random.Range(0.6f, 0.8f) * scale;

                // Generate new IceSharp.
                GameObject iceSharp = Instantiate(prefab_iceSharp, this.transform.position, Quaternion.identity);
                iceSharp.GetComponent<FallingIceSharpController>().Config(startPos, endPos, 1f, 1f, 3f, targetScale);
                // Debug.Log("ice sharp generated");

                generatedAmount++;
            }

            if (timeSinceBorn > lifeTime + 5f)
            {
                Destroy(this.gameObject);
            }


            timeSinceBorn += Time.deltaTime;
        }
    }
}
