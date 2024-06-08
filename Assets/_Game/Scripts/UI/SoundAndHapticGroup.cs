using NFramework;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class SoundAndHapticGroup : MonoBehaviour
    {
        [SerializeField] private Button _btnMusicToggle;
        [SerializeField] private Button _btnSFXToggle;
        [SerializeField] private Button _btnHapticToggle;
        [SerializeField] private Button _btnHandToggle;
        [SerializeField] private Button _btnTutorial;
        [SerializeField] private GameObject _goMusicOn;
        [SerializeField] private GameObject _goMusicOff;
        [SerializeField] private GameObject _goSFXOn;
        [SerializeField] private GameObject _goSFXOff;
        [SerializeField] private GameObject _goHapticOn;
        [SerializeField] private GameObject _goHapticOff;

        private void Awake()
        {
            _btnMusicToggle.onClick.AddListener(OnButtonMusicClicked);
            _btnSFXToggle.onClick.AddListener(OnButtonSFXClicked);
            _btnHapticToggle.onClick.AddListener(OnButtonHapticClicked);
            _btnTutorial.onClick.AddListener(OnButtonTutorialClicked);
        }

        private void OnEnable()
        {
            UpdateHapticButton();
            UpdateMusicButton();
            UpdateSFXButton();
        }

        private void OnButtonHapticClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            VibrationManager.I.Status = !VibrationManager.I.Status;
            UpdateHapticButton();
        }

        private void OnButtonSFXClicked()
        {
            SoundManager.I.SFXStatus = !SoundManager.I.SFXStatus;
            GameSound.I.PlayButtonClickSFX();
            UpdateSFXButton();
        }

        private void OnButtonMusicClicked()
        {
            SoundManager.I.MusicStatus = !SoundManager.I.MusicStatus;
            GameSound.I.PlayButtonClickSFX();
            UpdateMusicButton();
        }

        private void OnButtonTutorialClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            UIManager.I.Open(Define.UIName.TUTORIAL_POPUP);
        }

        private void UpdateMusicButton()
        {
            var musicStatus = SoundManager.I.MusicStatus;
            _goMusicOn.SetActive(musicStatus);
            _goMusicOff.SetActive(!musicStatus);
        }

        private void UpdateSFXButton()
        {
            var sfxStatus = SoundManager.I.SFXStatus;
            _goSFXOn.SetActive(sfxStatus);
            _goSFXOff.SetActive(!sfxStatus);
        }

        private void UpdateHapticButton()
        {
            var hapticStatus = VibrationManager.I.Status;
            _goHapticOn.SetActive(hapticStatus);
            _goHapticOff.SetActive(!hapticStatus);
        }
    }
}
