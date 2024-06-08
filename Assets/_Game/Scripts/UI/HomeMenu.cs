using NFramework;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class HomeMenu : BaseUIView
    {
        [SerializeField] private Button _btnNoAds;
        [SerializeField] private Button _btnPlay;
        [SerializeField] private Button _btnSetting;
        [SerializeField] private TextMeshProUGUI _txtLevel;

        private void Awake()
        {
            _btnPlay.onClick.AddListener(OnButtonPlayClicked);
            _btnNoAds.onClick.AddListener(OnButtonNoAdsClicked);
            _btnSetting.onClick.AddListener(OnButtonSettingClicked);
        }

        private void OnEnable() => UserData.OnUserLevelChanged += UserData_OnUserLevelChanged;

        private void OnDisable() => UserData.OnUserLevelChanged -= UserData_OnUserLevelChanged;

        public override void OnOpen()
        {
            base.OnOpen();
            UpdatePlayButtonText();
            StartCoroutine(CRShowPopup());
            GameAds.I.ShowBanner();
        }

        private void OnButtonPlayClicked()
        {
            CloseSelf();
            GameSound.I.PlayButtonClickSFX();
            GameManager.I.PlayLevel(UserData.I.CurrentLevel);
        }

        private void OnButtonNoAdsClicked()
        {
            GameSound.I.PlayButtonClickSFX();
        }

        private void OnButtonSettingClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            UIManager.I.Open(Define.UIName.SETTING_POPUP);
        }

        private void UserData_OnUserLevelChanged() => UpdatePlayButtonText();

        private void UpdatePlayButtonText() => _txtLevel.text = $"Level {UserData.I.CurrentLevel}";

        private IEnumerator CRShowPopup()
        {
            yield return CRShowDailyRewardPopup();
        }

        private IEnumerator CRShowDailyRewardPopup()
        {
            if (DailyRewardManager.I.CanShowDailyRewardPopup())
            {
                var popup = UIManager.I.Open(Define.UIName.DAILY_REWARD_POPUP);
                yield return new WaitUntil(() => popup == null || popup.gameObject.activeSelf == false);
            }
        }
    }
}
