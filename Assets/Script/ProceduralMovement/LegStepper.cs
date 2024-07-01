using System;
using System.Collections;
using UnityEngine;

namespace Script.ProceduralMovement
{
    public class LegStepper : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private Transform targetControlHandle;
        [SerializeField] private Transform pelvis;
        [Header("Settings")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float horizontalOffset;
        [SerializeField] private float distanceToStep;
        [SerializeField, Range(0f, 0.9f)] private float overStep;
        [SerializeField] private float timeToStep;
        [SerializeField] private AnimationCurve verticalCurve;
        [SerializeField] private AnimationCurve horizontalCurve;
        [SerializeField] private float stepHeight;
        
        public bool isStepping;
        private bool _isInitialized;
        private Vector3 _lastControlHandlePos;
        //private Vector3 _lastNormalPos;


        private void Start()
        {
            _lastControlHandlePos = targetControlHandle.position;
            //_lastNormalPos = targetControlHandle.up;
        }

        private void LateUpdate()
        {
            RefreshPositionHandle();
        }

        private void RefreshPositionHandle()
        {
            targetControlHandle.position = _lastControlHandlePos;
            //targetControlHandle.rotation = Quaternion.LookRotation(pelvis.forward, _lastNormalPos);
        }

       
        public bool TryStep()
        {
            if (!ShouldStep(out var hit))
            {
                return false;
            }

            var move = hit.point - targetControlHandle.position;
            var overStepMove = move * overStep;
            overStepMove = Vector3.ClampMagnitude(overStepMove, distanceToStep * overStep);
            var newPos = hit.point + overStepMove;
            newPos.y = pelvis.position.y;

            var overRay = new Ray(newPos, Vector3.down);
            if (!Physics.Raycast(overRay, out var overLandingHit, 300, groundLayer))
            {
                Step(hit.point, hit.normal);
                _isInitialized = true;
                return true;
            }
            
            //Step
            Step(overLandingHit.point, overLandingHit.normal);
            _isInitialized = true;
            return true;
        }
        
        private void Step(Vector3 landingPos, Vector3 landingNormal)
        {
            StartCoroutine(StepCoroutine(landingPos, landingNormal));
        }

        private IEnumerator StepCoroutine(Vector3 landingPos, Vector3 landingNormal)
        {
            isStepping = true;
            var startingPos = _lastControlHandlePos;
            //var startingNormal = _lastNormalPos;
            var time = 0f;

            while (time < timeToStep)
            {
                time += Time.deltaTime;
                var valueTime = time / timeToStep;

                var verticalOffset = verticalCurve.Evaluate(valueTime) * stepHeight;
                var horizontalValue = horizontalCurve.Evaluate(valueTime);

                var currentPos = Vector3.Lerp(startingPos, landingPos, horizontalValue);
                currentPos.y += verticalOffset;
                _lastControlHandlePos = currentPos;
                //_lastNormalPos = Vector3.Lerp(startingNormal, landingNormal, valueTime);

                yield return null;
            }

            _lastControlHandlePos = landingPos;
            //_lastNormalPos = landingNormal;

            isStepping = false;
        }


        //is method for Check distance 
        private bool ShouldStep(out RaycastHit hit)
        {
            hit = default;
            
            if (isStepping)
                return false;
            
            var rayOrigin = pelvis.position + pelvis.forward.normalized * horizontalOffset;
            var ray = new Ray(rayOrigin, Vector3.down);

            if (!Physics.Raycast(ray, out hit, 300, groundLayer))
            {
                return false;
            }

            var distance = Vector3.Distance(hit.point, targetControlHandle.position);

            if (_isInitialized && distance < distanceToStep)
            {
                return false;
            }

            return true;

        }
    }
}