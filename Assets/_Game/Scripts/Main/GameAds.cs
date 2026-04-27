using NFramework;
using System;

namespace TenCrush
{
    public class GameAds : SingletonMono<GameAds>
    {
        public void ShowReward(Action<bool> callback, string location = "", string rewardType = "")
        {
            // Ads removed — always grant the reward
            callback?.Invoke(true);
        }

        public void ShowInter(Action<bool> callback, string location = "")
        {
            // Ads removed — just invoke callback
            callback?.Invoke(false);
        }

        public void ShowBanner()
        {
            // Ads removed — no-op
        }
    }
}
