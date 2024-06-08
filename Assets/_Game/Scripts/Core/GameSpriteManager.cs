using NFramework;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TenCrush
{
    public class GameSpriteManager : SingletonMono<GameSpriteManager>
    {
        private Dictionary<int, string> _numberColorDic = new Dictionary<int, string>()
        {
            {1, "#FF3DEF"},
            {2, "#FF6064"},
            {3, "#FF8E00"},
            {4, "#D7B101"},
            {5, "#6BE300"},
            {6, "#D7B101"},
            {7, "#FF8E00"},
            {8, "#FF6064"},
            {9, "#FF3DEF"},
        };

        [SerializeField] private SerializableDictionaryBase<EBoosterType, Sprite> _boosterSpriteDic;
        [SerializeField] private SerializableDictionaryBase<ECurrencyType, Sprite> _currencySpriteDic;

        [SerializeField] private List<Sprite> _borderSprites;
        [SerializeField] private Sprite _sprClearedBorder;

        public Sprite GetBorderSprite(int number)
        {
            return _borderSprites[1];
            //return _borderSprites[number - 1];
        }

        public Sprite GetClearedBorderSprite() => _sprClearedBorder;

        public string GetNumberColor(int number)
        {
            if (_numberColorDic.TryGetValue(number, out var colorStr))
            {
                return colorStr;
            }
            else
            {
                return _numberColorDic.First().Value;
            }
        }

        public string GetClearedNumberColor() => "#2E2543";

        public Sprite GetBoosterSprite(EBoosterType type) => _boosterSpriteDic[type];

        public Sprite GetCurrencySprite(ECurrencyType type) => _currencySpriteDic[type];

    }
}
