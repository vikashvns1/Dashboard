using System.Collections.Generic;

namespace DashBoardModel
{
    public class Param
    {
        public string Control { get; set; }
        public string Label { get; set; }
        public string Label1 { get; set; }
        public string Value1 { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Sp { get; set; }
        public string QueryOrSp { get; set; }
        public int IsSp { get; set; }
        public string XValue { get; set; }
        public string YValue { get; set; }
        public string YValue1 { get; set; }
        public string YValue2 { get; set; }
        public string YValue3 { get; set; }
        public string YValue4 { get; set; }
       
        public string ddlData { get; set; }
        public string ddlValue { get; set; }
        public string Multiple { get; set; }
        public string PlaceHolder { get; set; }
        public string Position { get; set; }
        public string YearRange { get; set; }
        public List<DataClass> ControlData { get; set; }
        public List<DataClass> UpdateChartData { get; set; }
    }
}
