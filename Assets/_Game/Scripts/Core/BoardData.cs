using System;
using System.Collections.Generic;
using UnityEngine;

namespace TenCrush
{
    [Serializable]
    public class BoardData
    {
        private const string HYPHEN = "-";
        private const string WHITESPACE = " ";

        public int rows;
        public int columns;
        public CellData[,] cellDatas;

        public static BoardData CreateBoardData(LevelData levelData)
        {
            var boardData = new BoardData();

            CreateNumberBoard(levelData, ref boardData);
            CreateSpecialCellBoard(levelData, ref boardData);

            return boardData;
        }

        private static void CreateNumberBoard(LevelData levelData, ref BoardData boardData)
        {
            var rowsString = levelData.numberMatrix.Split(WHITESPACE);

            boardData.rows = rowsString.Length;
            boardData.columns = rowsString[0].Split(HYPHEN).Length;
            boardData.cellDatas = new CellData[boardData.rows, boardData.columns];

            var cellIndex = 0;
            for (var i = 0; i < rowsString.Length; i++)
            {
                var columnsStr = rowsString[i].Split(HYPHEN);
                for (var j = 0; j < columnsStr.Length; j++)
                {
                    var number = int.Parse(columnsStr[j]);
                    boardData.cellDatas[i, j] = new CellData(cellIndex, number, ECellType.Normal);
                    cellIndex++;
                }
            }
        }

        private static void CreateSpecialCellBoard(LevelData levelData, ref BoardData boardData)
        {
            var rowsString = levelData.objectMatrix.Split(WHITESPACE);

            for (var i = 0; i < rowsString.Length; i++)
            {
                var columnsStr = rowsString[i].Split(HYPHEN);
                if (columnsStr.Length == Define.MAX_COLUMN_COUNT)
                {
                    for (var j = 0; j < columnsStr.Length; j++)
                    {
                        var cellType = int.Parse(columnsStr[j]);
                        boardData.cellDatas[i, j].cellType = (ECellType)cellType;
                    }
                }
            }
        }

        public CellData[,] Clone(CellData[,] cells, int row, int column)
        {
            CellData[,] oldData = new CellData[row, column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    oldData[i, j] = new CellData(cells[i, j]);
                    oldData[i, j].CellView = cells[i, j].CellView;
                }
            }
            return oldData;
        }

        public bool CheckMatch(CellView cell, CellView targetCell)
        {
            GetCellCoordinate(cell, out var cellX, out var cellY);
            var validCells = GetValidMatchCells(cellX, cellY);

            if (validCells.Contains(targetCell.Data.id))
            {
                var isPair = targetCell.Data.number == cell.Data.number;
                var isSumEqualTen = targetCell.Data.number == GetMatchingNumber(cell.Data.number);

                return isPair || isSumEqualTen;
            }

            return false;
        }

        public void GetCellCoordinate(CellView cell, out int x, out int y)
        {
            x = -1; y = -1;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    if (cellDatas[i, j].id == cell.Data.id)
                    {
                        x = i;
                        y = j;
                        return;
                    }
                }
            }
            Debug.Log($"Can't get Coordinate of Cell: {cell}");
        }

        public CellData GetCellData(int cellIndex)
        {
            foreach (var cellData in cellDatas)
            {
                if (cellData.id == cellIndex)
                {
                    return cellData;
                }
            }

            Debug.LogError($"Can't get Cell Data with Index: {cellIndex}");
            return null;
        }

        public CellData GetCellData(int x, int y) => IsValidPos(x, y) ? cellDatas[x, y] : null;

        public List<CellData> GetUnclearedCells()
        {
            var result = new List<CellData>();

            foreach (var cell in cellDatas)
            {
                if (cell.isCleared)
                    continue;

                result.Add(cell);
            }

            return result;
        }

        public List<int> GetAdjacentCells(int x, int y)
        {
            var result = new List<int>();
            GetHorizontalElements(x, y, ref result, false);
            GetVerticalElements(x, y, ref result, false);
            GetDiagonalElements(x, y, ref result, false);
            return result;
        }

        public List<int> GetValidMatchCells(int x, int y)
        {
            var result = new List<int>();
            GetHorizontalElements(x, y, ref result);
            GetVerticalElements(x, y, ref result);
            GetDiagonalElements(x, y, ref result);
            GetLineByLineElement(x, y, ref result);
            return result;
        }

        private void GetHorizontalElements(int x, int y, ref List<int> result, bool needCheckValid = true)
        {
            // Check left
            for (var l = y - 1; l >= 0; l--)
            {
                var cellData = GetCellData(x, l);

                if (!cellData.CanCheckCell() && needCheckValid)
                    continue;

                result.Add(cellData.id);
                break;
            }

            // Check right
            for (var r = y + 1; r < columns; r++)
            {
                var cellData = GetCellData(x, r);

                if (!cellData.CanCheckCell() && needCheckValid)
                    continue;

                result.Add(cellData.id);
                break;
            }
        }

        private void GetVerticalElements(int x, int y, ref List<int> result, bool skipClearedCell = true)
        {
            // Check up
            for (var u = x - 1; u >= 0; u--)
            {
                var cellData = GetCellData(u, y);

                if (!cellData.CanCheckCell() && skipClearedCell)
                    continue;

                result.Add(cellData.id);
                break;
            }

            // Check down
            for (var d = x + 1; d < rows; d++)
            {
                var cellData = GetCellData(d, y);

                if (!cellData.CanCheckCell() && skipClearedCell)
                    continue;

                result.Add(cellData.id);
                break;
            }
        }

        private void GetDiagonalElements(int x, int y, ref List<int> result, bool skipClearedCell = true)
        {
            // Top right
            int i = x - 1, j = y + 1;
            while (i >= 0 && j < columns)
            {
                var cellData = GetCellData(i, j);

                if (cellData.CanCheckCell() || !skipClearedCell)
                {
                    result.Add(cellData.id);
                    break;
                }

                i--;
                j++;
            }

            // Bottom left
            i = x + 1;
            j = y - 1;
            while (i < rows && j >= 0)
            {
                var cellData = GetCellData(i, j);

                if (cellData.CanCheckCell() || !skipClearedCell)
                {
                    result.Add(cellData.id);
                    break;
                }

                i++;
                j--;
            }

            // Top left
            i = x - 1;
            j = y - 1;
            while (i >= 0 && j >= 0)
            {
                var cellData = GetCellData(i, j);

                if (cellData.CanCheckCell() || !skipClearedCell)
                {
                    result.Add(cellData.id);
                    break;
                }

                i--;
                j--;
            }

            // Bottom right
            i = x + 1;
            j = y + 1;
            while (i < rows && j < columns)
            {
                var cellData = GetCellData(i, j);

                if (cellData.CanCheckCell() || !skipClearedCell)
                {
                    result.Add(cellData.id);
                    break;
                }

                i++;
                j++;
            }
        }

        private void GetLineByLineElement(int x, int y, ref List<int> result)
        {
            var isBreak = false;
            y++;

            for (var i = x; i < rows && !isBreak; i++)
            {
                for (var j = y; j < columns && !isBreak; j++)
                {
                    var cellData = GetCellData(i, j);
                    if (!cellData.CanCheckCell())
                        continue;

                    result.Add(cellData.id);
                    isBreak = true;
                    break;
                }
                y = 0;
            }
        }

        public void DeleteRow(int rowIndex)
        {
            var newRows = rows - 1;
            var newRowIndex = 0;
            var newCellDatas = new CellData[newRows, columns];

            for (var i = 0; i < rows; i++)
            {
                if (i == rowIndex)
                {
                    continue;
                }

                for (var j = 0; j < columns; j++)
                {
                    newCellDatas[newRowIndex, j] = cellDatas[i, j];
                }
                newRowIndex++;
            }

            rows--;
            cellDatas = newCellDatas;
        }

        public bool IsRowCleared(int rowIndex)
        {
            var x = rowIndex;

            for (var y = 0; y < columns; y++)
            {
                var cellData = GetCellData(x, y);
                if (!cellData.isCleared)
                    return false;
            }

            return true;
        }

        public int GetMatchingNumber(int number) => Define.MATCH_NUMBER - number;

        private bool IsValidPos(int x, int y)
        {
            if (x < 0 || y < 0 || x > rows - 1 || y > columns - 1)
            {
                Debug.LogError($"Position not valid [{x}][{y}]");
                return false;
            }
            return true;
        }

        public List<CellData> GetRowCellDatas(int rowIndex)
        {
            var result = new List<CellData>();
            for (var i = 0; i < columns; i++)
            {
                result.Add(cellDatas[rowIndex, i]);
            }
            return result;
        }

        public int GetTotalCellType(ECellType cellType, bool getClearedCell = false)
        {
            var result = 0;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var cellData = GetCellData(i, j);

                    if (cellData.cellType == cellType)
                    {
                        var isCleared = cellData.isCleared;
                        if (getClearedCell)
                        {
                            result++;
                        }
                        else
                        {
                            if (isCleared)
                                continue;
                            else
                                result++;
                        }
                    }
                }
            }

            return result;
        }

        public int GetTotalCellWithNumber(int number, bool getClearedCell = false)
        {
            var result = 0;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var cellData = GetCellData(i, j);

                    if (cellData.number == number)
                    {
                        var isCleared = cellData.isCleared;
                        if (getClearedCell)
                        {
                            result++;
                        }
                        else
                        {
                            if (isCleared)
                                continue;
                            else
                                result++;
                        }
                    }
                }
            }

            return result;
        }
        #region Random Board Generator
        //public BoardData CreateBoardData(LevelData levelData)
        //{
        //    var boardData = new BoardData();

        //    boardData.rows = (levelData.cellCount / boardData.columns);
        //    boardData.cellDatas = new CellData[boardData.rows, boardData.columns];

        //    var cellIndex = 0;
        //    for (var i = 0; i < boardData.rows; i++)
        //    {
        //        for (var j = 0; j < boardData.columns; j++)
        //        {
        //            var number = UnityEngine.Random.Range(1, 10);
        //            boardData.cellDatas[i, j] = new CellData(cellIndex, number);
        //            cellIndex++;
        //        }
        //    }
        //    return boardData;
        //}

        //// TODO: Generate random pool
        //private List<int> CreatePoolNumber(int cellCount)
        //{
        //    var pool = new List<int>();
        //    var halfCellCount = cellCount / 2;

        //    for (var i = 0; i < halfCellCount; i++)
        //    {

        //    }

        //    for (var i = halfCellCount; i < cellCount; i++)
        //    {


        //    }

        //    return pool;
        //}
        #endregion
    }
}
