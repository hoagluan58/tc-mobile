using NFramework;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class CheatUI : BaseUIView
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _goCheatGroup;
        [SerializeField] private Button _btnNextLevel;
        [SerializeField] private Button _btnPrevLevel;
        [SerializeField] private Button _btnCheatBooster;
        [SerializeField] private Button _btnCheatWinLevel;
        [SerializeField] private Button _btnCheatLoseLevel;
        [SerializeField] private Button _btnCheatCoin;
        [SerializeField] private Button _btnToggleGameplayUI;
        [SerializeField] private Button _btnToggleTargetGroup;
        [SerializeField] private Button _btnToggleBoosterGroup;
        [SerializeField] private Button _btnToggleDebugConsole;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
            _btnNextLevel.onClick.AddListener(() => OnButtonCheatLevelClicked(true));
            _btnPrevLevel.onClick.AddListener(() => OnButtonCheatLevelClicked(false));
            _btnCheatBooster.onClick.AddListener(OnButtonCheatBoosterClicked);
            _btnCheatWinLevel.onClick.AddListener(OnButtonCheatWinClicked);
            _btnCheatLoseLevel.onClick.AddListener(OnButtonCheatLoseClicked);
            _btnCheatCoin.onClick.AddListener(OnButtonCheatCoinClicked);
            _btnToggleGameplayUI.onClick.AddListener(OnButtonToggleGameplayUIClicked);
            _btnToggleBoosterGroup.onClick.AddListener(OnButtonToggleBoosterGroupClicked);
            _btnToggleTargetGroup.onClick.AddListener(OnButtonToggleTargetGroupClicked);
            _btnToggleDebugConsole.onClick.AddListener(OnButtonToggleDebugConsoleClicked);
        }

        private void OnButtonToggleDebugConsoleClicked() => MainManager.I.ToggleDebugConsole();

        private void OnButtonToggleTargetGroupClicked()
        {
            if (UIManager.I.IsSpecificViewShown(Define.UIName.GAME_MENU, out var view))
            {
                var gameMenu = view as GameMenu;
                gameMenu.ToggleTargetGroup();
            };
        }

        private void OnButtonToggleBoosterGroupClicked()
        {
            if (UIManager.I.IsSpecificViewShown(Define.UIName.GAME_MENU, out var view))
            {
                var gameMenu = view as GameMenu;
                gameMenu.ToggleBoosterGroup();
            };
        }

        private void OnButtonToggleGameplayUIClicked()
        {
            if (UIManager.I.IsSpecificViewShown(Define.UIName.GAME_MENU, out var view))
            {
                var gameMenu = view as GameMenu;
                gameMenu.ToggleUnneededUI();
            };
        }

        private void OnButtonCheatBoosterClicked()
        {
            foreach (EBoosterType type in Enum.GetValues(typeof(EBoosterType)))
            {
                UserData.I.ModifyBoosterTypeAmount(type, 5);
            }
        }

        private void OnButtonClicked()
        {
            var isActive = _goCheatGroup.activeSelf;
            _goCheatGroup.SetActive(!isActive);
        }

        private void OnButtonCheatLevelClicked(bool isNextLevel)
        {
            if (isNextLevel)
            {
                if (GameManager.I.IsMaxLevel())
                    return;

                UserData.I.CurrentLevel += 1;
            }
            else
            {
                if (UserData.I.CurrentLevel == 1)
                    return;

                UserData.I.CurrentLevel -= 1;
            }
        }

        private void OnButtonCheatWinClicked()
        {
            if (GameManager.I.CurGameState.Value == EGameState.Playing)
            {
                GameManager.I.Win();
            }
        }

        private void OnButtonCheatLoseClicked()
        {
            if (GameManager.I.CurGameState.Value == EGameState.Playing)
            {
                GameManager.I.Lose();
            }
        }

        private void OnButtonCheatCoinClicked() => UserData.I.AddCoin(1000, "cheat", "cheat");

    }
}
