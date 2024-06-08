using NFramework;
using System.Collections.Generic;
using UnityEngine;

namespace TenCrush
{
    [System.Serializable]
    public class DailyRewardData
    {
        public int day;
        public List<RewardData> rewards;

        [HideInInspector] public string rewardString;
    }

    [CreateAssetMenu(menuName = "ScriptableObject/DailyRewardConfigSO")]
    public class DailyRewardConfigSO : GoogleSheetConfigSO<DailyRewardData>
    {
        private Dictionary<int, DailyRewardData> _dailyRewardConfigDic;
        public Dictionary<int, DailyRewardData> DailyRewardConfigDic => _dailyRewardConfigDic;

        public void Init()
        {
            _dailyRewardConfigDic = new Dictionary<int, DailyRewardData>();

            foreach (var data in _datas)
            {
                _dailyRewardConfigDic[data.day] = data;
            }
        }

        public DailyRewardData GetDailyRewardData(int day) => _dailyRewardConfigDic[day];

#if UNITY_EDITOR
        protected override void OnSynced(List<DailyRewardData> googleSheetData)
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
