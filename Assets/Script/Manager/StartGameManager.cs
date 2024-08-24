using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class StartGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject panelStartGame;
        [SerializeField] private Button startGameButton;

        private ButtonManager _buttonManager;

        private void Awake()
        {
            panelStartGame.SetActive(false);
            _buttonManager = new ButtonManager(null, startGameButton);
        }


        public void FinishAnimation()
        {
            panelStartGame.SetActive(true);
        }
    }
}