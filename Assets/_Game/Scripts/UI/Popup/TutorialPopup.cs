using NFramework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class TutorialPopup : BaseUIView
    {
        [SerializeField] private Button _btnSkip;
        [SerializeField] private List<RowView> _rowViews;
        [SerializeField] private TextMeshProUGUI _txtTitleStep;
        [SerializeField] private TextMeshProUGUI _txtStepDesc;
        [SerializeField] private TextMeshProUGUI _txtStep;

        private void Awake() => _btnSkip.onClick.AddListener(OnButtonSkipClicked);

        private void OnEnable() => TutorialManager.OnCurTutorialStepChanged += UpdateStepBoard;

        private void OnDisable() => TutorialManager.OnCurTutorialStepChanged -= UpdateStepBoard;

        public override void OnOpen()
        {
            base.OnOpen();

            TutorialManager.I.Init();
            CreateTutorialBoard();
            TutorialManager.I.HighlightCurStepCell();
            UpdateStepBoard(TutorialManager.I.CurTutorialStep);
        }

        public override void OnClose()
        {
            base.OnClose();
            GameManager.I.CurGameState.Value = EGameState.Playing;
        }

        private void OnButtonSkipClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
            UIManager.I.Open(Define.UIName.TUTORIAL_COMPLETE_POPUP);
        }

        private void CreateTutorialBoard()
        {
            for (var i = 0; i < _rowViews.Count; i++)
            {
                var index = i;
                var rowCellDatas = TutorialManager.I.TutorialBoardData.GetRowCellDatas(index);
                _rowViews[i].gameObject.SetActive(true);
                _rowViews[i].Init(index, rowCellDatas, TutorialManager.I.TutorialCellViewDic);
            }
        }

        private void UpdateStepBoard(int step)
        {
            _txtTitleStep.text = GetTitleStep(step);
            _txtStepDesc.text = GetStepDescription(step);
            _txtStep.text = GetStepText(step);
        }

        private string GetTitleStep(int step)
        {
            switch (step)
            {
                case 0:
                case 1:
                case 2:
                    return "Find Pairs of Numbers";
                case 3:
                    return "Check Different Directions";
                case 4:
                    return "Check for Diagonal Pairs";
                case 5:
                    return "Check Line by Line";
                default:
                    return "";
            }
        }

        private string GetStepDescription(int step)
        {
            switch (step)
            {
                case 0:
                case 1:
                case 2:
                    return "Search for numbers with equal value or numbers that add up to 10, and tap them";
                case 3:
                    return "Pairs can be horizontal, vertical, or even diagonal";
                case 4:
                    return "Search for numbers that are separated by empty cells. Diagonally opposite numbers can also make pairs";
                case 5:
                    return "Check the ending of one line on the right and the beginning of the following line on the left, there might be pairs";
                default:
                    return "";
            }
        }

        private string GetStepText(int step)
        {
            switch (step)
            {
                case 0:
                case 1:
                case 2:
                    return "Steps: 1/4";
                case 3:
                    return "Steps: 2/4";
                case 4:
                    return "Steps: 3/4";
                case 5:
                    return "Steps: 4/4";
                default:
                    return "";
            }
        }
    }
}
