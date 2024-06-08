using NFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TenCrush
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public static event Action<int> OnCurLevelScoreChanged;

        private int _curLevelScore;

        public int CurLevelScore
        {
            get => _curLevelScore;
            set
            {
                _curLevelScore = value;
                OnCurLevelScoreChanged?.Invoke(value);
            }
        }

        public void StartLevelScore()
        {
            CurLevelScore = 0;
        }

        public void CalculateCellDestroyedScore(List<CellView> cells)
        {
            var score = 0;
            foreach (var cellView in cells)
            {
                score += cellView.Data.number;
            }

            if (LevelTargetManager.I.IsHaveScoreTarget())
            {
                var startPos = cells.Last().transform.position;
                PlayFlyScoreAnimation(startPos, score);
            }
            else
            {
                CurLevelScore += score;
            }
        }

        private void PlayFlyScoreAnimation(Vector3 startPos, int score)
        {
            var targetPos = Vector3.zero;
            if (UIManager.I.IsSpecificViewShown(Define.UIName.GAME_MENU, out var view))
            {
                var gameMenu = view as GameMenu;
                targetPos = gameMenu.TargetGroup.GetTargetGroupPos();
            }
            UIManager.I.Open<FlyAnimationPopup>(Define.UIName.FLY_ANIMATION_POPUP).Init($"+{score}", startPos, targetPos, () =>
            {
                CurLevelScore += score;
                LevelTargetManager.I.UpdateLevelTarget(new List<CellView>());
            });
        }
    }
}
