using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public static class Utilities
    {
        public static List<RewardData> ParseStringToListRewardData(string dataString)
        {
            var result = new List<RewardData>();

            if (string.IsNullOrEmpty(dataString))
            {
                return result;
            }

            var datas = dataString.Split(";");

            foreach (var data in datas)
            {
                var splitDatas = data.Split("-");
                if (!Enum.TryParse<ERewardType>(splitDatas[0], out var rewardType))
                {
                    NFramework.Logger.LogError($"Can't parse reward type: {splitDatas[0]}");
                    return result;
                }

                switch (rewardType)
                {
                    case ERewardType.Booster:
                        if (Enum.TryParse<EBoosterType>(splitDatas[1], out var boosterType))
                        {
                            result.Add(new RewardData(rewardType, boosterType, int.Parse(splitDatas[2]), true));
                        }
                        break;
                    case ERewardType.Currency:
                        if (Enum.TryParse<ECurrencyType>(splitDatas[1], out var currencyType))
                        {
                            result.Add(new RewardData(rewardType, currencyType, int.Parse(splitDatas[2]), true));
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public static List<int> ConvertStringToListInt(string str)
        {
            var result = new List<int>();
            var splitStr = str.Split('-');

            foreach (var data in splitStr)
            {
                if (int.TryParse(data, out var number))
                {
                    result.Add(number);
                }
            }

            return result;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var random = new System.Random();
            int n = list.Count;

            for (int i = list.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                T value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
        }

        public static IEnumerator UpdateLayoutGroup(GridLayoutGroup layoutGroup)
        {
            layoutGroup.enabled = false;
            yield return new WaitForEndOfFrame();
            layoutGroup.enabled = true;
        }

        public static IEnumerator UpdateLayoutGroup(VerticalLayoutGroup layoutGroup)
        {
            layoutGroup.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
            layoutGroup.gameObject.SetActive(true);
        }
    }
}
