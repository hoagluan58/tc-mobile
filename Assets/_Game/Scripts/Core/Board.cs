using NFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class Board : SingletonMono<Board>
    {
        private const float CLEAR_ROW_PARTICLE_TIME = 0.5f;

        [SerializeField] private RowView _rowViewPf;
        [SerializeField] private ScrollRect _scrollRect;

        public Dictionary<int, CellView> CellViewDic = new Dictionary<int, CellView>();

        public BoardData Data { get; set; }

        private void OnDisable()
        {
            CellViewDic.Clear();
            _scrollRect.content.DestroyAllChildren();
        }

        public void Init(BoardData data)
        {
            Data = data;

            for (var i = 0; i < Data.rows; i++)
            {
                var row = Instantiate(_rowViewPf, _scrollRect.content);
                var rowCellDatas = Data.GetRowCellDatas(i);
                row.Init(i, rowCellDatas, CellViewDic);
            }
            _scrollRect.verticalNormalizedPosition = 1f;
        }

        public void UpdateBoard()
        {
            var isEnoughtRow = _scrollRect.content.childCount == Data.rows;

            if (!isEnoughtRow)
            {
                CellViewDic.Clear();
                foreach (Transform transform in _scrollRect.content)
                {
                    Destroy(transform.gameObject);
                }

                for (var i = 0; i < Data.rows; i++)
                {
                    var row = Instantiate(_rowViewPf, _scrollRect.content);
                    var rowCellDatas = Data.GetRowCellDatas(i);
                    row.Init(i, rowCellDatas, CellViewDic);
                }
            }
            else
            {
                foreach (var data in Data.cellDatas)
                {
                    if (CellViewDic.TryGetValue(data.id, out var cellView))
                    {
                        data.CellView = cellView;
                        cellView.Data = data;
                        cellView.RefreshView();
                    }
                }
            }
        }

        public int TryClearCellViewRow(CellView cellView)
        {
            Data.GetCellCoordinate(cellView, out var x, out var y);
            if (Data.IsRowCleared(x))
            {
                var rowView = cellView.RowView;
                rowView.PlayClearRowParticle();
                this.InvokeDelay(CLEAR_ROW_PARTICLE_TIME, () =>
                {
                    rowView.Destroy();
                });
                Data.DeleteRow(x);
                GameSound.I.PlaySFX(Define.SoundName.SFX_NUMBER_LINE_MATCH);
                return x;
            }
            return -1;
        }

        public void TryClearNearbyIceCell(CellView cellView)
        {
            var iceCellViews = new List<CellView>();
            Data.GetCellCoordinate(cellView, out var cellX, out var cellY);
            var adjacentIceCellIds = Data.GetAdjacentCells(cellX, cellY);

            foreach (var id in adjacentIceCellIds)
            {
                var cellData = Data.GetCellData(id);
                if (cellData.IsIceCell())
                    iceCellViews.Add(CellViewDic[id]);
            }

            foreach (var view in iceCellViews)
            {
                view.BreakIceCell();
            }
        }

        public List<CellView> GetBombBoosterArea(CellView cell)
        {
            var result = new List<CellView>();
            Data.GetCellCoordinate(cell, out var cellX, out var cellY);
            var adjacentCellIds = Data.GetAdjacentCells(cellX, cellY);

            foreach (var id in adjacentCellIds)
                result.Add(CellViewDic[id]);

            result.Add(cell);
            return result;
        }

        public IEnumerator PlayWinAnimation(Action callback = null)
        {
            var unclearedCells = Data.GetUnclearedCells();
            unclearedCells.Shuffle();

            foreach (var cellData in unclearedCells)
            {
                var cellView = cellData.CellView;
                cellView.ClearCell();
                cellView.PlayMatchNumberParticle();
                yield return new WaitForSeconds(0.01f);
            }
            callback?.Invoke();
            yield return null;
        }
    }
}
