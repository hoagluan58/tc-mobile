using NFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TenCrush
{
    [System.Serializable]
    public class LevelData
    {
        public int id;
        public string numberMatrix;
        public string objectMatrix;
        public List<TargetData> targets;

        [HideInInspector] public string targetDataString;
    }

    [System.Serializable]
    public class TargetData
    {
        public ETargetType targetType;
        public int amount;

        public TargetData(ETargetType targetType, int amount)
        {
            this.targetType = targetType;
            this.amount = amount;
        }

        public TargetData Clone()
        {
            return new TargetData(targetType, amount);
        }

        public bool IsGrassCellTarget() => targetType == ETargetType.GrassCell;

        public bool IsMatchNumberTarget() => targetType == ETargetType.MatchNumber1
                   || targetType == ETargetType.MatchNumber2
                   || targetType == ETargetType.MatchNumber3
                   || targetType == ETargetType.MatchNumber4
                   || targetType == ETargetType.MatchNumber5
                   || targetType == ETargetType.MatchNumber6
                   || targetType == ETargetType.MatchNumber7
                   || targetType == ETargetType.MatchNumber8
                   || targetType == ETargetType.MatchNumber9;
    }

    public enum ETargetType
    {
        GrassCell,
        Score,
        MatchNumber1,
        MatchNumber2,
        MatchNumber3,
        MatchNumber4,
        MatchNumber5,
        MatchNumber6,
        MatchNumber7,
        MatchNumber8,
        MatchNumber9,
    }

    [CreateAssetMenu(menuName = "ScriptableObject/Configs/LevelConfigSO")]
    public class LevelConfigSO : GoogleSheetConfigSO<LevelData>
    {
        public LevelData GetLevelData(int level)
        {
            var levelData = _datas.FirstOrDefault(x => x.id == level);
            if (levelData == null)
            {
                UserData.I.CurrentLevel = 1;
                level = 1;
            }
            return _datas.FirstOrDefault(x => x.id == level);
        }

        public int LevelMax() => _datas.Last().id;
#if UNITY_EDITOR
        protected override void OnSynced(List<LevelData> googleSheetData)
        {
            base.OnSynced(googleSheetData);
            foreach (var data in _datas)
            {
                data.targets = ConvertStrToTargets(data.targetDataString);
            }
        }
#endif

        private List<TargetData> ConvertStrToTargets(string str)
        {
            var result = new List<TargetData>();

            if (string.IsNullOrEmpty(str))
                return result;

            var targetDatasStr = str.Split(";");

            foreach (var data in targetDatasStr)
            {
                var targetStr = data.Split("-");
                var type = Enum.Parse<ETargetType>(targetStr[0]);
                var amount = int.Parse(targetStr[1]);
                result.Add(new TargetData(type, amount));
            }

            return result;
        }
    }
}
