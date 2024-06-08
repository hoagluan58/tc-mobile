using NFramework.Ads;
using NFramework.IAP;

namespace TenCrush
{
    public class GameIAP : IAPManager
    {
        protected override void ProcessRestore(IAPProductSO iapProductSO)
        {
            base.ProcessRestore(iapProductSO);

            if (iapProductSO.id == Define.IAPProductId.REMOVE_ADS)
                AdsManager.I.IsRemoveAds = true;
            if (iapProductSO.id == Define.IAPProductId.BIG_PACK)
                AdsManager.I.IsRemoveAds = true;
        }

        protected override void ShowLoadingPayment()
        {
            base.ShowLoadingPayment();
        }

        protected override void HideLoadingPayment()
        {
            base.HideLoadingPayment();
        }
    }
}
