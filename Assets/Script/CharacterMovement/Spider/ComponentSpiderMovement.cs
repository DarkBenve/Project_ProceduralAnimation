using System;
using Script.Extension;
using UnityEngine;

namespace Script.CharacterMovement
{
    public class ComponentSpiderMovement : MonoBehaviour
    {
        [SerializeField] private Controller.Controller controller;
        [SerializeField] private float accelerationForward = 40;
        [SerializeField] private float accelerationBack = 40;
        [SerializeField] private float accelerationSide = 40;
        [SerializeField] private float friction = 5;
        [SerializeField] private float accelerationShiftClick = 2;
        [SerializeField] private Vector2 addVelocity = Vector2.zero;
        private Vector2 _velocityNoAdd = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;
        private float _speed = 0;
        private float _maxSpeedEstimation;
        private float _speedProgress;
        private bool _isShiftPress;
        
        [SerializeField] float rotationSpeed = 90;

        [SerializeField, Range(0, 360)] private float arcAngle = 270;
        [SerializeField] private int arcResolution = 6;
        [SerializeField] private LayerMask arcLayer;
        [SerializeField] private Transform arcTransformRotation;


        public Controller.Controller Controller
        {
            get => controller;
        }

        public Vector2 VelocityNoAdd
        {
            get => VelocityNoAdd;
            set
            {
                _velocityNoAdd = value;
                UpdateVelocity();
            }
        }

        public Vector2 Velocity
        {
            get => _velocity;
        }

        public Vector3 Velocity3
        {
            get => new Vector3(_velocity.x, 0, _velocity.y);
        }

        public float Speed
        {
            get => _speed;
        }

        public float SpeedProgress
        {
            get => _speedProgress;
        }


        void OnValidate()
        {
            EstimateMaxSpeed();
        }

        void Awake()
        {
            EstimateMaxSpeed();
        }


        void OnDisable()
        {
            _velocityNoAdd = Vector3.zero;
            UpdateVelocity();
        }

        void Update()
        {
            _isShiftPress = controller.Shift.IsPressed;
            ApplyVelocity();
            Rotate();
        }

        void FixedUpdate()
        {
            ApplyAcceleration();
            ApplyFriction();
            UpdateVelocity();
        }

        void EstimateMaxSpeed()
        {
            // forward
            float v = 0, s;

            for (float t = 0; t < 10; t += Time.fixedDeltaTime)
            {
                v += Time.fixedDeltaTime * accelerationForward;
                v -= Time.fixedDeltaTime * friction * v;
            }

            v += addVelocity.y;
            s = Mathf.Abs(v);

            _maxSpeedEstimation = s;

            // back
            v = 0;

            for (float t = 0; t < 10; t += Time.fixedDeltaTime)
            {
                v -= Time.fixedDeltaTime * accelerationBack;
                v -= Time.fixedDeltaTime * friction * v;
            }

            v += addVelocity.y;
            s = Mathf.Abs(v);

            _maxSpeedEstimation = Mathf.Max(_maxSpeedEstimation, s);

            // side
            v = 0;

            for (float t = 0; t < 10; t += Time.fixedDeltaTime)
            {
                v += Time.fixedDeltaTime * accelerationSide;
                v -= Time.fixedDeltaTime * friction * v;
            }

            v += addVelocity.x * (addVelocity.x > 0 == v > 0 ? 1 : -1);
            s = Mathf.Abs(v);

            _maxSpeedEstimation = Mathf.Max(_maxSpeedEstimation, s);
        }

        void ApplyAcceleration()
        {
            if (!controller)
                return;

            Vector2 stickL = controller.StickL;

            if (stickL != Vector2.zero)
                _velocityNoAdd += Time.fixedDeltaTime *
                                 new Vector2(accelerationSide, stickL.y > 0 ? accelerationForward : accelerationBack) *
                                 stickL;
        }

        void ApplyFriction()
        {
            _velocityNoAdd -= Time.fixedDeltaTime * friction * _velocityNoAdd;
        }

        void UpdateVelocity()
        {
            _velocity = _velocityNoAdd + addVelocity;
            UpdateSpeed();
        }

        void UpdateSpeed()
        {
            if (!_isShiftPress)
            {
                _speed = _velocity.magnitude;
                _speedProgress = Mathf.Clamp01(_speed / _maxSpeedEstimation);
            }
            else
            {
                _speed = _velocity.magnitude * accelerationShiftClick;
                _speedProgress = Mathf.Clamp01(_speed / _maxSpeedEstimation);
            }
        }

        void ApplyVelocity()
        {
            if (_velocity == Vector2.zero)
                return;

            float arcRadius = _speed * Time.deltaTime;
            Vector3 worldVelocity = arcTransformRotation.TransformVector(Velocity3);

            if (PhysicsExtension.ArcCast(transform.position,
                    Quaternion.LookRotation(worldVelocity, arcTransformRotation.up), arcAngle, arcRadius, arcResolution,
                    arcLayer, out RaycastHit hit))
            {
                transform.position = hit.point;
                transform.MatchUp(hit.normal);
            }
        }

        void Rotate()
        {
            if (!controller)
                return;

            Vector2 stickR = controller.StickR;

            if (stickR.x != 0)
                transform.Rotate(0, rotationSpeed * Time.deltaTime * stickR.x, 0);
        }
    }
}