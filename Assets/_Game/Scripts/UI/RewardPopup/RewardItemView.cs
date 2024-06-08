using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField] private Image _imgIcon;
        [SerializeField] private TextMeshProUGUI _txtAmount;

        public void Init(RewardData rewardData)
        {
            _imgIcon.sprite = rewardData.icon;
            _txtAmount.text = $"x{rewardData.amount}";
        }

        public void Init(List<RewardData> rewardDatas)
        {
            if (rewardDatas.Count > 0)
                Init(rewardDatas[0]);
        }
    }
}
