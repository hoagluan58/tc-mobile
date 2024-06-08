using Falcon.FalconGoogleUMP;
using NFramework.Ads;
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
        [SerializeField] private Button _btnEditConsent;
        [SerializeField] private TextMeshProUGUI _txtVersion;
        [SerializeField] private GameObject _consentOn;
        [SerializeField] private GameObject _consentOff;
        [SerializeField] private GameObject _consentGroup;

        private void Awake()
        {
            _btnClose.onClick.AddListener(OnButtonCloseClicked);
            _btnTerms.onClick.AddListener(OnButtonTermsClicked);
            _btnPolicy.onClick.AddListener(OnButtonPolicyClicked);
            _btnEditConsent.onClick.AddListener(OnButtonEditConsentClicked);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            UpdateVersionText();
            AdsManager.OnConsentStatusChanged += AdsManager_OnConsentStatusChanged;
            _consentGroup.SetActive(FalconUMP.RequirePrivacyOptionsForm);
        }

        public override void OnClose()
        {
            base.OnClose();
            AdsManager.OnConsentStatusChanged -= AdsManager_OnConsentStatusChanged;
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

        private void OnButtonEditConsentClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            FalconUMP.ShowPrivacyOptionsForm();
        }

        private void UpdateVersionText() => _txtVersion.text = $"Version: {Application.version}";

        private void AdsManager_OnConsentStatusChanged(EConsentStatus status)
        {
            _consentOn.SetActive(status == EConsentStatus.Yes);
            _consentOff.SetActive(status != EConsentStatus.Yes);
        }
    }
}
