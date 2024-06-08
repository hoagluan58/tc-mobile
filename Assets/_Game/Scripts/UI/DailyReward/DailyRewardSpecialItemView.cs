using NFramework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class DailyRewardSpecialItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtDay;
        [SerializeField] private List<RewardItemView> _rewardItemViews;
        [SerializeField] private Button _btnClaim;

        private DailyRewardData _data;

        private void Awake() => _btnClaim.onClick.AddListener(OnButtonClaimClicked);

        private void OnClaimRewardCompleted(bool isClaimAds)
        {
            if (UIManager.I.IsSpecificViewShown(Define.UIName.DAILY_REWARD_POPUP, out var view))
            {
                UIManager.I.Close(view);
                DailyRewardManager.I.ClaimReward(_data.day, isClaimAds);
            }
        }

        private void OnButtonClaimClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            if (DailyRewardManager.I.CanClaim(_data.day))
            {
                UIManager.I.Open<X2RewardPopup>(Define.UIName.X2_REWARD_POPUP).Init(_data.rewards, OnClaimRewardCompleted);
            }
        }

        public void Init(DailyRewardData data)
        {
            _data = data;
            _txtDay.text = $"Day {_data.day}";
            _rewardItemViews.ForEach(x => x.gameObject.SetActive(false));

            for (var i = 0; i < _data.rewards.Count; i++)
            {
                _rewardItemViews[i].gameObject.SetActive(true);
                _rewardItemViews[i].Init(_data.rewards[i]);
            }
        }
    }
}
