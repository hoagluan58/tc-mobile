using NFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class RewardPopup : BaseUIView
    {
        [SerializeField] private RewardItemView _pfRewardItemView;
        [SerializeField] private RewardItemView _singleRewardItemView;
        [SerializeField] private GameObject _goSingleReward;
        [SerializeField] private GameObject _goMultipleReward;
        [SerializeField] private Button _btnContinue;

        private List<RewardData> _rewards = new List<RewardData>();
        private Action _callback;

        private void Awake() => _btnContinue.onClick.AddListener(OnButtonContinueClicked);

        private void OnButtonContinueClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            StartCoroutine(CRShowFlyRewardAnimation());
        }

        public void Init(RewardData reward, Action callback = null)
        {
            _callback = callback;
            _goSingleReward.SetActive(true);
            _goMultipleReward.SetActive(false);
            _singleRewardItemView.Init(reward);
            _rewards.Clear();
            _rewards.Add(reward);
        }

        public void Init(List<RewardData> rewards, Action callback = null)
        {
            _callback = callback;
            _goSingleReward.SetActive(false);
            _goMultipleReward.SetActive(true);
            _rewards.Clear();
            _rewards.AddRange(rewards);
            ClearChild(_goMultipleReward.transform);
            foreach (var reward in rewards)
            {
                var view = Instantiate(_pfRewardItemView, _goMultipleReward.transform);
                view.Init(reward);
            }
        }

        private void ClearChild(Transform transform)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        protected IEnumerator CRShowFlyRewardAnimation(Action<bool> callback = null, bool isWatchAds = false)
        {
            UIManager.I.DisableInteract(this);
            var popups = new List<BaseUIView>();
            foreach (var reward in _rewards)
            {
                var popup = UIManager.I.Open<FlyAnimationPopup>(Define.UIName.FLY_ANIMATION_POPUP);
                popup.Init(reward, Vector3.zero);
                popups.Add(popup);
            }

            yield return new WaitUntil(() => popups.All(x => x == null || x.gameObject.activeSelf == false));
            UIManager.I.EnableInteract(this);
            _callback?.Invoke();
            callback?.Invoke(isWatchAds);
            CloseSelf();
        }
    }
}
