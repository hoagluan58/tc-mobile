namespace TenCrush
{
    public static class Define
    {
        public const string TERM_AND_CONDITION_URL = "https://playviet.net/Terms-and-Conditions/";
        public const string PRIVACY_POLICY_URL = "https://playviet.net/Privacy-Policy/";
        public const int MAX_COLUMN_COUNT = 9;
        public const int MAX_NUMBER = 9;
        public const int MATCH_NUMBER = 10;
        public const int WIN_LEVEL_COIN = 50;
        public const int LOSE_BOOSTER_REWARD_AMOUNT = 1;

        public class UIName
        {
            public const string CHEAT_UI = "CheatUI";
            public const string LOADING_UI = "LoadingUI";
            public const string HOME_MENU = "HomeMenu";
            public const string GAME_MENU = "GameMenu";

            public const string LOSE_LEVEL_POPUP = "Popup/LoseLevelPopup";
            public const string WIN_LEVEL_POPUP = "Popup/WinLevelPopup";
            public const string USE_HAMMER_BOOSTER_POPUP = "Popup/UseHammerBoosterPopup";
            public const string USE_BOMB_BOOSTER_POPUP = "Popup/UseBombBoosterPopup";
            public const string FLY_ANIMATION_POPUP = "Popup/FlyAnimationPopup";
            public const string SHOP_POPUP = "Popup/ShopPopup";
            public const string PAUSE_POPUP = "Popup/PausePopup";
            public const string SETTING_POPUP = "Popup/SettingPopup";
            public const string DAILY_REWARD_POPUP = "Popup/DailyRewardPopup";
            public const string REWARD_POPUP = "Popup/RewardPopup";
            public const string X2_REWARD_POPUP = "Popup/X2RewardPopup";
            public const string TUTORIAL_POPUP = "Popup/TutorialPopup";
            public const string TUTORIAL_COMPLETE_POPUP = "Popup/TutorialCompletePopup";
            public const string NOTICE_POPUP = "Popup/NoticePopup";
            public const string LEVEL_TARGET_POPUP = "Popup/LevelTargetPopup";
        }

        public class SoundName
        {
            private const string ROOT = "Sound/";

            public const string BGM_MUSIC_1 = ROOT + "bgm_tc";
            public const string SFX_WIN = ROOT + "sfx_win";
            public const string SFX_LOSE = ROOT + "sfx_lose";
            public const string SFX_BUY = ROOT + "sfx_Buy";
            public const string SFX_CANT_BUY = ROOT + "sfx_Cant_Buy";
            public const string SFX_GET_COIN = ROOT + "sfx_Get_Coin";
            public const string SFX_BUTTON_CLICK = ROOT + "click";
            public const string SFX_NUMBER_MATCH = ROOT + "number_match";
            public const string SFX_GRASS_CELL_MATCH = ROOT + "sfx_match_grass";
            public const string SFX_NUMBER_LINE_MATCH = ROOT + "number_line_match";
            public const string SFX_BOMB_BOOSTER = ROOT + "bomb_1";
        }

        public class IAPProductId
        {
            public const string BIG_PACK = "bigpack";
            public const string REMOVE_ADS = "removeads";
        }

        public class AdjustEventToken
        {
            public const string AJ_CONTENT_ID = "6pfpfc";
            public const string AJ_INTERS_DISPLAYED = "t3182i";
            public const string AJ_INTERS_SHOW = "o4r71y";
            public const string AJ_LEVEL_ACHIEVED = "c8hkdy";
            public const string AJ_LEVEL_FAIL = "cywc18";
            public const string AJ_LEVEL_START = "8ttxk7";
            public const string AJ_PURCHASE = "v5c28v";
            public const string AJ_PURCHASE_ORDERS = "zfg2rw";
            public const string AJ_REWARDED_DISPLAYED = "8c49fy";
            public const string AJ_REWARDED_SHOW = "c9cj7q";
        }
    }
}
