using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class NumberTarget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtNumber;
        [SerializeField] private TextMeshProUGUI _txtAmount;
        [SerializeField] private Image _imgBorder;

        private TargetData _targetData;

        public void Init(TargetData targetData)
        {
            _targetData = targetData;
            var number = int.Parse(GetNumberText());
            var txtColor = GameSpriteManager.I.GetNumberColor(number);
            _txtNumber.text = $"<color={txtColor}>{number}";
            _imgBorder.sprite = GameSpriteManager.I.GetBorderSprite(number);
            UpdateAmountText();
        }

        private void UpdateAmountText() => _txtAmount.text = $"{_targetData.amount}";

        private string GetNumberText()
        {
            switch (_targetData.targetType)
            {
                case ETargetType.MatchNumber1:
                    return "1";
                case ETargetType.MatchNumber2:
                    return "2";
                case ETargetType.MatchNumber3:
                    return "3";
                case ETargetType.MatchNumber4:
                    return "4";
                case ETargetType.MatchNumber5:
                    return "5";
                case ETargetType.MatchNumber6:
                    return "6";
                case ETargetType.MatchNumber7:
                    return "7";
                case ETargetType.MatchNumber8:
                    return "8";
                case ETargetType.MatchNumber9:
                    return "9";
                default:
                    return "?";
            }
        }
    }
}
