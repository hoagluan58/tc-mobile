using DG.Tweening;
using NFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class LevelTargetPopup : BaseUIView
    {
        [SerializeField] private Image _imgBg;
        [SerializeField] private Image _imgTargetBoard;
        [SerializeField] private TextMeshProUGUI _txtTarget;
        [SerializeField] private TargetGroupView _targetGroupView;

        private Vector3 _targetGroupStartPos;
        private Vector3 _targetGroupStartScale;

        private void Awake()
        {
            _targetGroupStartPos = _targetGroupView.transform.position;
            _targetGroupStartScale = _targetGroupView.transform.localScale;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _targetGroupView.Init();
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            var endPos = new Vector3();
            if (UIManager.I.IsSpecificViewShown(Define.UIName.GAME_MENU, out var view))
            {
                var gameMenu = view as GameMenu;
                endPos = gameMenu.TargetGroup.transform.position;
                gameMenu.TargetGroup.ToggleRoot(false);
            }
            _txtTarget.gameObject.SetActive(true);
            _imgTargetBoard.SetAlpha(1f);
            _imgBg.SetAlpha(0.8f);
            _txtTarget.color = _txtTarget.color.WithAlpha(1f);
            _targetGroupView.transform.position = _targetGroupStartPos;
            _targetGroupView.transform.localScale = _targetGroupStartScale;

            DOTween.Kill(this);

            var seq = DOTween.Sequence();
            seq.SetId(this);
            seq.Insert(1f, _imgBg.DOFade(0f, 1f));
            seq.Insert(1f, _imgTargetBoard.DOFade(0f, 1f));
            seq.Insert(1f, _txtTarget.DOFade(0f, 1f));
            seq.Append(_targetGroupView.transform.DOMove(endPos, 1f));
            seq.Insert(2.85f, _targetGroupView.transform.DOScale(_targetGroupStartScale * 0.5f, 0.15f).SetEase(Ease.Linear));
            seq.OnComplete(() =>
            {
                CloseSelf();
                (view as GameMenu).TargetGroup.ToggleRoot(true);
            });
        }
    }
}
