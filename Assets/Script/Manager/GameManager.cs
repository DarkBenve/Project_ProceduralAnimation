using System;
using Script.CharacterMovement.Camera;
using Script.CharacterMovement.Controller;
using Script.Extension;
using Script.Generator;
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
            Started += SpawnerEnemy.Instance.InitSpawnEnemy;
        }

        private void Start()
        {
            Started?.Invoke();
        }
    }
}