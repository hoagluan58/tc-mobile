using DG.Tweening;
using NFramework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TenCrush
{
    public class CellView : MonoBehaviour
    {
        public static event Action<CellView> OnCellViewCleared;

        [SerializeField] private Image _numberBorder;
        [SerializeField] private Image _iceImage;
        [SerializeField] private GameObject _grassCell;
        [SerializeField] private GameObject _selectedCell;
        [SerializeField] private GameObject _highlightCell;
        [SerializeField] private GameObject _tutorialCell;
        [SerializeField] private List<Sprite> _iceCellSprites;
        [SerializeField] private TextMeshProUGUI _txtNumber;
        [SerializeField] private GameObject _goHintParticle;
        [SerializeField] private ParticleSystem _matchNumberParticle;
        [SerializeField] private ParticleSystem _grassClearParticle;
        [SerializeField] private ParticleSystem _breakIceParticle;

        private Dictionary<ECellType, Sprite> _iceCellSpriteDic = new Dictionary<ECellType, Sprite>();
        private Tween _shakeNumberTween;

        public CellData Data { get; set; }
        public RowView RowView { get; set; }

        private void Awake()
        {
            InitIceCellSpriteDic();
        }

        public void Init(CellData data, RowView rowView)
        {
            if (data == null)
                return;

            Data = data;
            RowView = rowView;
            UpdateTextNumber();
            UpdateCellBorder();
            UpdateIceCell();
            UpdateGrassCell();
            ResetCell();

            gameObject.name = $"Cell {data.id}";
        }

        public void UpdateCellBorder() => _numberBorder.sprite = Data.isCleared ? GameSpriteManager.I.GetClearedBorderSprite() : GameSpriteManager.I.GetBorderSprite(Data.number);

        public void UpdateTextNumber()
        {
            var color = Data.isCleared ? GameSpriteManager.I.GetClearedNumberColor() : GameSpriteManager.I.GetNumberColor(Data.number);
            _txtNumber.text = $"<color={color}>{Data.number}";
        }

        public void UpdateGrassCell() => _grassCell.SetActive((Data.cellType == ECellType.Grass || Data.IsIceCell()) && !_iceImage.gameObject.activeSelf);

        public void SetActiveTutorialCell(bool isActive) => _tutorialCell.SetActive(isActive);

        public void UpdateSelectCellUI(bool isSelected)
        {
            _selectedCell.SetActive(isSelected);
            if (isSelected)
            {
                //PlayScaleAnimation();
            }
        }

        public void HighlightDestroyCell(bool isHighlight) => _highlightCell.SetActive(isHighlight);

        public void ResetCell()
        {
            UpdateSelectCellUI(false);
            HighlightDestroyCell(false);
        }

        public void ClearCell()
        {
            switch (Data.cellType)
            {
                case ECellType.Normal:
                    GameSound.I.PlaySFX(Define.SoundName.SFX_NUMBER_MATCH);
                    HandleClearNormalCell();
                    break;
                case ECellType.Grass:
                    GameSound.I.PlaySFX(Define.SoundName.SFX_GRASS_CELL_MATCH);
                    HandleClearNormalCell();
                    HandleClearGrassCell();
                    break;
                case ECellType.Ice1:
                case ECellType.Ice2:
                case ECellType.Ice3:
                case ECellType.Ice4:
                case ECellType.Ice5:
                    BreakIceCell();
                    break;
                default:
                    break;
            }
        }

        public void RefreshView()
        {
            if (Data.isCleared)
                return;

            UpdateIceCell();
            UpdateGrassCell();
            UpdateTextNumber();
            UpdateCellBorder();
        }

        private void HandleClearNormalCell()
        {
            Data.isCleared = true;
            UpdateTextNumber();
            UpdateCellBorder();
            UpdateSelectCellUI(false);
            if (Board.I != null)
            {
                Board.I.TryClearNearbyIceCell(this);
            }
        }

        private void HandleClearGrassCell()
        {
            var imgGrassCell = _grassCell.GetComponent<Image>();
            var flyPos = Vector3.zero;

            if (UIManager.I.IsSpecificViewShown(Define.UIName.GAME_MENU, out var menu))
            {
                var gameMenu = menu as GameMenu;
                flyPos = gameMenu.TargetGroup.GetGrassCellPos();
            }

            _grassCell.gameObject.SetActive(false);
            _grassClearParticle.Play();
            UIManager.I.Open<FlyAnimationPopup>(Define.UIName.FLY_ANIMATION_POPUP).Init(imgGrassCell.sprite, transform.position, flyPos, () =>
            {
                OnCellViewCleared?.Invoke(this);
            });
        }

        public void BreakIceCell()
        {
            Data.BreakIceLayer();
            _breakIceParticle.Play();
            UpdateIceCell();
            UpdateGrassCell();
        }

        public void PlayMatchNumberParticle() => _matchNumberParticle.Play();

        public void SetActiveHintParticle(bool isActive) => _goHintParticle.SetActive(isActive);

        public void ShakeNumber()
        {
            _shakeNumberTween?.Kill();
            var shakePos = new Vector3(0, 0, 30f);
            _shakeNumberTween = _txtNumber.transform.DOShakeRotation(0.5f, shakePos).OnKill(() =>
            {
                _txtNumber.transform.localEulerAngles = Vector3.zero;
            });
        }

        public bool CanSelect()
        {
            if (GameManager.I.CurGameState.Value == EGameState.Tutorial)
                return _tutorialCell.activeSelf;

            return !Data.IsIceCell() && !Data.isCleared;
        }

        private void InitIceCellSpriteDic()
        {
            _iceCellSpriteDic.Add(ECellType.Ice1, _iceCellSprites[0]);
            _iceCellSpriteDic.Add(ECellType.Ice2, _iceCellSprites[1]);
            _iceCellSpriteDic.Add(ECellType.Ice3, _iceCellSprites[2]);
            _iceCellSpriteDic.Add(ECellType.Ice4, _iceCellSprites[3]);
            _iceCellSpriteDic.Add(ECellType.Ice5, _iceCellSprites[4]);
        }

        private void PlayScaleAnimation()
        {
            _txtNumber.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        _txtNumber.transform.DOScale(Vector3.one, 0.2f);
                    });
        }

        private void UpdateIceCell()
        {
            _iceImage.gameObject.SetActive(Data.IsIceCell());
            if (_iceCellSpriteDic.TryGetValue(Data.cellType, out var sprite))
            {
                _iceImage.sprite = sprite;
            }
        }
    }
}
