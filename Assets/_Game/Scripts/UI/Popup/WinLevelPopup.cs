using NFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class WinLevelPopup : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _txtRewardAmount;
        [SerializeField] private Button _btnAdsReward;
        [SerializeField] private Button _btnNextLevel;

        private void Awake()
        {
            _btnAdsReward.onClick.AddListener(OnButtonAdsRewardClicked);
            _btnNextLevel.onClick.AddListener(OnButtonNextLevelClicked);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            GameSound.I.PlaySFX(Define.SoundName.SFX_WIN);
            _txtRewardAmount.text = $"x{Define.WIN_LEVEL_COIN}";
        }

        private void OnButtonAdsRewardClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            GameAds.I.ShowReward((result) =>
            {
                if (result)
                {
                    var multiplyValue = 2;
                    var reward = new RewardData(ERewardType.Currency, ECurrencyType.Coin,
                        Define.WIN_LEVEL_COIN * multiplyValue, false, "level", "levelwinads");
                    UserData.I.AddRewardDataToUserData(reward);
                    CloseSelf();
                    UIManager.I.Open<RewardPopup>(Define.UIName.REWARD_POPUP).Init(reward, () =>
                    {
                        GameManager.I.PlayLevel(UserData.I.CurrentLevel);
                    });
                }
            }, Identifier, "coin");
        }

        private void OnButtonNextLevelClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            var reward = new RewardData(ERewardType.Currency, ECurrencyType.Coin,
                Define.WIN_LEVEL_COIN, false, "level", "levelwin");
            UserData.I.AddRewardDataToUserData(reward);
            CloseSelf();
            UIManager.I.Open<RewardPopup>(Define.UIName.REWARD_POPUP).Init(reward, () =>
            {
                GameManager.I.PlayLevel(UserData.I.CurrentLevel);
            });
        }
    }
}
