using System;
using Script.CharacterMovement.Camera;
using Script.CharacterMovement.Controller;
using Script.Extension;
using UnityEngine;

namespace Script.Manager
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private CameraFollow cameraFollow;
        public event Action Started;

        protected override void Awake()
        {
            base.Awake();
            Started += cameraFollow.InitCamFollow;
        }

        private void Start()
        {
            Started?.Invoke();
        }
    }
}