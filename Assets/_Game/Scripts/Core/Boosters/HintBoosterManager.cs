using NFramework;
using System;
using System.Collections.Generic;

namespace TenCrush
{
    public class HintBoosterManager : Singleton<HintBoosterManager>
    {
        public ObservableValue<int> MovesLeft = new ObservableValue<int>();

        private List<PairData> _pairDatas;

        public void CheckPossibleMoves()
        {
            _pairDatas = new List<PairData>();
            var boardData = Board.I.Data;

            for (var i = 0; i < boardData.rows; i++)
            {
                for (var j = 0; j < boardData.columns; j++)
                {
                    var currentCellData = boardData.GetCellData(i, j);

                    if (currentCellData.isCleared || currentCellData.IsIceCell())
                        continue;

                    var validCells = boardData.GetValidMatchCells(i, j);

                    foreach (var id in validCells)
                    {
                        var targetCellData = boardData.GetCellData(id);
                        var isPair = currentCellData.number == targetCellData.number;
                        var isSumEqualTen = currentCellData.number == boardData.GetMatchingNumber(targetCellData.number);
                        if (isPair || isSumEqualTen)
                        {
                            var pairData = new PairData(currentCellData.id, targetCellData.id);
                            if (!IsExisted(pairData))
                            {
                                _pairDatas.Add(pairData);
                            }
                        }
                    }
                }
            }

            MovesLeft.Value = _pairDatas.Count;
            var noPossibleMoveLeft = MovesLeft.Value == 0;

            if (noPossibleMoveLeft)
                GameManager.I.Lose();
        }

        public void UseBooster()
        {
            var cellViewDic = Board.I.CellViewDic;
            var pairData = GetRandomPairData();

            ResetHint();

            UserData.I.ModifyBoosterTypeAmount(EBoosterType.Hint, -1);
            cellViewDic[pairData.cellId1].SetActiveHintParticle(true);
            cellViewDic[pairData.cellId2].SetActiveHintParticle(true);
        }

        public void ResetHint()
        {
            var cellViewDic = Board.I.CellViewDic;

            foreach (var cellView in cellViewDic.Values)
                cellView.SetActiveHintParticle(false);
        }

        private PairData GetRandomPairData()
        {
            var random = new Random();
            return _pairDatas[random.Next(_pairDatas.Count)];
        }

        private bool IsExisted(PairData pairData)
        {
            foreach (var data in _pairDatas)
            {
                if (data.cellId1 == pairData.cellId1 && data.cellId2 == pairData.cellId2)
                {
                    return true;
                }

                if (data.cellId1 == pairData.cellId2 && data.cellId2 == pairData.cellId1)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class PairData
    {
        public int cellId1;
        public int cellId2;

        public PairData(int cellId1, int cellId2)
        {
            this.cellId1 = cellId1;
            this.cellId2 = cellId2;
        }
    }
}
