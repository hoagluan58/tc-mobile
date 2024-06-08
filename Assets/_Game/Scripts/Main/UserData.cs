using NFramework;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TenCrush
{
    public class UserData : SingletonMono<UserData>, ISaveable
    {
        public static event Action OnUserLevelChanged;
        public static event Action<int> OnUserCoinChanged;
        public static event Action<EBoosterType, int> OnUserBoosterChanged;
        public static event Action OnNewDayChanged;

        [SerializeField] private SaveData _saveData;

        public int CurrentLevel
        {
            get => _saveData.level;
            set
            {
                _saveData.level = value;
                DataChanged = true;
                OnUserLevelChanged?.Invoke();
            }
        }

        public int Coin => _saveData.coin;

        public void AddCoin(int amount, string reasonGeneral, string reasonSpecific)
        {
            _saveData.coin += amount;
            DataChanged = true;
            OnUserCoinChanged?.Invoke(_saveData.coin);
            GameTracking.I.TrackEarnVirtualCurrency(amount, reasonGeneral, reasonSpecific);
        }

        public void SpendCoin(int amount, string reasonGeneral, string reasonSpecific)
        {
            _saveData.coin -= amount;
            DataChanged = true;
            OnUserCoinChanged?.Invoke(_saveData.coin);
            GameTracking.I.TrackSpendVirtualCurrency(amount, reasonGeneral, reasonSpecific);
        }

        public int GetBoosterTypeAmount(EBoosterType type)
        {
            if (_saveData.boosterAmountDic.TryGetValue(type, out var value))
            {
                return value;
            }

            return 0;
        }

        public void ModifyBoosterTypeAmount(EBoosterType type, int amount)
        {
            if (_saveData.boosterAmountDic.ContainsKey(type))
            {
                _saveData.boosterAmountDic[type] += amount;
            }
            else
            {
                _saveData.boosterAmountDic.Add(type, amount);
            }
            OnUserBoosterChanged?.Invoke(type, GetBoosterTypeAmount(type));
            DataChanged = true;
        }

        public void AddRewardDataToUserData(RewardData rewardData, int multiple = 1)
        {
            switch (rewardData.rewardType)
            {
                case ERewardType.Booster:
                    ModifyBoosterTypeAmount(rewardData.boosterType, rewardData.amount * multiple);
                    break;
                case ERewardType.Currency:
                    AddCoin(rewardData.amount * multiple, rewardData.reasonGeneral, rewardData.reasonSpecific);
                    break;
            }
        }

        public void AddRewardDataToUserData(List<RewardData> rewardDatas, int multiple = 1)
        {
            foreach (var reward in rewardDatas)
            {
                AddRewardDataToUserData(reward, multiple);
            }
        }

        public bool NeedForcePlayTutorial()
        {
            if (_saveData.havePlayTutorial)
            {
                return false;
            }
            else
            {
                _saveData.havePlayTutorial = true;
                DataChanged = true;
                return true;
            }
        }

        public bool IsHaveBooster(EBoosterType type) => GetBoosterTypeAmount(type) > 0;

        public bool IsEnoughCoin(int value) => _saveData.coin >= value;

        public bool CheckNewDay()
        {
            var curDay = TimeUtils.GetCurrentTimeSpan().Days;
            if (curDay != _saveData.lastCheckDay)
            {
                if (DailyRewardManager.I.IsClaimAllDaily())
                {
                    DailyRewardManager.I.ResetDailyReward();
                }
                _saveData.lastCheckDay = curDay;
                OnNewDayChanged?.Invoke();
                DataChanged = true;
                return true;
            }
            return false;
        }

        #region ISaveable
        [System.Serializable]
        public class SaveData
        {
            public int level;
            public int coin;
            public SerializableDictionaryBase<EBoosterType, int> boosterAmountDic = new SerializableDictionaryBase<EBoosterType, int>();
            public int lastCheckDay;
            public bool havePlayTutorial;
        }

        public string SaveKey => "UserData";

        public bool DataChanged { get; set; }

        public object GetData() => _saveData;

        public void OnAllDataLoaded() { }

        public void SetData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                _saveData = new SaveData();
                _saveData.level = 1;
                DataChanged = true;
            }
            else
            {
                _saveData = JsonUtility.FromJson<SaveData>(data);
            }
        }
        #endregion
    }

    public enum EBoosterType
    {
        Hint = 0,
        Bomb = 1,
        Hammer = 2,
        Undo = 3,
    }
}
