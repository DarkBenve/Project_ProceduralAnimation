using Script.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Manager
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [SerializeField] private TextMeshProUGUI currentTextKill;
        private int _currentKill;

        protected override void Awake()
        {
            base.Awake();
            _currentKill = 0;
            ChangeTextKill();
        }

        public void AddKill()
        {
            _currentKill += 1;
            ChangeTextKill();
        }

        private void ChangeTextKill()
        {
            currentTextKill.text = "" + _currentKill;
        }
    }
}