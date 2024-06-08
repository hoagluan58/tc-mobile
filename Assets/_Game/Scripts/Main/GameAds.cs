using NFramework;
using NFramework.Ads;
using System;
using UnityEngine;

namespace TenCrush
{
    public class GameAds : SingletonMono<GameAds>
    {
        public const float INTER_CAPPING_TIME = 60f;

        private float _lastShowInterTime = float.MinValue;
        private int _callCount = 0;

        private void OnApplicationPause(bool pause)
        {
            if (Time.realtimeSinceStartup > 5f && !pause && !UIManager.I.IsSpecificViewShown(Define.UIName.LOADING_UI, out var view))
                AdsManager.I.ShowAOA();
        }

        public void ShowReward(Action<bool> callback, string location = "", string rewardType = "")
        {
            if (!AdsManager.I.IsRewardReady())
            {
                callback?.Invoke(false);
                UIManager.I.Open<NoticePopup>(Define.UIName.NOTICE_POPUP).Init("Ad is not available right now!");
                return;
            }

            AdsManager.I.ShowReward(new AdsShowData(callback, location, null, rewardType));
        }

        public void ShowInter(Action<bool> callback, string location = "")
        {
            _callCount++;
            if (_callCount < 2 || Time.realtimeSinceStartup < _lastShowInterTime + INTER_CAPPING_TIME)
            {
                callback?.Invoke(false);
                return;
            }

            callback += result =>
            {
                if (result)
                    _lastShowInterTime = Time.realtimeSinceStartup;
            };

            AdsManager.I.ShowInter(new AdsShowData(callback, location));
        }

        public void ShowBanner()
        {
            AdsManager.I.ShowBanner();
        }
    }
}
