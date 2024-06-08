using NFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TenCrush
{
    public class PlayerController : SingletonMono<PlayerController>
    {
        private List<CellView> _curSelectedCell = new List<CellView>();

        private EControlMode _controlMode;

        public EControlMode ControlMode
        {
            get => _controlMode;
            set
            {
                _controlMode = value;
                DeselectAllSelectedCell();
            }
        }

        private void Start() => GameManager.I.CurGameState.OnValueChanged += CurGameState_OnValueChanged;

        private void Update()
        {
            if (GameManager.I.CurGameState.Value == EGameState.Tutorial)
            {
                HandleTutorialControl();
                return;
            }

            if (GameManager.I.CurGameState.Value == EGameState.Playing)
            {
                switch (ControlMode)
                {
                    case EControlMode.Normal:
                        HandleNormalControlMode();
                        break;
                    case EControlMode.Hammer:
                        HandleHammerControlMode();
                        break;
                    case EControlMode.Bomb:
                        HandleBombControlMode();
                        break;
                }
                return;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (GameManager.IsSingletonAlive)
                GameManager.I.CurGameState.OnValueChanged -= CurGameState_OnValueChanged;
        }

        private void CurGameState_OnValueChanged(EGameState state) => DeselectAllSelectedCell();

        private void HandleNormalControlMode()
        {
            if (UIManager.I.IsPopupShown())
                return;

            if (Input.GetMouseButtonDown(0) && TryGetCellAtMousePos(out var cell))
            {
                if (!cell.CanSelect())
                {
                    var isCleared = cell.Data.isCleared;
                    if (isCleared)
                    {
                        cell.ShakeNumber();
                        return;
                    }
                    return;
                }

                if (IsHaveSelectedCell())
                {
                    if (IsAlreadySelected(cell))
                    {
                        cell.UpdateSelectCellUI(false);
                        _curSelectedCell.Remove(cell);
                        return;
                    }

                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    var selectedCell = _curSelectedCell.First();
                    if (Board.I.Data.CheckMatch(selectedCell, cell))
                    {
                        var cellDatas = Board.I.Data.Clone(Board.I.Data.cellDatas, Board.I.Data.rows, Board.I.Data.columns);
                        var cellCleared = new List<CellView>() { cell, selectedCell };
                        cellCleared.ForEach(cell =>
                        {
                            cell.ClearCell();
                            cell.PlayMatchNumberParticle();
                        });
                        _curSelectedCell.Clear();

                        var rowDeleteds = new List<int>();
                        var isCellOnSameRow = selectedCell.RowView.Index == cell.RowView.Index;

                        if (isCellOnSameRow)
                        {
                            rowDeleteds.Add(Board.I.TryClearCellViewRow(selectedCell));
                        }
                        else
                        {
                            rowDeleteds.Add(Board.I.TryClearCellViewRow(selectedCell));
                            rowDeleteds.Add(Board.I.TryClearCellViewRow(cell));
                        }

                        UndoBoosterManager.I.PushUndo(cellDatas, rowDeleteds);
                        ScoreManager.I.CalculateCellDestroyedScore(cellCleared);
                        LevelTargetManager.I.UpdateLevelTarget(cellCleared);
                        HintBoosterManager.I.ResetHint();
                        HintBoosterManager.I.CheckPossibleMoves();
                    }
                    else
                    {
                        selectedCell.UpdateSelectCellUI(false);
                        cell.UpdateSelectCellUI(true);
                        _curSelectedCell.Remove(selectedCell);
                        _curSelectedCell.Add(cell);
                    }
                }
                else
                {
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    cell.UpdateSelectCellUI(true);
                    _curSelectedCell.Add(cell);
                }
            }
        }

        private void HandleHammerControlMode()
        {
            if (Input.GetMouseButton(0) && TryGetCellAtMousePos(out var cell))
            {
                if (IsAlreadySelected(cell))
                    return;

                if (UIManager.I.IsSpecificViewShown(Define.UIName.USE_HAMMER_BOOSTER_POPUP, out var view))
                {
                    var useHammerBoosterPopup = view as UseHammerBoosterPopup;
                    useHammerBoosterPopup.UpdateUI(cell);
                    DeselectAllSelectedCell();
                    _curSelectedCell.Add(cell);
                }
            }
        }

        private void HandleBombControlMode()
        {
            if (Input.GetMouseButton(0) && TryGetCellAtMousePos(out var cell))
            {
                if (UIManager.I.IsSpecificViewShown(Define.UIName.USE_BOMB_BOOSTER_POPUP, out var view))
                {
                    var useBombBoosterPopup = view as UseBombBoosterPopup;
                    var result = Board.I.GetBombBoosterArea(cell);

                    DeselectAllSelectedCell();
                    useBombBoosterPopup.UpdateUI(result);
                    _curSelectedCell.AddRange(result);
                }
            }
        }

        private void HandleTutorialControl()
        {
            if (Input.GetMouseButtonDown(0) && TryGetCellAtMousePos(out var cell))
            {
                if (!cell.CanSelect())
                {
                    var isCleared = cell.Data.isCleared;
                    if (isCleared)
                    {
                        cell.ShakeNumber();
                        return;
                    }
                    return;
                }

                if (IsHaveSelectedCell())
                {
                    if (IsAlreadySelected(cell))
                    {
                        cell.UpdateSelectCellUI(false);
                        _curSelectedCell.Remove(cell);
                        return;
                    }

                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    var selectedCell = _curSelectedCell.First();
                    if (TutorialManager.I.TutorialBoardData.CheckMatch(selectedCell, cell))
                    {
                        var cellCleared = new List<CellView>() { cell, selectedCell };
                        cellCleared.ForEach(cell =>
                        {
                            cell.ClearCell();
                            cell.PlayMatchNumberParticle();
                        });
                        _curSelectedCell.Clear();

                        var isCellOnSameRow = selectedCell.RowView.Index == cell.RowView.Index;
                        if (isCellOnSameRow)
                        {
                            TutorialManager.I.TryClearRow(selectedCell);
                        }
                        else
                        {
                            TutorialManager.I.TryClearRow(selectedCell);
                            TutorialManager.I.TryClearRow(cell);
                        }

                        TutorialManager.I.GoNextStep();
                    }
                    else
                    {
                        selectedCell.UpdateSelectCellUI(false);
                        cell.UpdateSelectCellUI(true);
                        _curSelectedCell.Remove(selectedCell);
                        _curSelectedCell.Add(cell);
                    }
                }
                else
                {
                    VibrationManager.I.Haptic(VibrationManager.EHapticType.LightImpact);
                    cell.UpdateSelectCellUI(true);
                    _curSelectedCell.Add(cell);
                }
            }
        }

        private void DeselectAllSelectedCell()
        {
            _curSelectedCell.ForEach(cell => cell.ResetCell());
            _curSelectedCell.Clear();
        }

        private bool TryGetCellAtMousePos(out CellView cell)
        {
            cell = null;
            var pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.transform.TryGetComponent(out cell))
                    return true;
            }
            return false;
        }

        private bool IsHaveSelectedCell() => _curSelectedCell.Count > 0;

        private bool IsAlreadySelected(CellView cell) => _curSelectedCell.Contains(cell);
    }

    public enum EControlMode
    {
        Normal,
        Hammer,
        Bomb,
    }
}
