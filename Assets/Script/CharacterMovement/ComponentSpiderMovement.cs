using System;
using UnityEngine;

namespace Script.CharacterMovement
{
    public class ComponentSpiderMovement : MonoBehaviour
    {
        [SerializeField] private float acceleration = 40;
        [SerializeField] private float friction = 5;
        [SerializeField] private float rotationSpeed = 90;

        private Vector2 _velocity = Vector2.zero;
        private float _speed;


        private void Update()
        {
            ApplyAcceleration();
            ApplyFriction();
            UpdateSpeed();
            ApplyVelocity();
            Rotate();
        }


        private void ApplyAcceleration()
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A)) dir.x -= 1;
            if (Input.GetKey(KeyCode.D)) dir.x += 1;
            if (Input.GetKey(KeyCode.S)) dir.y -= 1;
            if (Input.GetKey(KeyCode.W)) dir.y += 1;

            if (dir != Vector2.zero)
            {
                dir.Normalize();
                _velocity += acceleration * Time.deltaTime * dir;
            }
        }

        private void ApplyFriction()
        {
            _velocity -= friction * Time.deltaTime * _velocity;
        }

        private void UpdateSpeed()
        {
            _speed = _velocity.magnitude;
        }

        private void ApplyVelocity()
        {
            transform.Translate(new Vector3(_velocity.x, 0, _velocity.y) * Time.deltaTime);
        }

        private void Rotate()
        {
            float dir = 0;

            if (Input.GetKey(KeyCode.LeftArrow)) dir -= 1;
            if (Input.GetKey(KeyCode.RightArrow)) dir += 1;
            
            transform.Rotate(0, rotationSpeed * Time.deltaTime * dir, 0);
        }
    }
}