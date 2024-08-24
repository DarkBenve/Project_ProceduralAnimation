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
        [SerializeField] private TextMeshProUGUI currentTextKillFinal;
        [SerializeField] private TextMeshProUGUI scoreTimerText;
        [SerializeField] private Button buttonRestartGame;
        [SerializeField] private GameObject panelFinishGame;
        [SerializeField] private GameObject panelTryToShoot;

        private ButtonManager _buttonManager;

        private bool _isTryToShootSignalOpen;
        private int _currentKill;
        private Timer _timer;

        protected override void Awake()
        {
            base.Awake();
            _currentKill = 0;
            ChangeTextKill();
            _timer = new Timer(scoreTimerText, GameManager.Instance.timerMax);
            _buttonManager = new ButtonManager(buttonRestartGame, null);
            _timer.FinishGame += FinishGameOpenPanel;
            _timer.FinishGame += OffGameUI;
            _timer.FinishGame += UpdateScoreFinal;
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
            OffToShootSignal();
        }

        public void TryToShootSignal()
        {
            panelTryToShoot.SetActive(true);
        }
        
        public void OffToShootSignal()
        {
            panelTryToShoot.SetActive(false);
        }

        private void UpdateScoreFinal()
        {
            currentTextKillFinal.text = "Score: " + _currentKill;
        }

        private void OffGameUI()
        {
            Image killPanel = currentTextKill.GetComponentInParent<Image>();
            Image scoreTimerPanel = scoreTimerText.GetComponentInParent<Image>();
            killPanel.gameObject.SetActive(false);
            scoreTimerPanel.gameObject.SetActive(false);
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