using System;
using Script.Manager;
using UnityEngine;

namespace Script.CharacterMovement.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float positionSpeed = 10;
        [SerializeField] private float rotationSpeed = 10;
        [SerializeField] private LayerMask layer;

        Vector3 posProj;
        Quaternion rotProj;

        

        public void InitCamFollow()
        {
            // if (!target)
            // {
            //     CamTarget camTarget = FindObjectOfType<CamTarget>();
            //     if (camTarget)
            //         target = camTarget.transform;
            // }

            posProj = target.InverseTransformPoint(transform.position);
            posProj.y = 10;
            rotProj = Quaternion.Inverse(target.rotation) * transform.rotation;
        }

        void Update()
        {
            Quaternion rot = target.rotation * rotProj;

            Vector3 pos = target.TransformPoint(posProj);
            Vector3 dir = pos - target.position;

            if (Physics.Raycast(target.position, dir, out RaycastHit hit, dir.magnitude, layer))
                pos = hit.point;

            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * positionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }
    }
}