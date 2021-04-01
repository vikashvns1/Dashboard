using System.Collections.Generic;

namespace DashBoardModel
{
    public class ChartsFeaturesList
    {
        public List<ChartTypeModel> ChartTypeListData { get; set; }
        public List<LegendPositionModel> LegendPositionListData { get; set; }
        public List<BlockPositionModel> BlockPositionListData { get; set; }
        public List<LineTypeModel> LineTypListData { get; set; }
        public List<MarkerTypeModel> MarkerTypeListData { get; set; }
        public List<ColorSchemeModel> ColorSchemeListData { get; set; }
        public List<CardBgColorModel> CardBgColorListData { get; set; }

    }
}
