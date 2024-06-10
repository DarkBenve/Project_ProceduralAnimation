using System;
using System.Collections.Generic;
using cmdwtf.UnityTools.Dynamics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Test_MoveProceduralStudy
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private List<DynamicsTransform> dynamicsTransform;

        private void Start()
        {
            foreach (var dynamics in dynamicsTransform)
            {
                dynamics.SetTarget(gameObject.transform);
            }
        }
    }
}