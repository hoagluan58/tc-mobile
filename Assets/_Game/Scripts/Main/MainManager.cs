using NFramework;
using NFramework.Ads;
using System.Collections;
using UnityEngine;

namespace TenCrush
{
    public class MainManager : SingletonMono<MainManager>
    {
        [SerializeField] private GameObject _ingameDebugConsole;

        private void Start()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;

            StartCoroutine(CRLoadGame());
        }

        public void BackToHomeMenu()
        {
            UIManager.I.CloseAllInLayer(EUILayer.Menu);
            UIManager.I.CloseAllInLayer(EUILayer.Popup);
            UIManager.I.CloseAllInLayer(EUILayer.AlwaysOnTop);
            UIManager.I.Open(Define.UIName.HOME_MENU);
            UIManager.I.Open(Define.UIName.CHEAT_UI);
        }

        public void ToggleDebugConsole()
        {
            var isOn = _ingameDebugConsole.activeSelf;
            _ingameDebugConsole.SetActive(!isOn);
        }

        private IEnumerator CRLoadGame()
        {
            var loadingUI = UIManager.I.Open<LoadingUI>(Define.UIName.LOADING_UI);
            yield return null;

            if (DeviceInfo.IsDevelopment)
            {
                UIManager.I.Open(Define.UIName.CHEAT_UI);
                _ingameDebugConsole.SetActive(true);
            }
            else
            {
                Destroy(_ingameDebugConsole);
            }

            RegisterAndLoadSave();
            InitGameConfig();

            yield return new WaitForSecondsRealtime(0.5f);
            yield return loadingUI.Fill(Random.Range(0.2f, 0.4f), 1f);

            if (!DeviceInfo.IsNoTracking)
            {
                GameTracking.I.Init();
                yield return null;
            }

            GameIAP.I.Init();
            yield return null;

            if (!DeviceInfo.IsNoAds)
            {
                Falcon.FalconGoogleUMP.FalconUMP.ShowConsentForm(consent =>
                {
                    AdsManager.I.ConsentStatus = consent ? EConsentStatus.Yes : EConsentStatus.No;
                    AdsManager.I.Init(EAdsAdapterType.IronSource, GameTracking.I);
                }, () =>
                {
                    AdsManager.I.Init(EAdsAdapterType.AdMob, GameTracking.I);
                }, null);
                yield return new WaitUntil(() => AdsManager.I.ConsentStatus != EConsentStatus.Unknown);
            }

            if (!Application.isEditor)
            {
                yield return new WaitForSecondsRealtime(1f);
                yield return loadingUI.Fill(Random.Range(0.5f, 0.6f), 1f);
                yield return new WaitForSecondsRealtime(1f);
                yield return loadingUI.Fill(Random.Range(0.7f, 0.8f), 1f);
                yield return new WaitForSecondsRealtime(1f);
                yield return loadingUI.Fill(1f, 0.5f);
            }

            loadingUI.CloseSelf(true);
            if (UserData.I.NeedForcePlayTutorial())
            {
                UIManager.I.Open(Define.UIName.HOME_MENU);
                UIManager.I.Open(Define.UIName.TUTORIAL_POPUP);
            }
            else
            {
                UIManager.I.Open(Define.UIName.HOME_MENU);
            }
            GameSound.I.PlayBGM();
        }

        private void RegisterAndLoadSave()
        {
            SaveManager.I.RegisterSaveData(UserData.I);
            SaveManager.I.RegisterSaveData(VibrationManager.I);
            SaveManager.I.RegisterSaveData(SoundManager.I);
            SaveManager.I.RegisterSaveData(DailyRewardManager.I);
            SaveManager.I.RegisterSaveData(AdsManager.I);
            SaveManager.I.Load();
        }

        private void InitGameConfig()
        {
            DailyRewardManager.I.Init();
            ShopManager.I.Init();
        }
    }
}
