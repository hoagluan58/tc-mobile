using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TenCrush
{
    public class TargetGroupView : MonoBehaviour
    {
        [SerializeField] private GameObject _goClearedTarget;
        [SerializeField] private GameObject _goScoreTarget;
        [SerializeField] private NumberTarget _pfNumberTarget;
        [SerializeField] private GrassCellTarget _pfGrassCellTarget;
        [SerializeField] private TextMeshProUGUI _txtScore;
        [SerializeField] private GameObject _root;

        private List<TargetData> _targetDatas;
        private List<NumberTarget> _numberTargetViews = new List<NumberTarget>();
        private GrassCellTarget _grassCellTargetView;

        private void OnEnable()
        {
            LevelTargetManager.OnUpdateLevelTarget += RefreshView;
            ScoreManager.OnCurLevelScoreChanged += ScoreManager_OnCurLevelScoreChanged;
        }

        private void OnDisable()
        {
            LevelTargetManager.OnUpdateLevelTarget -= RefreshView;
            ScoreManager.OnCurLevelScoreChanged -= ScoreManager_OnCurLevelScoreChanged;
        }

        private void ScoreManager_OnCurLevelScoreChanged(int obj) => RefreshView();

        public void Init()
        {
            _targetDatas = LevelTargetManager.I.CurLevelTargets;
            var isHaveScoreTarget = LevelTargetManager.I.IsHaveScoreTarget();
            _goClearedTarget.SetActive(!isHaveScoreTarget);
            _goScoreTarget.SetActive(isHaveScoreTarget);
            CheckAndSpawnGrassCellTarget();
            CheckAndSpawnNumberTarget();
            RefreshView();
        }

        public Vector3 GetGrassCellPos() => _grassCellTargetView.GetGrassCellPos();

        public Vector3 GetTargetGroupPos() => gameObject.transform.position;

        private void RefreshView()
        {
            _targetDatas = LevelTargetManager.I.CurLevelTargets;
            var isHaveScoreTarget = LevelTargetManager.I.IsHaveScoreTarget();
            if (isHaveScoreTarget)
            {
                UpdateTextScore();
            }
            else
            {
                RefreshGrassCellTargetView();
                RefreshNumberTargetViews();
            }
        }

        private void RefreshGrassCellTargetView()
        {
            if (_grassCellTargetView != null)
            {
                _grassCellTargetView.Init(_targetDatas.Where(x => x.targetType == ETargetType.GrassCell).FirstOrDefault());
            }
        }

        private void CheckAndSpawnGrassCellTarget()
        {
            if (_grassCellTargetView != null)
            {
                Destroy(_grassCellTargetView.gameObject);
                _grassCellTargetView = null;
            }

            foreach (var data in _targetDatas)
            {
                if (data.IsGrassCellTarget())
                {
                    var grassCellTargetView = Instantiate(_pfGrassCellTarget, _goClearedTarget.transform);
                    grassCellTargetView.Init(data);
                    _grassCellTargetView = grassCellTargetView;
                }
            }
        }

        private void RefreshNumberTargetViews()
        {
            var index = 0;
            foreach(var target in _targetDatas)
            {
                if (target.IsMatchNumberTarget())
                {
                    _numberTargetViews[index].Init(target);
                    index++;
                }
            }
        }

        private void CheckAndSpawnNumberTarget()
        {
            _numberTargetViews.ForEach(view => Destroy(view.gameObject));
            _numberTargetViews.Clear();
            foreach (var data in _targetDatas)
            {
                if (data.IsMatchNumberTarget())
                {
                    var numberTargetView = Instantiate(_pfNumberTarget, _goClearedTarget.transform);
                    numberTargetView.Init(data);
                    _numberTargetViews.Add(numberTargetView);
                }
            }
        }

        private void UpdateTextScore() => _txtScore.text = $"SCORE: {ScoreManager.I.CurLevelScore}/{LevelTargetManager.I.GetDataByTargetType(ETargetType.Score).amount}";

        public void ToggleRoot(bool active) => _root.SetActive(active);
    }
}
