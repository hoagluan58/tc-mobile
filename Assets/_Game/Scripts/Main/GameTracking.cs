using NFramework;
using System;

namespace TenCrush
{
    public class GameTracking : SingletonMono<GameTracking>
    {
        public void Init() { }

        public void TrackLevelStart() { }

        public void TrackLevelWin(TimeSpan duration) { }

        public void TrackLevelLose(TimeSpan duration, int failCount) { }

        public void TrackEarnVirtualCurrency(int amount, string generalReason = "unknown", string specificReason = "unknown") { }

        public void TrackSpendVirtualCurrency(int amount, string generalReason = "unknown", string specificReason = "unknown") { }
    }
}
