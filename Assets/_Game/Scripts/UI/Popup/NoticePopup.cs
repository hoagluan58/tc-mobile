using NFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class NoticePopup : BaseUIView
    {
        [SerializeField] private Button _btnClose;
        [SerializeField] private TextMeshProUGUI _descTMP;

        private void Awake() => _btnClose.onClick.AddListener(OnButtonCloseClicked);

        public void Init(string desc) => _descTMP.text = desc;

        private void OnButtonCloseClicked()
        {
            GameSound.I.PlayButtonClickSFX();
            CloseSelf();
        }
    }
}
