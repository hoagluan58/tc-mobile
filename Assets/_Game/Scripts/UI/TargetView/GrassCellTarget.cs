using TMPro;
using UnityEngine;

namespace TenCrush
{
    public class GrassCellTarget : MonoBehaviour
    {
        [SerializeField] private Transform _tfGrass;
        [SerializeField] private TextMeshProUGUI _txtGrassCellLeft;

        private TargetData _targetData;

        public void Init(TargetData targetData)
        {
            _targetData = targetData;
            UpdateAmountText();
        }

        public Vector3 GetGrassCellPos() => _tfGrass.position;

        public void UpdateAmountText() => _txtGrassCellLeft.text = $"{_targetData.amount}";
    }
}
