using DG.Tweening;
using NFramework;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class LoadingUI : BaseUIView
    {
        [SerializeField] private Image _fill;

        public override void OnOpen()
        {
            base.OnOpen();
            _fill.fillAmount = 0f;
        }

        public YieldInstruction Fill(float value, float duration) => 
            _fill.DOFillAmount(value, duration).SetEase(Ease.Linear).SetUpdate(false).WaitForCompletion();
    }
}

