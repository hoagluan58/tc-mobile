using NFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TenCrush
{
    public class TutorialManager : SingletonMono<TutorialManager>
    {
        public static Action<int> OnCurTutorialStepChanged;
        private const int TUTORIAL_LEVEL_ID = -1;
        private const float CLEAR_ROW_PARTICLE_TIME = 0.5f;

        private Dictionary<int, List<int>> HighlightTutorialCellDic = new Dictionary<int, List<int>>()
        {
            {0, new List<int>(){ 2, 3 }},
            {1, new List<int>(){ 0, 5 }},
            {2, new List<int>(){ 6, 7 }},
            {3, new List<int>(){ 8, 14 }},
            {4, new List<int>(){ 1, 13 }},
            {5, new List<int>(){ 9, 10 }},
        };

        public Dictionary<int, CellView> TutorialCellViewDic = new Dictionary<int, CellView>();

        public int CurTutorialStep { get; set; }
        public LevelData TutorialLevelData { get; set; }
        public BoardData TutorialBoardData { get; set; }


        public void Init()
        {
            GameManager.I.CurGameState.Value = EGameState.Tutorial;
            CurTutorialStep = 0;
            TutorialCellViewDic.Clear();
            TutorialLevelData = GameManager.I.LevelConfig.GetLevelData(TUTORIAL_LEVEL_ID);
            TutorialBoardData = BoardData.CreateBoardData(TutorialLevelData);
        }

        public void GoNextStep()
        {
            CurTutorialStep++;
            OnCurTutorialStepChanged?.Invoke(CurTutorialStep);

            if (HighlightTutorialCellDic.Last().Key < CurTutorialStep)
            {
                foreach (var pair in TutorialCellViewDic)
                {
                    pair.Value.SetActiveTutorialCell(false);
                }

                if (UIManager.I.IsSpecificViewShown(Define.UIName.TUTORIAL_POPUP, out var view))
                {
                    this.InvokeDelay(0.5f, () =>
                    {
                        UIManager.I.Close(view);
                        UIManager.I.Open(Define.UIName.TUTORIAL_COMPLETE_POPUP);
                    });
                }

                return;
            }

            HighlightCurStepCell();
        }

        public void HighlightCurStepCell()
        {
            var highlightCellIndex = HighlightTutorialCellDic[CurTutorialStep];
            foreach (var pair in TutorialCellViewDic)
            {
                var isHintCell = highlightCellIndex.Contains(pair.Key);
                pair.Value.SetActiveTutorialCell(isHintCell);
            }
        }

        public void TryClearRow(CellView cellView)
        {
            TutorialBoardData.GetCellCoordinate(cellView, out var x, out var y);
            if (TutorialBoardData.IsRowCleared(x))
            {
                var rowView = cellView.RowView;
                rowView.PlayClearRowParticle();
                this.InvokeDelay(CLEAR_ROW_PARTICLE_TIME, () =>
                {
                    rowView.gameObject.SetActive(false);
                });
                GameSound.I.PlaySFX(Define.SoundName.SFX_NUMBER_LINE_MATCH);
            }
        }
    }
}
