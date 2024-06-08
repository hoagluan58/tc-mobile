using NFramework;
using UnityEngine;

namespace TenCrush
{
    public enum ERewardType
    {
        Booster,
        Currency,
    }

    public enum ECurrencyType
    {
        Coin,
    }

    [System.Serializable]
    public class RewardData
    {
        public ERewardType rewardType;

        [ConditionalField(nameof(rewardType), compareValues: ERewardType.Booster)]
        public EBoosterType boosterType;
        [ConditionalField(nameof(rewardType), compareValues: ERewardType.Currency)]
        public ECurrencyType currencyType;

        public Sprite icon;
        public int amount;
        public string reasonGeneral;
        public string reasonSpecific;

        public RewardData(ERewardType rewardType, EBoosterType boosterType, int amount, bool isCreateFromEditor = false,
            string reasonGeneral = "unknown", string reasonSpecific = "unknown")
        {
            this.rewardType = rewardType;
            this.boosterType = boosterType;
#if UNITY_EDITOR
            this.icon = isCreateFromEditor ? FileUtils.LoadFirstAssetWithName<Sprite>($"Booster_{boosterType}") : GameSpriteManager.I.GetBoosterSprite(boosterType);
#else
            this.icon = GameSpriteManager.I.GetBoosterSprite(boosterType);
#endif
            this.amount = amount;
            this.reasonGeneral = reasonGeneral;
            this.reasonSpecific = reasonSpecific;
        }

        public RewardData(ERewardType rewardType, ECurrencyType currencyType, int amount, bool isCreateFromEditor = false,
            string reasonGeneral = "unknown", string reasonSpecific = "unknown")
        {
            this.rewardType = rewardType;
            this.currencyType = currencyType;
#if UNITY_EDITOR
            this.icon = isCreateFromEditor ? FileUtils.LoadFirstAssetWithName<Sprite>("Coin") : GameSpriteManager.I.GetCurrencySprite(currencyType);
#else
            this.icon = GameSpriteManager.I.GetCurrencySprite(currencyType);
#endif
            this.amount = amount;
            this.reasonGeneral = reasonGeneral;
            this.reasonSpecific = reasonSpecific;
        }
    }
}
