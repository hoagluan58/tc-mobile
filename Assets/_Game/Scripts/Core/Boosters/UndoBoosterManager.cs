using NFramework;
using System;
using System.Collections.Generic;

namespace TenCrush
{
    public class UndoBoosterManager : Singleton<UndoBoosterManager>
    {
        public static event Action<bool> OnEventUpdateUndo;

        public class DataUndo
        {
            public CellData[,] cellDatas;
            public List<int> rowDeleteds = new List<int>();
            public List<TargetData> targets = new List<TargetData>();
        }

        private Stack<DataUndo> _dataUndos = new Stack<DataUndo>();

        public void PushUndo(CellData[,] oldData, List<int> rowDeleteds)
        {
            rowDeleteds.RemoveAll(item => item == -1);

            var newData = new DataUndo();
            newData.cellDatas = oldData;
            newData.rowDeleteds = rowDeleteds;
            newData.targets = LevelTargetManager.I.CloneCurLevelTargets();
            _dataUndos.Push(newData);
            OnEventUpdateUndo?.Invoke(_dataUndos.Count > 0);
        }

        public void UseBooster()
        {
            var data = PopUndo();
            UserData.I.ModifyBoosterTypeAmount(EBoosterType.Undo, -1);
            Board.I.Data.rows = data.cellDatas.GetLength(0);
            Board.I.Data.columns = data.cellDatas.GetLength(1);
            Board.I.Data.cellDatas = data.cellDatas;
            Board.I.UpdateBoard();
            LevelTargetManager.I.Undo(data.targets);
        }

        public bool CanUseBooster() => _dataUndos.Count > 0;
        private DataUndo PopUndo()
        {
            var data = _dataUndos.Pop();
            OnEventUpdateUndo?.Invoke(_dataUndos.Count > 0);
            return data;
        }
    }
}
