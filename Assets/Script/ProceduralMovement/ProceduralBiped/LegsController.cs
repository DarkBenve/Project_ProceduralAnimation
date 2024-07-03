using System;
using UnityEngine;

namespace Script.ProceduralMovement
{
    public class LegsController : MonoBehaviour
    {
        [SerializeField] private LegStepper legStepperLeft;
        [SerializeField] private LegStepper legStepperRight;

        private void LateUpdate()
        {
            if (legStepperLeft.isStepping || legStepperRight.isStepping)
            {
                return;
            }

            if (legStepperLeft.TryStep())
            {
                return;
            }

            legStepperRight.TryStep();
        }
    }
}