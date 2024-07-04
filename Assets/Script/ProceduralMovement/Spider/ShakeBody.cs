using System;
using Script.CharacterMovement;
using UnityEngine;

namespace Script.ProceduralMovement.Spider
{
    [RequireComponent(typeof(ComponentSpiderMovement))]
    public class ShakeBody : MonoBehaviour
    {
        [SerializeField] Transform body;

        [SerializeField] BodyShakeData bodyShakeIdle, bodyShakeMove;


        private ComponentSpiderMovement player3D;
        private Vector3 bodyLocalPos;
        private Quaternion bodyLocalRot;
        private float timeOffset;

        public Transform Body
        {
            get => body;
        }


        void Awake()
        {
            Cache();
        }

        void OnDisable()
        {
            body.localPosition = bodyLocalPos;
            body.localRotation = bodyLocalRot;
        }


        void Cache()
        {
            player3D = GetComponent<ComponentSpiderMovement>();
            bodyLocalPos = body.localPosition;
            bodyLocalRot = body.localRotation;
            timeOffset = UnityEngine.Random.value * 1000;
        }

        void Update()
        {
            body.localPosition = bodyLocalPos + Vector3.Lerp(bodyShakeIdle.Pos(timeOffset),
                bodyShakeMove.Pos(timeOffset), player3D.SpeedProgress);
            body.localRotation = bodyLocalRot * Quaternion.Lerp(bodyShakeIdle.Rot(timeOffset),
                bodyShakeMove.Rot(timeOffset), player3D.SpeedProgress);
        }


        [Serializable]
        public class BodyShakeData
        {
            public float posNoise, rotNoise, ySin;
            public float posNoiseFreq, rotNoiseFreq, ySinFreq;

            public Vector3 Pos(float timeOffset = 0)
            {
                float time = Time.time + timeOffset;

                return new Vector3(
                    (0.5f - Mathf.PerlinNoise(000, time * posNoiseFreq)) * posNoise,
                    (0.5f - Mathf.PerlinNoise(100, time * posNoiseFreq)) * posNoise +
                    Mathf.Sin(time * Mathf.PI * 2 * ySinFreq) * ySin,
                    (0.5f - Mathf.PerlinNoise(200, time * posNoiseFreq)) * posNoise);
            }

            public Quaternion Rot(float timeOffset = 0)
            {
                float time = Time.time + timeOffset;

                return Quaternion.Euler(
                    (0.5f - Mathf.PerlinNoise(300, time * rotNoiseFreq)) * rotNoise,
                    (0.5f - Mathf.PerlinNoise(400, time * rotNoiseFreq)) * rotNoise,
                    (0.5f - Mathf.PerlinNoise(500, time * rotNoiseFreq)) * rotNoise);
            }
        }
    }
}