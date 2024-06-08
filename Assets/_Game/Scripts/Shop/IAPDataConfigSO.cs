using NFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TenCrush
{
    [System.Serializable]
    public class IAPData
    {
        public string id;
        public List<RewardData> rewards;

        [HideInInspector] public string rewardString;
    }

    [CreateAssetMenu(menuName = "ScriptableObject/IAPDataConfigSO")]
    public class IAPDataConfigSO : GoogleSheetConfigSO<IAPData>
    {
        public IAPData GetIAPData(string id) => _datas.FirstOrDefault(x => x.id == id);

#if UNITY_EDITOR
        protected override void OnSynced(List<IAPData> googleSheetData)
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
