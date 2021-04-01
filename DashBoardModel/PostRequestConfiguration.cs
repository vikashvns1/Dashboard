namespace DashBoardModel
{
    public class PostRequestConfiguration
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public bool IsSp { get; set; }
        public string QueryOrSp { get; set; }
        public int ChartType { get; set; }
        public int BlockPosition { get; set; }
        public int ChartOrder { get; set; }
        public int ColorScheme { get; set; }
        public string Title { get; set; }
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
        public int CardBgColor { get; set; }
        public bool IsPopUpOpen { get; set; }
        public bool IsChartGridLineOn { get; set; }
        public bool Active { get; set; }
        public bool IsFilterOn { get; set; }
        public string Xml { get; set; }
        //Label 1 Visible
        public bool LineType1 { get; set; }
        //Label 2 Visible
        public bool LineType2 { get; set; }
        //Label 3 Visible
        public bool LineType3 { get; set; }
        //Label 4 Visible
        public bool LineType4 { get; set; }
        //Label 5 Visible
        public bool LineType5 { get; set; }
        //Label 1 Position
        public int MarkerType1 { get; set; }
        //Label 2 Position
        public int MarkerType2 { get; set; }
        //Label 3 Position
        public int MarkerType3 { get; set; }
        //Label 4 Position
        public int MarkerType4 { get; set; }
        //Label 5 Position
        public int MarkerType5 { get; set; }
        //LegendProperties
        public int LegendPosition { get; set; }
        public bool LegendAllowToggleSeries { get; set; }
        public bool LegendVisible { get; set; }
        //Tooltip Enable
        public bool LegendOrientation { get; set; }
        public int LegendRelativePosition { get; set; }
        public int LegendHorizontalAlignment { get; set; }
        public int LegendVerticalEdge { get; set; }
        
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
