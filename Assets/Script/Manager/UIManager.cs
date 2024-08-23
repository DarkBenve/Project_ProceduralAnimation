using System;
using Script.CharacterMovement;
using Script.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [SerializeField] private TextMeshProUGUI currentTextKill;
        [SerializeField] private TextMeshProUGUI scoreTimerText;
        [SerializeField] private Button buttonRestartGame;
        [SerializeField] private GameObject panelFinishGame;

        private ButtonManager _buttonManager;
        
        private int _currentKill;
        private Timer _timer;

        protected override void Awake()
        {
            base.Awake();
            _currentKill = 0;
            ChangeTextKill();
            _timer = new Timer(scoreTimerText, GameManager.Instance.timerMax);
            _buttonManager = new ButtonManager(buttonRestartGame);
            _timer.FinishGame += FinishGameOpenPanel;
            _timer.FinishGame += OffGameUI;
        }

        private void Start()
        {
            _timer.StartTimer();
        }

        private void Update()
        {
            _timer.UpdateTimer();
        }

        public void AddKill()
        {
            _currentKill += 1;
            ChangeTextKill();
        }

        private void OffGameUI()
        {
            currentTextKill.gameObject.SetActive(false);
            scoreTimerText.gameObject.SetActive(false);
        }

        private void ChangeTextKill()
        {
            currentTextKill.text = "" + _currentKill;
        }

        private void FinishGameOpenPanel()
        {
            GameManager.Instance.player.enabled = false;
            panelFinishGame.SetActive(true);
        }
    }
}