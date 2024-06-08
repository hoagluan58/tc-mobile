using NFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class TutorialCompletePopup : BaseUIView
    {
        [SerializeField] private Button _btnContinue;
        [SerializeField] private Button _btnRestart;

        private void Awake()
        {
            _btnContinue.onClick.AddListener(OnButtonContinueClicked);
            _btnRestart.onClick.AddListener(OnButtonRestartClicked);
        }

        private void OnButtonContinueClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
        }

        private void OnButtonRestartClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
            UIManager.I.Open(Define.UIName.TUTORIAL_POPUP);
        }
    }
}
