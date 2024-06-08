using NFramework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class LoseLevelPopup : BaseUIView
    {
        [SerializeField] private Image _imgBooster;
        [SerializeField] private TextMeshProUGUI _txtBoosterAmount;
        [SerializeField] private Button _btnAds;
        [SerializeField] private Button _btnRetryLevel;

        private RewardData _rewardData;

        private void Awake()
        {
            _btnAds.onClick.AddListener(OnButtonAdsClicked);
            _btnRetryLevel.onClick.AddListener(OnButtonRetryClicked);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            Init();
        }

        private void OnButtonRetryClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
            GameManager.I.PlayLevel(UserData.I.CurrentLevel);
        }

        private void OnButtonAdsClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            GameAds.I.ShowReward((result) =>
            {
                CloseSelf();
                UserData.I.AddRewardDataToUserData(_rewardData);
                UIManager.I.Open<RewardPopup>(Define.UIName.REWARD_POPUP).Init(_rewardData, () =>
                {
                    GameManager.I.PlayLevel(UserData.I.CurrentLevel);
                });
            }, Identifier, _rewardData.rewardType.ToString());
        }

        private void Init()
        {
            GameSound.I.PlaySFX(Define.SoundName.SFX_LOSE);
            _rewardData = new RewardData(ERewardType.Booster, GetRandomBoosterType(), 
                Define.LOSE_BOOSTER_REWARD_AMOUNT, false, "level", "levelfail");
            _txtBoosterAmount.text = $"x{Define.LOSE_BOOSTER_REWARD_AMOUNT}";
            _imgBooster.sprite = _rewardData.icon;
        }

        private EBoosterType GetRandomBoosterType() => (EBoosterType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EBoosterType)).Length);
    }
}
