using Coffee.UIExtensions;
using NFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class UseBombBoosterPopup : BaseUIView
    {
        private const float BOMB_PARTICLE_TIME = 0.5f;

        [SerializeField] private Button _btnCloseDescBoard;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private GameObject _goDescBoard;
        [SerializeField] private UIParticle _bombParticle;

        private List<CellView> _destroyCells = new List<CellView>();

        private void Awake()
        {
            _btnCloseDescBoard.onClick.AddListener(OnButtonCloseDescBoardClicked);
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }


        public override void OnOpen()
        {
            base.OnOpen();
            PlayerController.I.ControlMode = EControlMode.Bomb;
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

        public void UpdateUI(List<CellView> cells)
        {
            _destroyCells = cells;
            _destroyCells.ForEach(cell => cell.HighlightDestroyCell(true));
            _confirmButton.interactable = !_destroyCells.All(cell => cell.Data.isCleared != false);
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
            GameSound.I.PlaySFX(Define.SoundName.SFX_BOMB_BOOSTER);
            UserData.I.ModifyBoosterTypeAmount(EBoosterType.Bomb, -1);
            VibrationManager.I.Haptic(VibrationManager.EHapticType.HeavyImpact);
            UIManager.I.DisableInteract(this);
            _bombParticle.transform.position = _destroyCells.Last().transform.position;
            _bombParticle.Play();
            this.InvokeDelay(BOMB_PARTICLE_TIME, () =>
            {
                UIManager.I.EnableInteract(this);
                var cellDatas = Board.I.Data.Clone(Board.I.Data.cellDatas, Board.I.Data.rows, Board.I.Data.columns);
                var rowDeleteds = new List<int>();
                var unclearedCells = _destroyCells.Where(x => x.Data.isCleared == false).ToList();
                unclearedCells.ForEach(cell =>
                {
                    cell.ClearCell();
                    rowDeleteds.Add(Board.I.TryClearCellViewRow(cell));
                });

                UndoBoosterManager.I.PushUndo(cellDatas, rowDeleteds);

                var haveClearedCell = false;
                foreach (var cell in unclearedCells)
                {
                    var isCleared = cell.Data.isCleared;
                    if (isCleared)
                    {
                        haveClearedCell = true;
                        break;
                    }
                }

                if (haveClearedCell)
                {
                    ScoreManager.I.CalculateCellDestroyedScore(unclearedCells);
                    LevelTargetManager.I.UpdateLevelTarget(unclearedCells);
                }

                HintBoosterManager.I.CheckPossibleMoves();
                HintBoosterManager.I.ResetHint();
                CloseSelf();
            });
        }

        private void CheckToggleBoosterGroup()
        {
            if(UIManager.I.IsSpecificViewShown(Define.UIName.GAME_MENU, out var view))
            {
                var gameMenu = view as GameMenu;
                gameMenu.ToggleBoosterGroup();
            }
        }
    }
}
