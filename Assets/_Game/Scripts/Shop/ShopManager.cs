using NFramework;
using NFramework.IAP;
using UnityEngine;

namespace TenCrush
{
    public class ShopManager : SingletonMono<ShopManager>, ISaveable
    {
        public const int ADS_BUNDLE_REWARD_AMOUNT = 100;

        [SerializeField] private ShopDataConfigSO _shopConfig;
        [SerializeField] private IAPDataConfigSO _iapDataConfig;
        [SerializeField] private SaveData _saveData;

        public ShopDataConfigSO ShopConfig { get => _shopConfig; }

        public void Init() => _shopConfig.Init();

        public IAPData GetIAPData(string id) => _iapDataConfig.GetIAPData(id);

        public void ClaimPurchaseReward(IAPProductSO productSO)
        {
            var iapData = GetIAPData(productSO.id);
            UserData.I.AddRewardDataToUserData(iapData.rewards);
        }

        public void ClaimAdsBundle()
        {
            var curDay = TimeUtils.GetCurrentTimeSpan().Days;
            UserData.I.AddRewardDataToUserData(GetAdsBundleReward());
            _saveData.lastClaimAdsBundleDay = curDay;
            DataChanged = true;
        }

        public void PurchaseShopItem(string id)
        {
            var data = _shopConfig.GetShopItemData(id);
            UserData.I.SpendCoin(data.price, "shop", "shop");
            UserData.I.AddRewardDataToUserData(data.rewards);
        }

        public bool IsAdsBundleReady()
        {
            var curDay = TimeUtils.GetCurrentTimeSpan().Days;
            var isClaimed = curDay == _saveData.lastClaimAdsBundleDay;
            var isNewDay = UserData.I.CheckNewDay();
            return isNewDay || !isClaimed;
        }

        public RewardData GetAdsBundleReward() => new RewardData(ERewardType.Currency, ECurrencyType.Coin, ADS_BUNDLE_REWARD_AMOUNT);

        #region ISaveable
        [System.Serializable]
        public class SaveData
        {
            public int lastClaimAdsBundleDay;
        }

        public string SaveKey => "ShopManager";

        public bool DataChanged { get; set; }

        public object GetData() => _saveData;

        public void OnAllDataLoaded() { }

        public void SetData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                _saveData = new SaveData();
            }
            else
            {
                _saveData = JsonUtility.FromJson<SaveData>(data);
            }
        }
        #endregion
    }
}
