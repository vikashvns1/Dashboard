using DashBoardModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;

namespace Geniee
{
    public static class StaticDataSource
    {
        public static ObservableCollection<ExpandoObject> appConfiguration { get; set; }
        public static ObservableCollection<ExpandoObject> DynamicObject { get; set; }
        public static DataTable DtDynamic { get; set; }
        public static DataTable DtAppConfig { get; set; }
        public static string Xml { get; set; }
        public static List<Param> ParamList { get; set; }
        public static string ConnString { get; set; }
        public static string StoreProcedure { get; set; }
        public static string Action { get; set; }
        public static string Tital { get; set; }
        public static string PopHeadarMsg { get; set; }
        public static string PopMsg { get; set; }

        public static bool IsInitialRenderAppConfig = false;

        public static bool IsInitialRenderChartConfig = false;

        public static bool IsInitialRender = false;

        public static bool IsInitialRenderGrid = false;

        public static bool IsInitialRenderChart = false;

        public static string RowsCount = "0";

        public static string ColumnCount = "0";

        public static ObservableCollection<ExpandoObject> ChartDataObj;

        public static DataTable ChartdataTable;
        public static List<DataClass> chartDataList { get; set; }
        public static List<CompanyProfile> AdminData { get; set; }
        public static List<GetRequestConfiguration> chartAttributesList { get; set; }
        public static PostRequestConfiguration ModelChartRequest { get; set; } = new PostRequestConfiguration();
        public static CompanyProfile userInfo { get; set; }
        public static List<ChartTypeModel> ChartTypeList { get; set; }
        public static List<LegendPositionModel> LegendPositionList { get; set; }
        public static List<BlockPositionModel> BlockPositionData { get; set; }
        public static List<MarkerTypeModel> MarkerTypeList { get; set; }
        public static List<LineTypeModel> LineTypeList { get; set; }
        public static List<ColorSchemeModel> ColorSchemeList { get; set; }
        public static List<CardBgColorModel> CardBgColorList { get; set; }
        public static ChartsFeaturesList ChartsFeaturesListData { get; set; }

        public static bool IsInitialRenderChartFeature = false;
        public static List<MenuItem> MenuItemList { get; set; }
        public static int ComponentId { get; set; }
        public static GetRequestConfiguration Report { get; set; }
        public static IEnumerable<ExpandoObject> MailSendData { get; set; }       
        public static MenuItem MenuItem { get; set; }


        public static List<ExpandoObject> ReportData;
        public static DataTable ReportdataTable;
    }
}
