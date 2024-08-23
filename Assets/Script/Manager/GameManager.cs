using System;
using Script.CharacterMovement;
using Script.CharacterMovement.Camera;
using Script.CharacterMovement.Controller;
using Script.Extension;
using Script.Generator;
using TMPro;
using UnityEngine;

namespace Script.Manager
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private CameraFollow cameraFollow;
        public float timerMax;
        public ComponentSpiderMovement player;
        
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