using NFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenCrush
{
    [System.Serializable]
    public class ShopItemData
    {
        public string id;
        public List<RewardData> rewards;
        public int price;

        [HideInInspector] public string rewardString;
    }

    [CreateAssetMenu(menuName = "ScriptableObject/ShopDataConfigSO")]
    public class ShopDataConfigSO : GoogleSheetConfigSO<ShopItemData>
    {
        private Dictionary<string, ShopItemData> _shopItemDataConfigDic;

        public void Init()
        {
            _shopItemDataConfigDic = new Dictionary<string, ShopItemData>();

            foreach (var data in _datas)
            {
                _shopItemDataConfigDic[data.id] = data;
            }
        }

        public ShopItemData GetShopItemData(string id) => _shopItemDataConfigDic[id];

#if UNITY_EDITOR
        protected override void OnSynced(List<ShopItemData> googleSheetData)
        {
            base.OnSynced(googleSheetData);
            foreach (var data in _datas)
            {
                data.rewards = Utilities.ParseStringToListRewardData(data.rewardString);
            }
        }
#endif
    }
}
