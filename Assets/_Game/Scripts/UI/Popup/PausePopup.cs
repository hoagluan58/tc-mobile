using NFramework;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class PausePopup : BasePopup
    {
        [SerializeField] private Button _btnHome;
        [SerializeField] private Button _btnContinue;
        [SerializeField] private Button _btnRetry;
        [SerializeField] private Button _btnClose;

        private void Awake()
        {
            _btnHome.onClick.AddListener(OnButtonHomeClicked);
            _btnContinue.onClick.AddListener(OnButtonContinueClicked);
            _btnRetry.onClick.AddListener(OnButtonRetryClicked);
            _btnClose.onClick.AddListener(OnButtonCloseClicked);
        }

        private void OnButtonCloseClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
        }

        private void OnButtonRetryClicked()
        {
            CloseSelf();
            GameSound.I.PlayButtonClickSFX();
            UIManager.I.Close(Define.UIName.GAME_MENU);
            GameAds.I.ShowInter(null, Identifier);
            GameManager.I.PlayLevel(UserData.I.CurrentLevel);
        }

        private void OnButtonContinueClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
        }

        private void OnButtonHomeClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            GameAds.I.ShowInter(null, Identifier);
            MainManager.I.BackToHomeMenu();
        }
    }
}
