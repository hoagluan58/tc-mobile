using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class X2RewardPopup : RewardPopup
    {
        [SerializeField] private Button _btnX2Reward;
        [SerializeField] private Button _btnCool;

        private Action<bool> _callback;

        private void Awake()
        {
            _btnX2Reward.onClick.AddListener(OnButtonX2RewardClicked);
            _btnCool.onClick.AddListener(OnButtonCoolClicked);
        }

        public void Init(List<RewardData> rewards, Action<bool> callbackWatchAds)
        {
            base.Init(rewards, null);
            _callback = callbackWatchAds;
        }

        private void OnButtonX2RewardClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            GameAds.I.ShowReward((result) =>
            {
                if (result)
                {
                    StartCoroutine(CRShowFlyRewardAnimation(_callback, true));
                }
            });
        }

        private void OnButtonCoolClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            StartCoroutine(CRShowFlyRewardAnimation(_callback, false));
        }
    }
}
