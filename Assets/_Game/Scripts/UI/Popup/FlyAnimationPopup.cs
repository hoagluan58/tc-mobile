using AssetKits.ParticleImage;
using DG.Tweening;
using NFramework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class FlyAnimationPopup : BaseUIView
    {
        [SerializeField] private Transform _tfAttraction;
        [SerializeField] private Transform _tfBoosterAttraction;
        [SerializeField] private Image _imgBoosters;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private ParticleImage _particleCoin;

        private Action _callback;

        private void Awake() => _particleCoin.onLastParticleFinish.AddListener(OnParticleCoinFinish);

        private void OnParticleCoinFinish()
        {
            CloseSelf();
            _callback?.Invoke();
        }

        public void Init(Sprite sprite, Vector3 startPos, Vector3 targetPos, Action onFinishCallback = null)
        {
            _tfAttraction.position = targetPos;
            _text.gameObject.SetActive(false);
            _imgBoosters.gameObject.SetActive(false);
            _particleCoin.gameObject.SetActive(false);
            _image.gameObject.SetActive(true);
            _image.transform.position = startPos;
            _image.sprite = sprite;
            _image.transform.DOMove(_tfAttraction.position, 0.5f).SetEase(Ease.Flash).OnComplete(() =>
            {
                onFinishCallback?.Invoke();
                CloseSelf();
            });
        }

        public void Init(string text, Vector3 startPos, Vector3 targetPos, Action onFinishCallback = null)
        {
            _tfAttraction.position = targetPos;
            _image.gameObject.SetActive(false);
            _imgBoosters.gameObject.SetActive(false);
            _particleCoin.gameObject.SetActive(false);
            _text.gameObject.SetActive(true);
            _text.text = text;
            _text.transform.position = startPos;
            _text.transform.DOMove(_tfAttraction.position, 1f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                onFinishCallback?.Invoke();
                CloseSelf();
            });
        }

        public void Init(RewardData reward, Vector3 startPos, Action callback = null)
        {
            _callback = callback;
            _text.gameObject.SetActive(false);
            _image.gameObject.SetActive(false);
            _imgBoosters.gameObject.SetActive(false);
            _particleCoin.gameObject.SetActive(false);

            switch (reward.rewardType)
            {
                case ERewardType.Booster:
                    _imgBoosters.gameObject.SetActive(true);
                    _imgBoosters.sprite = reward.icon;
                    _imgBoosters.transform.position = startPos;
                    _imgBoosters.transform.DOMove(_tfBoosterAttraction.position, 0.5f).SetEase(Ease.Flash).OnComplete(() =>
                    {
                        callback?.Invoke();
                        CloseSelf();
                    });
                    break;
                case ERewardType.Currency:
                    _particleCoin.gameObject.SetActive(true);
                    GameSound.I.PlaySFX(Define.SoundName.SFX_GET_COIN);
                    break;
            }
        }
    }
}
