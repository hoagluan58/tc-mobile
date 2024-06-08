using NFramework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class BoosterGroupView : MonoBehaviour
    {
        [SerializeField] private Button _btnHintBooster;
        [SerializeField] private Button _btnBombBooster;
        [SerializeField] private Button _btnHammerBooster;
        [SerializeField] private Button _btnUndoBooster;
        [SerializeField] private GameObject _goHintAmountView;
        [SerializeField] private GameObject _goBombAmountView;
        [SerializeField] private GameObject _goHammerAmountView;
        [SerializeField] private GameObject _goUndoAmountView;
        [SerializeField] private GameObject _goHintPlusView;
        [SerializeField] private GameObject _goBombPlusView;
        [SerializeField] private GameObject _goHammerPlusView;
        [SerializeField] private GameObject _goUndoPlusView;
        [SerializeField] private TextMeshProUGUI _txtHintBoosterAmount;
        [SerializeField] private TextMeshProUGUI _txtBombBoosterAmount;
        [SerializeField] private TextMeshProUGUI _txtHammerBoosterAmount;
        [SerializeField] private TextMeshProUGUI _txtUndoBoosterAmount;

        private void Awake()
        {
            _btnHintBooster.onClick.AddListener(OnButtonHintBoosterClicked);
            _btnBombBooster.onClick.AddListener(OnBombBoosterButtonClicked);
            _btnHammerBooster.onClick.AddListener(OnHammerBoosterButtonClicked);
            _btnUndoBooster.onClick.AddListener(OnUndoBoosterButtonClicked);
        }

        private void OnEnable()
        {
            UndoBoosterManager.OnEventUpdateUndo += UpdateButtonUndo;
            HintBoosterManager.I.MovesLeft.OnValueChanged += MovesLeft_OnValueChanged;
            UserData.OnUserBoosterChanged += UserData_OnUserBoosterChanged;
            UpdateButtonUndo(UndoBoosterManager.I.CanUseBooster());
            UpdateView();
        }

        private void OnDisable()
        {
            UndoBoosterManager.OnEventUpdateUndo -= UpdateButtonUndo;
            HintBoosterManager.I.MovesLeft.OnValueChanged -= MovesLeft_OnValueChanged;
            UserData.OnUserBoosterChanged -= UserData_OnUserBoosterChanged;
        }

        private void OnButtonHintBoosterClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            if (UserData.I.IsHaveBooster(EBoosterType.Hint))
            {
                HintBoosterManager.I.UseBooster();
            }
            else
            {
                UIManager.I.Open(Define.UIName.SHOP_POPUP);
            }
        }

        private void OnBombBoosterButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            if (UserData.I.IsHaveBooster(EBoosterType.Bomb))
            {
                UIManager.I.Open(Define.UIName.USE_BOMB_BOOSTER_POPUP);
            }
            else
            {
                UIManager.I.Open(Define.UIName.SHOP_POPUP);
            }
        }

        private void OnHammerBoosterButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            if (UserData.I.IsHaveBooster(EBoosterType.Hammer))
            {
                UIManager.I.Open(Define.UIName.USE_HAMMER_BOOSTER_POPUP);
            }
            else
            {
                UIManager.I.Open(Define.UIName.SHOP_POPUP);
            }
        }

        private void OnUndoBoosterButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            if (UserData.I.IsHaveBooster(EBoosterType.Undo))
            {
                UndoBoosterManager.I.UseBooster();
            }
            else
            {
                UIManager.I.Open(Define.UIName.SHOP_POPUP);
            }
        }

        private void UserData_OnUserBoosterChanged(EBoosterType type, int amount) => UpdateView();

        private void MovesLeft_OnValueChanged(int value) => UpdateButtonHint(value);

        private void UpdateButtonHint(int value) => _btnHintBooster.interactable = value > 0;

        private void UpdateButtonUndo(bool isEnable) => _btnUndoBooster.interactable = isEnable;

        private void UpdateView()
        {
            var isHaveHintBooster = UserData.I.IsHaveBooster(EBoosterType.Hint);
            var isHaveBombBooster = UserData.I.IsHaveBooster(EBoosterType.Bomb);
            var isHaveHammerBooster = UserData.I.IsHaveBooster(EBoosterType.Hammer);
            var isHaveUndoBooster = UserData.I.IsHaveBooster(EBoosterType.Undo);

            _goHintAmountView.SetActive(isHaveHintBooster);
            _goBombAmountView.SetActive(isHaveBombBooster);
            _goHammerAmountView.SetActive(isHaveHammerBooster);
            _goUndoAmountView.SetActive(isHaveUndoBooster);
            _goHintPlusView.SetActive(!isHaveHintBooster);
            _goBombPlusView.SetActive(!isHaveBombBooster);
            _goHammerPlusView.SetActive(!isHaveHammerBooster);
            _goUndoPlusView.SetActive(!isHaveUndoBooster);
            _txtHintBoosterAmount.text = $"{UserData.I.GetBoosterTypeAmount(EBoosterType.Hint)}";
            _txtBombBoosterAmount.text = $"{UserData.I.GetBoosterTypeAmount(EBoosterType.Bomb)}";
            _txtHammerBoosterAmount.text = $"{UserData.I.GetBoosterTypeAmount(EBoosterType.Hammer)}";
            _txtUndoBoosterAmount.text = $"{UserData.I.GetBoosterTypeAmount(EBoosterType.Undo)}";
        }
    }
}
