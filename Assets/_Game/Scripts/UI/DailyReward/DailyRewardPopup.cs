using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TenCrush
{
    public class DailyRewardPopup : BasePopup
    {
        [SerializeField] private List<DailyRewardItemView> _dailyRewardItems;
        [SerializeField] private DailyRewardSpecialItemView _specialDailyItem;

        public override void OnOpen()
        {
            base.OnOpen();
            Init();
        }

        public void Init()
        {
            for (var i = 0; i < DailyRewardManager.I.Config.DailyRewardConfigDic.Keys.Count; i++)
            {
                var isLastIndex = i == DailyRewardManager.I.Config.DailyRewardConfigDic.Keys.Count - 1;
                var data = DailyRewardManager.I.Config.DailyRewardConfigDic.ElementAt(i).Value;

                if (isLastIndex)
                {
                    _specialDailyItem.Init(data);
                }
                else
                {
                    _dailyRewardItems[i].Init(data);
                }
            }
        }
    }
}
