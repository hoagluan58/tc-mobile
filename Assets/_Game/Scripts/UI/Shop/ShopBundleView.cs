using NFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class ShopBundleView : MonoBehaviour
    {
        [SerializeField] private RewardItemView _rewardView;
        [SerializeField] private TextMeshProUGUI _txtPrice;
        [SerializeField] private Button _btnBuy;
        [SerializeField] private string _shopItemId;

        private ShopItemData _data;

        private void Awake() => _btnBuy.onClick.AddListener(OnBuyButtonClicked);

        private void OnEnable()
        {
            _data = ShopManager.I.ShopConfig.GetShopItemData(_shopItemId);
            RefreshView();
        }

        private void OnBuyButtonClicked()
        {
            var canBuy = UserData.I.IsEnoughCoin(_data.price);
            if (canBuy)
            {
                GameSound.I.PlaySFX(Define.SoundName.SFX_BUY);
                ShopManager.I.PurchaseShopItem(_shopItemId);
                UIManager.I.Open<RewardPopup>(Define.UIName.REWARD_POPUP).Init(_data.rewards);
            }
            else
            {
                GameSound.I.PlaySFX(Define.SoundName.SFX_CANT_BUY);
            }
        }

        private void RefreshView()
        {
            _rewardView.Init(_data.rewards);
            _txtPrice.text = $"{_data.price}";
        }
    }
}
