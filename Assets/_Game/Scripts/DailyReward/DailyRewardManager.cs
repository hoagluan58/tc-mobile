using NFramework;
using UnityEngine;

namespace TenCrush
{
    public class DailyRewardManager : SingletonMono<DailyRewardManager>, ISaveable
    {
        private const int MAX_CLAIM_DAY = 7;

        [SerializeField] private DailyRewardConfigSO _config;
        [SerializeField] private SaveData _saveData;

        public DailyRewardConfigSO Config { get => _config; }

        public void Init() => _config.Init();

        public void ClaimReward(int day, bool isClaimAds)
        {
            var data = _config.GetDailyRewardData(day);
            UserData.I.AddRewardDataToUserData(data.rewards, isClaimAds ? 2 : 1);
            _saveData.lastClaimDay = day;
            DataChanged = true;
        }

        public bool CanShowDailyRewardPopup() => UserData.I.CheckNewDay();

        public bool IsDailyRewardClaimed(int day) => day <= _saveData.lastClaimDay;

        public bool IsClaimAllDaily() => _saveData.lastClaimDay == MAX_CLAIM_DAY;

        public bool CanClaim(int day) => day == _saveData.lastClaimDay + 1;

        public void ResetDailyReward()
        {
            _saveData.lastClaimDay = 0;
            DataChanged = true;
        }

        #region ISaveable
        [System.Serializable]
        public class SaveData
        {
            public int lastClaimDay;
        }

        public string SaveKey => "DailyRewardManager";

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

