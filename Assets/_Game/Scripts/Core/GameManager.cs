using NFramework;
using System;
using UnityEngine;

namespace TenCrush
{
    public enum EGameState
    {
        None,
        Playing,
        Win,
        Lose,
        Tutorial,
    }

    public class GameManager : SingletonMono<GameManager>
    {
        [SerializeField] private LevelConfigSO _levelConfig;

        private LevelData _curLevelData;

        public ObservableValue<EGameState> CurGameState = new ObservableValue<EGameState>();
        public LevelConfigSO LevelConfig { get => _levelConfig; }
        public DateTime LevelStartTime { get; private set; }
        public int FailCount { get; private set; }

        public void PlayLevel(int level)
        {
            _curLevelData = _levelConfig.GetLevelData(level);
            var boardData = BoardData.CreateBoardData(_curLevelData);
            LevelTargetManager.I.CreateLevelTarget(_curLevelData.targets);
            ScoreManager.I.StartLevelScore();
            UIManager.I.Open<GameMenu>(Define.UIName.GAME_MENU).Init(boardData);
            UIManager.I.Open(Define.UIName.LEVEL_TARGET_POPUP);
            HintBoosterManager.I.CheckPossibleMoves();
            CurGameState.Value = EGameState.Playing;
            GameTracking.I.TrackLevelStart();
            LevelStartTime = DateTime.Now;
        }

        public void Win()
        {
            if (!IsPlaying())
                return;

            CurGameState.Value = EGameState.Win;
            FailCount = 0;
            GameTracking.I.TrackLevelWin(DateTime.Now - LevelStartTime);
            UserData.I.CurrentLevel++;
            StartCoroutine(Board.I.PlayWinAnimation(() =>
            {
                this.InvokeDelay(0.5f, () =>
                {
                    GameAds.I.ShowInter(null, Define.UIName.GAME_MENU);
                    UIManager.I.Close(Define.UIName.GAME_MENU);
                    UIManager.I.Open(Define.UIName.WIN_LEVEL_POPUP);
                });
            }));
        }

        public void Lose()
        {
            if (!IsPlaying())
                return;

            CurGameState.Value = EGameState.Lose;
            FailCount++;
            GameTracking.I.TrackLevelLose(DateTime.Now - LevelStartTime, FailCount);
            GameAds.I.ShowInter(null, Define.UIName.GAME_MENU);
            UIManager.I.Close(Define.UIName.GAME_MENU);
            UIManager.I.Open(Define.UIName.LOSE_LEVEL_POPUP);
        }

        public bool IsMaxLevel() => UserData.I.CurrentLevel >= _levelConfig.LevelMax();

        private bool IsPlaying() => CurGameState.Value == EGameState.Playing;
    }
}
