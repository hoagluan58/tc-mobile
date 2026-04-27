using NFramework;
#if USE_UNITY_PURCHASING
using NFramework.IAP;
#endif
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class IAPPackageView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtName;
        [SerializeField] private TextMeshProUGUI _txtPrice;
        [SerializeField] private List<RewardItemView> _rewardItems;
        [SerializeField] private Button _btnBuy;
#if USE_UNITY_PURCHASING
        [SerializeField] private IAPProductSO _productSO;
#endif

#if USE_UNITY_PURCHASING
        private IAPData _iapData;
#endif

        private void Awake()
        {
            _btnBuy.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnEnable() => RefreshView();

        private void OnBuyButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();
#if USE_UNITY_PURCHASING
            GameIAP.I.Purchase(_productSO, (result) =>
            {
                if (result)
                {
                    ShopManager.I.ClaimPurchaseReward(_productSO);
                    UIManager.I.Open<RewardPopup>(Define.UIName.REWARD_POPUP).Init(_iapData.rewards);
                }
            });
#endif
        }

        private void RefreshView()
        {
#if USE_UNITY_PURCHASING
            _iapData = ShopManager.I.GetIAPData(_productSO.id);

            _txtName.text = _productSO.id;
            _txtPrice.text = _productSO.PriceString;
            _rewardItems.ForEach(x => x.gameObject.SetActive(false));
            for (var i = 0; i < _iapData.rewards.Count; i++)
            {
                _rewardItems[i].gameObject.SetActive(true);
                _rewardItems[i].Init(_iapData.rewards[i]);
            }
#endif
        }
    }
}
