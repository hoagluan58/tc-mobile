using Coffee.UIExtensions;
using NFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class UseHammerBoosterPopup : BaseUIView
    {
        private const float HAMMER_PARTICLE_TIME = 0.5f;

        [SerializeField] private Button _btnCloseDescBoard;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private UIParticle _hammerParticle;
        [SerializeField] private GameObject _goDescBoard;

        private CellView _destroyCell;

        private void Awake()
        {
            _btnCloseDescBoard.onClick.AddListener(OnButtonCloseDescBoardClicked);
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            PlayerController.I.ControlMode = EControlMode.Hammer;
            _confirmButton.interactable = false;
            _goDescBoard.SetActive(true);
            CheckToggleBoosterGroup();
        }

        public override void OnClose()
        {
            base.OnClose();
            PlayerController.I.ControlMode = EControlMode.Normal;
            CheckToggleBoosterGroup();
        }

        public void UpdateUI(CellView cell)
        {
            _destroyCell = cell;
            cell.HighlightDestroyCell(true);
            _confirmButton.interactable = !cell.Data.isCleared;
        }

        private void OnButtonCloseDescBoardClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            _goDescBoard.SetActive(false);
        }

        private void OnCancelButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
        }

        private void OnConfirmButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            VibrationManager.I.Haptic(VibrationManager.EHapticType.HeavyImpact);
            UserData.I.ModifyBoosterTypeAmount(EBoosterType.Hammer, -1);
            UIManager.I.DisableInteract(this);
            _hammerParticle.transform.position = _destroyCell.transform.position;
            _hammerParticle.Play();

            this.InvokeDelay(HAMMER_PARTICLE_TIME, () =>
            {
                UIManager.I.EnableInteract(this);
                var cellDatas = Board.I.Data.Clone(Board.I.Data.cellDatas, Board.I.Data.rows, Board.I.Data.columns);
                var rowDeleteds = new List<int>();
                
                _destroyCell.ClearCell();
                rowDeleteds.Add(Board.I.TryClearCellViewRow(_destroyCell));

                UndoBoosterManager.I.PushUndo(cellDatas, rowDeleteds);

                var isCleared = _destroyCell.Data.isCleared;
                if (isCleared)
                {
                    var clearedCell = new List<CellView> { _destroyCell };
                    ScoreManager.I.CalculateCellDestroyedScore(clearedCell);
                    LevelTargetManager.I.UpdateLevelTarget(clearedCell);
                }

                HintBoosterManager.I.ResetHint();
                HintBoosterManager.I.CheckPossibleMoves();
                CloseSelf();
            });
        }

        private void CheckToggleBoosterGroup()
        {
            if (UIManager.I.IsSpecificViewShown(Define.UIName.GAME_MENU, out var view))
            {
                var gameMenu = view as GameMenu;
                gameMenu.ToggleBoosterGroup();
            }
        }
    }
}
