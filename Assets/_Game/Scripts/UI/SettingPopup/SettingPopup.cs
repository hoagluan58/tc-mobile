using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class SettingPopup : BasePopup
    {
        [SerializeField] private Button _btnClose;
        [SerializeField] private Button _btnTerms;
        [SerializeField] private Button _btnPolicy;
        [SerializeField] private TextMeshProUGUI _txtVersion;

        private void Awake()
        {
            _btnClose.onClick.AddListener(OnButtonCloseClicked);
            _btnTerms.onClick.AddListener(OnButtonTermsClicked);
            _btnPolicy.onClick.AddListener(OnButtonPolicyClicked);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            UpdateVersionText();
        }

        private void OnButtonPolicyClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            Application.OpenURL(Define.PRIVACY_POLICY_URL);
        }

        private void OnButtonTermsClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            Application.OpenURL(Define.TERM_AND_CONDITION_URL);
        }

        private void OnButtonCloseClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
        }

        private void UpdateVersionText() => _txtVersion.text = $"Version: {Application.version}";
    }
}
