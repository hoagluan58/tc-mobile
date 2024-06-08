using NFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class CoinBarUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtCoinAmount;
        [SerializeField] private Button _btnShop;

        private void Awake() => _btnShop.onClick.AddListener(OnButtonShopClicked);

        private void OnEnable()
        {
            UserData.OnUserCoinChanged += UserData_OnUserCoinChanged;
            Init();
        }

        private void OnDisable() => UserData.OnUserCoinChanged -= UserData_OnUserCoinChanged;

        private void OnButtonShopClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            UIManager.I.Open(Define.UIName.SHOP_POPUP);
        }

        private void UserData_OnUserCoinChanged(int value) => UpdateCoinText(value);

        private void Init() => UpdateCoinText(UserData.I.Coin);

        private void UpdateCoinText(int value) => _txtCoinAmount.text = $"{value}";
    }
}
