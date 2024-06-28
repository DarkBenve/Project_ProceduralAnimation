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

            var direction = new Vector3(movementHorizontal, 0, movementVertical);

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
    
                // Imposta la rotazione del personaggio verso la rotazione calcolata
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * speedRotation);
            }

            transform.position += direction.normalized * (speed * Time.fixedDeltaTime);
            // if (!IsTouchGround())
            // {
            //     _rigidbody.AddForce(Vector3.down * gravityForce, ForceMode.Force);
            // }
            
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