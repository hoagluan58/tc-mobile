#if USE_ADJUST_ANALYTICS
using com.adjust.sdk;
#endif
using NFramework;
using NFramework.Ads;
using NFramework.IAP;
using NFramework.Tracking;
using System;
using System.Collections.Generic;
using System.Data;

namespace TenCrush
{
    public class GameTracking : SingletonMono<GameTracking>, IAdsCallbackListener
    {
        public void Init()
        {
            TrackingManager.I.Init();
            IAPManager.OnPurchased += IAPManager_OnPurchased;
        }

        private void IAPManager_OnPurchased(UnityEngine.Purchasing.Product product, string location)
        {
            var iapRevenueData = new IAPRevenueData(product.definition.id, product.metadata.isoCurrencyCode, product.metadata.localizedPrice,
          product.transactionID, "", location);
            TrackingManager.I.TrackIAP(Define.AdjustEventToken.AJ_PURCHASE, iapRevenueData, ETrackingAdapterType.Adjust);
            //if (TrackingManager.I.TryGetAdapter(ETrackingAdapterType.Adjust, out var adapter))
            //{
            //    var adjustAdapter = adapter as AdjustAnalyticsAdapter;
            //}

            Falcon.DWHLog.Log.InAppLog(product.definition.id, product.metadata.isoCurrencyCode, product.metadata.localizedPrice,
                product.transactionID, "", location);
        }

        public void TrackLevelStart()
        {
            var parameters = new Dictionary<string, object>
            {
                { "level", $"{UserData.I.CurrentLevel}" },
                { "current_coin", $"{UserData.I.Coin}" }
            };
            TrackingManager.I.TrackEvent("level_start", parameters, ETrackingAdapterType.FirebaseAnalytics);
            TrackingManager.I.TrackEvent(Define.AdjustEventToken.AJ_LEVEL_START, ETrackingAdapterType.Adjust);
        }

        public void TrackLevelWin(TimeSpan duration)
        {
            var parameters = new Dictionary<string, object>
            {
                { "level", $"{UserData.I.CurrentLevel}" },
                { "timeplayed", $"{duration.TotalSeconds}" }
            };
            TrackingManager.I.TrackEvent("level_complete", parameters, ETrackingAdapterType.FirebaseAnalytics);
            TrackingManager.I.TrackEvent(Define.AdjustEventToken.AJ_LEVEL_ACHIEVED, ETrackingAdapterType.Adjust);
            Falcon.DWHLog.Log.LevelLog(UserData.I.CurrentLevel, duration, 0, "normal", Falcon.FalconAnalytics.Scripts.Enum.LevelStatus.Pass);
        }

        public void TrackLevelLose(TimeSpan duration, int failCount)
        {
            var parameters = new Dictionary<string, object>
            {
                { "level", $"{UserData.I.CurrentLevel}" },
                { "failcount", $"{failCount}" }
            };
            TrackingManager.I.TrackEvent("level_fail", parameters, ETrackingAdapterType.FirebaseAnalytics);
            TrackingManager.I.TrackEvent(Define.AdjustEventToken.AJ_LEVEL_FAIL, ETrackingAdapterType.Adjust);
            Falcon.DWHLog.Log.LevelLog(UserData.I.CurrentLevel, duration, 0, "normal", Falcon.FalconAnalytics.Scripts.Enum.LevelStatus.Fail);
        }

        public void TrackEarnVirtualCurrency(int amount, string generalReason = "unknown", string specificReason = "unknown")
        {
            var currencyName = "coin";
            var parameters = new Dictionary<string, object>
            {
                { "virtual_currency_name", currencyName },
                { "current_coin", (long)amount },
                { "source", specificReason }
            };
            TrackingManager.I.TrackEvent("earn_virtual_currency", parameters, ETrackingAdapterType.FirebaseAnalytics);
            Falcon.DWHLog.Log.ResourceLog(UserData.I.CurrentLevel, Falcon.FalconAnalytics.Scripts.Enum.FlowType.Source, generalReason,
                specificReason, currencyName, amount);
        }

        public void TrackSpendVirtualCurrency(int amount, string generalReason = "unknown", string specificReason = "unknown")
        {
            var currencyName = "coin";
            var parameters = new Dictionary<string, object>
            {
                { "spend_virtual_currency", currencyName },
                { "current_coin", (long)amount },
                { "item_name", specificReason }
            };
            TrackingManager.I.TrackEvent("spend_virtual_currency", parameters, ETrackingAdapterType.FirebaseAnalytics);
            Falcon.DWHLog.Log.ResourceLog(UserData.I.CurrentLevel, Falcon.FalconAnalytics.Scripts.Enum.FlowType.Sink, generalReason,
                specificReason, currencyName, amount);
        }

        #region IAdsCallbackListener
        public void OnAdsRevenuePaid(AdsRevenueData data)
        {
            TrackingManager.I.TrackAdImpression("", data, ETrackingAdapterType.FirebaseAnalytics);
            TrackingManager.I.TrackAdImpression("", data, ETrackingAdapterType.Adjust);
        }

        public void OnRewardLoaded() => TrackingManager.I.TrackEvent("ads_reward_load", ETrackingAdapterType.FirebaseAnalytics);

        public void OnRewardClicked() => TrackingManager.I.TrackEvent("ads_reward_click", ETrackingAdapterType.FirebaseAnalytics);

        public void OnRewardDisplayed()
        {
            TrackingManager.I.TrackEvent("ads_reward_show_success", ETrackingAdapterType.FirebaseAnalytics);
            TrackingManager.I.TrackEvent(Define.AdjustEventToken.AJ_REWARDED_DISPLAYED, ETrackingAdapterType.Adjust);
        }

        public void OnRewardDisplayFailed() => TrackingManager.I.TrackEvent("ads_reward_show_fail", ETrackingAdapterType.FirebaseAnalytics);

        public void OnRewardRecieved() => TrackingManager.I.TrackEvent("ads_reward_complete", ETrackingAdapterType.FirebaseAnalytics);

        public void OnInterLoadFailed() => TrackingManager.I.TrackEvent("ad_inter_load_fail", ETrackingAdapterType.FirebaseAnalytics);

        public void OnInterLoaded() => TrackingManager.I.TrackEvent("ad_inter_load_success", ETrackingAdapterType.FirebaseAnalytics);

        public void OnInterDisplayed()
        {
            TrackingManager.I.TrackEvent("ad_inter_show", ETrackingAdapterType.FirebaseAnalytics);
            TrackingManager.I.TrackEvent(Define.AdjustEventToken.AJ_INTERS_DISPLAYED, ETrackingAdapterType.Adjust);
        }

        public void OnInterClicked() => TrackingManager.I.TrackEvent("ad_inter_click", ETrackingAdapterType.FirebaseAnalytics);

        public void OnRewardLoadFailed() { }

        public void OnAOADisplayed() { }

        public void OnRequestShowReward() => TrackingManager.I.TrackEvent(Define.AdjustEventToken.AJ_REWARDED_SHOW, ETrackingAdapterType.Adjust);

        public void OnRequestShowInter() => TrackingManager.I.TrackEvent(Define.AdjustEventToken.AJ_INTERS_SHOW, ETrackingAdapterType.Adjust);
        #endregion
    }
}
