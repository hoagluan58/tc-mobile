using NFramework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class ResultMenu : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _txtLevel;
        [SerializeField] private Button _btnHome;
        [SerializeField] private Button _btnNextLevel;

        private void Awake()
        {
            _btnHome.onClick.AddListener(OnHomeButtonClicked);
            _btnNextLevel.onClick.AddListener(OnPlayButtonClicked);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _txtLevel.text = $"Level {UserData.I.CurrentLevel - 1}";
        }

        private void OnHomeButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            MainManager.I.BackToHomeMenu();
        }

        private void OnPlayButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
            GameManager.I.PlayLevel(UserData.I.CurrentLevel);
        }
    }
}
