using NFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TenCrush
{
    public class LevelTargetManager : Singleton<LevelTargetManager>
    {
        public static event Action OnUpdateLevelTarget;

        public List<TargetData> CurLevelTargets  = new List<TargetData>();

        public void CreateLevelTarget(List<TargetData> targetDatas)
        {
            CurLevelTargets.Clear();
            foreach (var targetData in targetDatas)
            {
                CurLevelTargets.Add(targetData.Clone());
            }
        }

        public TargetData GetDataByTargetType(ETargetType targetType) => CurLevelTargets.FirstOrDefault(x => x.targetType == targetType);

        public void UpdateLevelTarget(List<CellView> cellCleared)
        {
            var cellDatas = new List<CellData>();
            cellCleared.ForEach(cell => cellDatas.Add(cell.Data));
            
            foreach (var targetData in CurLevelTargets)
            {
                switch (targetData.targetType)
                {
                    case ETargetType.GrassCell:
                        var grassCellCount = cellDatas.Where(x => x.cellType == ECellType.Grass).Count();

                        //var result = 0;
                        //var cellTypeList = new List<ECellType>() { ECellType.Grass, ECellType.Ice1, ECellType.Ice2, ECellType.Ice3, ECellType.Ice4, ECellType.Ice5 };

                        //foreach (var cellType in cellTypeList)
                        //    result += Board.I.Data.GetTotalCellType(cellType);

                        targetData.amount -= grassCellCount;
                        break;
                    case ETargetType.Score:
                        break;
                    case ETargetType.MatchNumber1:
                        targetData.amount -= cellDatas.Where(x => x.number == 1).Count();
                        break;
                    case ETargetType.MatchNumber2:
                        targetData.amount -= cellDatas.Where(x => x.number == 2).Count();
                        break;
                    case ETargetType.MatchNumber3:
                        targetData.amount -= cellDatas.Where(x => x.number == 3).Count();
                        break;
                    case ETargetType.MatchNumber4:
                        targetData.amount -= cellDatas.Where(x => x.number == 4).Count();
                        break;
                    case ETargetType.MatchNumber5:
                        targetData.amount -= cellDatas.Where(x => x.number == 5).Count();
                        break;
                    case ETargetType.MatchNumber6:
                        targetData.amount -= cellDatas.Where(x => x.number == 6).Count();
                        break;
                    case ETargetType.MatchNumber7:
                        targetData.amount -= cellDatas.Where(x => x.number == 7).Count();
                        break;
                    case ETargetType.MatchNumber8:
                        targetData.amount -= cellDatas.Where(x => x.number == 8).Count();
                        break;
                    case ETargetType.MatchNumber9:
                        targetData.amount -= cellDatas.Where(x => x.number == 9).Count();
                        break;
                }
            }
            OnUpdateLevelTarget?.Invoke();

            if (IsAllTargetCleared())
            {
                GameManager.I.Win();
            }
        }

        public bool IsAllTargetCleared()
        {
            foreach (var targetData in CurLevelTargets)
            {
                switch (targetData.targetType)
                {
                    case ETargetType.Score:
                        if (ScoreManager.I.CurLevelScore >= targetData.amount) continue;
                        else return false;

                    case ETargetType.GrassCell:
                    case ETargetType.MatchNumber1:
                    case ETargetType.MatchNumber2:
                    case ETargetType.MatchNumber3:
                    case ETargetType.MatchNumber4:
                    case ETargetType.MatchNumber5:
                    case ETargetType.MatchNumber6:
                    case ETargetType.MatchNumber7:
                    case ETargetType.MatchNumber8:
                    case ETargetType.MatchNumber9:
                        if (targetData.amount == 0) continue;
                        else return false;
                }
            }
            return true;
        }

        public List<TargetData> CloneCurLevelTargets()
        {
            var result = new List<TargetData>();
            foreach(var data in CurLevelTargets)
            {
                result.Add(data.Clone());
            }
            return result;
        }

        public void Undo(List<TargetData> dataUndo)
        {
            CurLevelTargets.Clear();
            CurLevelTargets = dataUndo;
            OnUpdateLevelTarget?.Invoke();
        }

        public bool IsHaveScoreTarget() => CurLevelTargets.Where(x => x.targetType == ETargetType.Score).Count() > 0;
    }
}
