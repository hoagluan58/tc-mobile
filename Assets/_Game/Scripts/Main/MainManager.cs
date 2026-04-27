using System.Collections;
using NFramework;
using UnityEngine;

namespace TenCrush
{
    public class MainManager : SingletonMono<MainManager>
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;

            // Destroy debug console in non-editor builds
#if !UNITY_EDITOR
            var debugConsole = FindObjectOfType<IngameDebugConsole.DebugLogManager>();
            if (debugConsole != null)
                Destroy(debugConsole.gameObject);
#endif

            StartCoroutine(CRLoadGame());
        }

        public void BackToHomeMenu()
        {
            UIManager.I.CloseAllInLayer(EUILayer.Menu);
            UIManager.I.CloseAllInLayer(EUILayer.Popup);
            UIManager.I.CloseAllInLayer(EUILayer.AlwaysOnTop);
            UIManager.I.Open(Define.UIName.HOME_MENU);
        }

        private IEnumerator CRLoadGame()
        {
            var loadingUI = UIManager.I.Open<LoadingUI>(Define.UIName.LOADING_UI);
            yield return null;

            RegisterAndLoadSave();
            InitGameConfig();

            yield return new WaitForSecondsRealtime(0.5f);
            yield return loadingUI.Fill(Random.Range(0.2f, 0.4f), 1f);

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
            SaveManager.I.Load();
        }

        private void InitGameConfig()
        {
            DailyRewardManager.I.Init();
            ShopManager.I.Init();
        }
    }
}
