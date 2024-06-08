using NFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class GameMenu : BaseUIView
    {
        [SerializeField] private Board _board;
        [SerializeField] private TextMeshProUGUI _txtPossibleMoves;
        [SerializeField] private TextMeshProUGUI _txtCurrentLevel;
        [SerializeField] private Button _btnPause;
        [SerializeField] private Button _btnTutorial;

        [Header("Cheat")]
        [SerializeField] private GameObject _goCoinBar;
        [SerializeField] private GameObject _goPossibleMoves;
        [SerializeField] private GameObject _goBoosterGroup;
        [SerializeField] private GameObject _goTargetGroup;

        public TargetGroupView TargetGroup;

        private void Awake()
        {
            _btnPause.onClick.AddListener(OnButtonPauseClicked);
            _btnTutorial.onClick.AddListener(OnButtonTutorialClicked);
        }

        private void OnEnable() => HintBoosterManager.I.MovesLeft.OnValueChanged += MovesLeft_OnValueChanged;

        private void OnDisable() => HintBoosterManager.I.MovesLeft.OnValueChanged -= MovesLeft_OnValueChanged;

        private void OnButtonPauseClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            UIManager.I.Open(Define.UIName.PAUSE_POPUP);
        }

        private void OnButtonTutorialClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            UIManager.I.Open(Define.UIName.TUTORIAL_POPUP);
        }

        private void MovesLeft_OnValueChanged(int value) => UpdatePossibleMovesText(value);

        public void Init(BoardData boardData)
        {
            _board.Init(boardData);
            _txtCurrentLevel.text = $"Level {UserData.I.CurrentLevel}";
            TargetGroup.Init();
        }

        #region Cheat
        public void ToggleUnneededUI()
        {
            var isOn = _btnPause.gameObject.activeSelf;
            _btnPause.gameObject.SetActive(!isOn);
            _btnTutorial.gameObject.SetActive(!isOn);
            _txtCurrentLevel.gameObject.SetActive(!isOn);
            _goPossibleMoves.SetActive(!isOn);
            _goCoinBar.SetActive(!isOn);
        }

        public void ToggleBoosterGroup()
        {
            var isOn = _goBoosterGroup.activeSelf;
            _goBoosterGroup.SetActive(!isOn);
        }

        public void ToggleTargetGroup()
        {
            var isOn = _goTargetGroup.activeSelf;
            _goTargetGroup.SetActive(!isOn);
        }
        #endregion

        private void UpdatePossibleMovesText(int value) => _txtPossibleMoves.text = value > 5 ? $"5+" : $"{value}";
    }
}
