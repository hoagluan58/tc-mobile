using System;

namespace TenCrush
{
    [Serializable]
    public class CellData
    {
        public int id;
        public int number;
        public ECellType cellType;
        public bool isCleared;

        public CellView CellView { get; set; }

        public CellData(int id, int number, ECellType cellType)
        {
            this.id = id;
            this.number = number;
            this.cellType = cellType;
        }

        public CellData(CellData data)
        {
            this.id = data.id;
            this.number = data.number;
            this.cellType = data.cellType;
            this.isCleared = data.isCleared;
        }

        public void BreakIceLayer()
        {
            if (cellType == ECellType.Ice1)
            {
                cellType = ECellType.Grass;
                return;
            }
            else cellType--;
        }

        public bool IsIceCell() => cellType == ECellType.Ice1
                                   || cellType == ECellType.Ice2
                                   || cellType == ECellType.Ice3
                                   || cellType == ECellType.Ice4
                                   || cellType == ECellType.Ice5;

        public bool CanCheckCell() => !isCleared && !IsIceCell();
    }

    public enum ECellType
    {
        Normal = 0,
        Grass = 1,
        Ice1 = 2,
        Ice2 = 3,
        Ice3 = 4,
        Ice4 = 5,
        Ice5 = 6,
    }
}
