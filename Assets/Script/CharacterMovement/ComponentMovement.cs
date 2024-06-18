using System;
using UnityEngine;

namespace Script.CharacterMovement
{
    public enum ControllerSwitch
    {
        Keyboard,
        Controller
    }

    public class ComponentMovement : MonoBehaviour
    {
        [Header("Setting")] [SerializeField] private ControllerSwitch selectController;
        [SerializeField] private LayerMask layerGround;
        [SerializeField] private float speed = 6;
        [SerializeField] private float speedRotation = 700;
        [SerializeField] private float gravityForce = 700;
        [Header("Reference")] [SerializeField] private Transform cameraTransform;

        private string[] _currentAxis;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _currentAxis = SelectController();
        }

        private void Update()
        {
            //If not change controller in realtime off this
            _currentAxis = SelectController();
        }

        private void FixedUpdate()
        {
            float movementHorizontal = Input.GetAxis(_currentAxis[0]);
            float movementVertical = Input.GetAxis(_currentAxis[1]);

            Vector3 movement = new Vector3(movementHorizontal, 0.0f, movementVertical).normalized *
                               (speed * Time.fixedDeltaTime);
            movement = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movement;
            if (movement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref speedRotation, 0.1f);

                _rigidbody.rotation = Quaternion.Euler(0, angle, 0);

                _rigidbody.MovePosition(_rigidbody.position + movement);
            }
            else
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }

            if (!IsTouchGround())
            {
                _rigidbody.AddForce(Vector3.down * gravityForce, ForceMode.Force);
            }

            var position = transform.position;
            Debug.DrawRay(new Vector3(position.x, position.y + 0.9f, position.z), Vector3.down, Color.blue, 0.5f);
        }

        private string[] SelectController()
        {
            string[] axis = new string[2];
            switch (selectController)
            {
                case ControllerSwitch.Keyboard:
                    axis[0] = "HorizontalKeyboard";
                    axis[1] = "VerticalKeyboard";
                    break;
                case ControllerSwitch.Controller:
                    axis[0] = "Horizontal";
                    axis[1] = "Vertical";
                    break;
            }

            return axis;
        }

        private bool IsTouchGround()
        {
            var position = transform.position;
            return Physics.Raycast(new Vector3(position.x, position.y + 0.9f, position.z), Vector3.down, 1, layerGround);
        }
    }
}