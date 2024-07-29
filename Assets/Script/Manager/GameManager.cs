using System;
using Script.CharacterMovement.Controller;
using Script.Extension;
using UnityEngine;

namespace Script.Manager
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        public event Action Started;

        private void Start()
        {
            Started?.Invoke();
        }
    }
}