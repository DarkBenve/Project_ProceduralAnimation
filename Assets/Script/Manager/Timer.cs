using System;
using TMPro;
using UnityEngine;

namespace Script.Manager
{
    public class Timer
    {
        public event Action FinishGame;
        private TextMeshProUGUI _timerText;
        private readonly float _maxTime;
        private float _elapsedTimer;
        
        public Timer(TextMeshProUGUI timerTextMeshPro, float timeMax)
        {
            _timerText = timerTextMeshPro;
            _maxTime = timeMax;
        }

        public void StartTimer()
        {
            _elapsedTimer = _maxTime;
            _timerText.text = "00:00";
        }

        public void UpdateTimer()
        {
            if (_elapsedTimer <= 0.01)
            {
                _elapsedTimer = 0;
                FinishGame?.Invoke();
                return;
            }
            _elapsedTimer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(_elapsedTimer / 60);
            int seconds = Mathf.FloorToInt(_elapsedTimer % 60);
            _timerText.text = $"{minutes:00} : {seconds:00}";
        }
    }
}