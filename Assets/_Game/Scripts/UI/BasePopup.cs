using NFramework;
using UnityEngine;

namespace TenCrush
{
    public class BasePopup : BaseUIView
    {
        [SerializeField] private Animator _animator;

        public override void OnOpen()
        {
            _animator.Play("Open");
            this.InvokeDelayRealtime(0.75f, () =>
            {
                base.OnOpen();
            });
        }

        public override void OnClose()
        {
            _animator.Play("Close");
            this.InvokeDelayRealtime(0.15f, () =>
            {
                base.OnClose();
            });
        }
    }
}
