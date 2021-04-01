using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace DashBoardModel
{
    public class GetRequestConfiguration
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int IsSp { get; set; }
        public string QueryOrSp { get; set; }
        public string ChartType { get; set; }
        public int ChartOrder { get; set; }
        public string BlockPosition { get; set; }
        public int Active { get; set; }
        public string Title { get; set; }
        public string CardBgColor { get; set; }
        public string LegendPosition { get; set; }
        public int IsChartGridLineOn { get; set; }
        public int IsPopUpOpen { get; set; }
        public int IsFilterOn { get; set; }
        public string Xml { get; set; }
        public string ColorScheme { get; set; }
        public string XAxixValueColumnName { get; set; }
        public string YAxixValueColumnName1 { get; set; }
        public string YAxixValueColumnName2 { get; set; }
        public string YAxixValueColumnName3 { get; set; }
        public string YAxixValueColumnName4 { get; set; }
        public string YAxixValueColumnName5 { get; set; }
        public string Label1 { get; set; }
        public string Label2 { get; set; }
        public string Label3 { get; set; }
        public string Label4 { get; set; }
        public string Label5 { get; set; }
        public string XAxixDisplayName { get; set; }
        public string YAxixDisplayName { get; set; }
        public string FormateString { get; set; }
        public string LineType1 { get; set; }
        public string LineType2 { get; set; }
        public string LineType3 { get; set; }
        public string LineType4 { get; set; }
        public string LineType5 { get; set; }
        public string MarkerType1 { get; set; }
        public string MarkerType2 { get; set; }
        public string MarkerType3 { get; set; }
        public string MarkerType4 { get; set; }
        public string MarkerType5 { get; set; }
        public List<DataClass> ChartData { get; set; }
        public List<ExpandoObject> TableData { get; set; }
        public DataTable ColumnDataTable { get; set; }
        public List<Param> Filter { get; set; }
        public string GroupKey { get; set; }
        public int CommanSeries { get; set; }
        //LegendProperties
        public int LegendOrientation { get; set; }
        public int LegendRelativePosition { get; set; }
        public int LegendHorizontalAlignment { get; set; }
        public int LegendVerticalEdge { get; set; }
        public int LegendAllowToggleSeries { get; set; }
        public int LegendVisible { get; set; }
        public int DashBoardNo { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Y1Color { get; set; }
        public string Y2Color { get; set; }
        public string Y3Color { get; set; }
        public string Y4Color { get; set; }
        public string Y5Color { get; set; }

    }
}
