using NFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class DailyRewardItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtDay;
        [SerializeField] private RewardItemView _rewardItemView;
        [SerializeField] private Button _btnClaim;
        [SerializeField] private GameObject _goClaimed;
        [SerializeField] private GameObject _vfxHighlight;

        private DailyRewardData _data;

        private void Awake() => _btnClaim.onClick.AddListener(OnButtonClaimClicked);

        private void OnButtonClaimClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            if (DailyRewardManager.I.CanClaim(_data.day))
            {
                UIManager.I.Open<X2RewardPopup>(Define.UIName.X2_REWARD_POPUP).Init(_data.rewards, OnClaimRewardCompleted);
            }
        }

        private void OnClaimRewardCompleted(bool isWatchAds)
        {
            if (UIManager.I.IsSpecificViewShown(Define.UIName.DAILY_REWARD_POPUP, out var view))
            {
                UIManager.I.Close(view);
                DailyRewardManager.I.ClaimReward(_data.day, isWatchAds);
                _vfxHighlight.SetActive(false);
            }
        }

        public void Init(DailyRewardData data)
        {
            _data = data;
            _txtDay.text = $"Day {_data.day}";
            _rewardItemView.Init(_data.rewards);
            _goClaimed.SetActive(DailyRewardManager.I.IsDailyRewardClaimed(_data.day));
            _vfxHighlight.SetActive(DailyRewardManager.I.CanClaim(_data.day));
        }
    }
}
