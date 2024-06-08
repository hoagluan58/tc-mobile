using NFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class ShopPopup : BaseUIView
    {
        [SerializeField] private Button _btnBack;
        [SerializeField] private Button _btnAdsBundle;
        [SerializeField] private TextMeshProUGUI _txtAdsBundleRewardAmount;
        [SerializeField] private GameObject _goAdsBundle;
        [SerializeField] private RectTransform _rectLayout;

        private void Awake()
        {
            _btnBack.onClick.AddListener(OnButtonBackClicked);
            _btnAdsBundle.onClick.AddListener(OnButtonAdsBundleClicked);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            Init();
        }

        private void OnButtonAdsBundleClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            var reward = ShopManager.I.GetAdsBundleReward();
            GameAds.I.ShowReward((result) =>
            {
                if (result)
                {
                    ShopManager.I.ClaimAdsBundle();
                    UIManager.I.Open<RewardPopup>(Define.UIName.REWARD_POPUP).Init(reward, RefreshAdsBundle);
                }
            }, Identifier, reward.rewardType.ToString());
        }

        private void OnButtonBackClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
        }

        private void Init() => RefreshAdsBundle();

        private void RefreshAdsBundle()
        {
            _goAdsBundle.SetActive(ShopManager.I.IsAdsBundleReady());
            _txtAdsBundleRewardAmount.text = $"{ShopManager.ADS_BUNDLE_REWARD_AMOUNT}";
        }
    }
}
