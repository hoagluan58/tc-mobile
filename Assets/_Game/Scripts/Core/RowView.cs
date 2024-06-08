using System.Collections.Generic;
using UnityEngine;

namespace TenCrush
{
    public class RowView : MonoBehaviour
    {
        [SerializeField] private List<CellView> _cellViews;
        [SerializeField] private ParticleSystem _lineClearParticle;
        [SerializeField] private ParticleSystem _lineClearDustParticle;

        private List<CellData> _cellDatas;
        public int Index { get; private set; }

        public void Init(int index, List<CellData> cellDatas, Dictionary<int, CellView> dictView)
        {
            Index = index;
            _cellDatas = cellDatas;

            for (var i = 0; i < cellDatas.Count; i++)
            {
                var cellData = cellDatas[i];
                var cellView = _cellViews[i];

                cellView.Init(cellData, this);
                cellData.CellView = cellView;
                dictView[cellData.id] = cellView;
            }
        }

        public void PlayClearRowParticle()
        {
            _lineClearParticle.Play();
            _lineClearDustParticle.Play();
        }

        public void Destroy()
        {
            _cellDatas.ForEach(x =>
            {
                Board.I.CellViewDic.Remove(x.id);
            });
            Destroy(gameObject);
        }
    }
}
