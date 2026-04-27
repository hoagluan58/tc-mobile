using NFramework;
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
        [SerializeField] private NFramework.IAP.IAPProductSO _productSO;

        private IAPData _iapData;

        private void Awake()
        {
            _btnBuy.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnEnable() => RefreshView();

        private void OnBuyButtonClicked()
        {
            GameSound.I.PlayButtonClickSFX();

            if (_iapData == null) return;

            // IAP removed — grant rewards for free
            UserData.I.AddRewardDataToUserData(_iapData.rewards);
            UIManager.I.Open<RewardPopup>(Define.UIName.REWARD_POPUP).Init(_iapData.rewards);
            
            // Apply remove ads if applicable
            if (_productSO.id == Define.IAPProductId.REMOVE_ADS || _productSO.id == Define.IAPProductId.BIG_PACK)
                NFramework.Ads.AdsManager.I.IsRemoveAds = true;
        }

        private void RefreshView()
        {
            _iapData = ShopManager.I.GetIAPData(_productSO.id);
            if (_iapData == null) return;

            _txtName.text = _iapData.id;
            _txtPrice.text = "FREE";
            _rewardItems.ForEach(x => x.gameObject.SetActive(false));
            for (var i = 0; i < _iapData.rewards.Count; i++)
            {
                _rewardItems[i].gameObject.SetActive(true);
                _rewardItems[i].Init(_iapData.rewards[i]);
            }
        }
    }
}
