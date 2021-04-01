using DashBoardModel;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace DashBoard
{
    public class IndexBase : ComponentBase
    {
        //[Inject]
        //public IDynamicAPIService dynamicAPIService { get; set; }
        //[Inject]
        //public IConfiguration _configuration { get; set; }
        //protected override async Task OnInitializedAsync()
        //{
        //    if (StaticDataSource.IsInitialRenderChartConfig == false)
        //    {
        //        StaticDataSource.IsInitialRenderChartConfig = true;
        //        string conn = _configuration.GetConnectionString("DashBoardConnection");
        //        string storeProc = "GetConfiguration";

        //        IEnumerable<GetRequestConfiguration> _chartConfigurationList = new List<GetRequestConfiguration>();
        //        _chartConfigurationList = await dynamicAPIService.GetChartsConfiguration(conn, storeProc, StaticDataSource.userInfo.Id);

        //        StaticDataSource.chartAttributesList = _chartConfigurationList.Where(a => a.Active == 1).ToList();

        //        List<StoreProcedures> storeprocslist = StaticDataSource.chartAttributesList.Where(a => a.ChartType != "Report").Select(Result => new StoreProcedures { Id = Result.Id, SpOrQuery = Result.QueryOrSp, IsSp = Result.IsSp }).ToList();
        //        storeprocslist = await dynamicAPIService.GetAllChartsData(storeprocslist, StaticDataSource.userInfo.ConnectionString, StaticDataSource.userInfo.ProviderName);

        //        foreach (GetRequestConfiguration Chartitem in StaticDataSource.chartAttributesList)
        //        {
        //            var ChartDataObj = storeprocslist.Where(a => a.Id == Chartitem.Id).Select(b => b.ChatData).FirstOrDefault();

        //            if (Chartitem.ChartType.ToLower() != "Tab".ToLower() && Chartitem.ChartType.ToLower() != "Report".ToLower() && Chartitem.ChartType.ToLower() != "Data".ToLower() && Chartitem.ChartType.ToLower() != "CardData".ToLower())
        //            {
        //                List<DataClass> chartDataList = new List<DataClass>();
        //                foreach (var inputAttribute in ChartDataObj)
        //                {
        //                    DataClass row = new DataClass();
        //                    foreach (KeyValuePair<string, object> itm in inputAttribute)
        //                    {
        //                        // Create a new dynamic ExpandoObject
        //                        if (itm.Key.ToString() == Chartitem.XAxixValueColumnName)
        //                            row.XData = itm.Value.ToString();
        //                        else if (itm.Key.ToString() == Chartitem.YAxixValueColumnName1)
        //                            row.YData1 = (itm.Value == null) ? 0 : Convert.ToDouble((itm.Value.ToString()));
        //                        else if (itm.Key.ToString() == Chartitem.YAxixValueColumnName2)
        //                            row.YData2 = (itm.Value == null) ? 0 : Convert.ToDouble((itm.Value.ToString()));
        //                        else if (itm.Key.ToString() == Chartitem.YAxixValueColumnName3)
        //                            row.YData3 = (itm.Value == null) ? 0 : Convert.ToDouble((itm.Value.ToString()));
        //                        else if (itm.Key.ToString() == Chartitem.YAxixValueColumnName4)
        //                            row.YData4 = (itm.Value == null) ? 0 : Convert.ToDouble((itm.Value.ToString()));
        //                        else if (itm.Key.ToString() == Chartitem.YAxixValueColumnName5)
        //                            row.YData5 = (itm.Value == null) ? 0 : Convert.ToDouble((itm.Value.ToString()));
        //                        if (Chartitem.ChartType == "Scatter" || Chartitem.ChartType == "Common" || Chartitem.ChartType == "Bubble" || Chartitem.ChartType == "MultiAxis")
        //                        {
        //                            if (itm.Key.ToString() == Chartitem.GroupKey)
        //                                row.GropKey = itm.Value.ToString();
        //                        }
        //                    }
        //                    // Add the object to the dynamic output list
        //                    chartDataList.Add(row);
        //                }
        //                Chartitem.ChartData = chartDataList;
        //            }
        //            else if ((Chartitem.ChartType.ToLower() == "Tab".ToLower() || Chartitem.ChartType.ToLower() == "Data".ToLower()) && Chartitem.ChartType.ToLower() != "CardData".ToLower())
        //            {
        //                Chartitem.TableData = ChartDataObj;
        //                var json = JsonConvert.SerializeObject(ChartDataObj);
        //                Chartitem.ColumnDataTable = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
        //            }
        //            else if (Chartitem.ChartType.ToLower() == "CardData".ToLower() && Chartitem.ChartType.ToLower() != "Data".ToLower())
        //            {
        //                foreach (var inputAttribute in ChartDataObj)
        //                {
        //                    foreach (KeyValuePair<string, object> itm in inputAttribute)
        //                    {
        //                        if (itm.Key.ToString() == Chartitem.XAxixValueColumnName)
        //                            Chartitem.YAxixValueColumnName1 = (itm.Value == null) ? null : itm.Value.ToString().Trim();
        //                    }
        //                }
        //            }

        //            if (Chartitem.Filter != null)
        //            {
        //                foreach (Param prm in Chartitem.Filter)
        //                {
        //                    if (prm.Control == "ddl")
        //                    {
        //                        if (prm.ddlData == "XValue")
        //                        {
        //                            DataClass dt = new DataClass();
        //                            dt.XData = "ALL";
        //                            dt.YData1 = 0; dt.YData2 = 0; dt.YData3 = 0; dt.YData4 = 0; dt.YData5 = 0;
        //                            List<DataClass> dtr = new List<DataClass>();
        //                            if (prm.Multiple == "false")
        //                                dtr.Add(dt);
        //                            dtr.AddRange(Chartitem.ChartData);
        //                            prm.ControlData = dtr;
        //                        }
        //                        else
        //                        {//Change Required in API Function GetAppConfiguration
        //                            Param prm2 = new Param();
        //                            prm2.IsSp = prm.IsSp;
        //                            prm2.QueryOrSp = prm.Sp;

        //                            string strDdl = await dynamicAPIService.GetDatabyParam(StaticDataSource.userInfo.ConnectionString, StaticDataSource.userInfo.ProviderName, prm2);
        //                            var response = JsonConvert.DeserializeObject<OutputData>(strDdl);
        //                            var ddlresponse = new ObservableCollection<ExpandoObject>(response.DynamicData);
        //                            List<DataClass> dtr = new List<DataClass>();
        //                            DataClass dt = new DataClass();
        //                            dt.XData = "ALL";
        //                            dt.YData1 = -1; dt.YData2 = 0; dt.YData3 = 0; dt.YData4 = 0; dt.YData5 = 0;
        //                            dtr.Add(dt);
        //                            foreach (var input in ddlresponse)
        //                            {
        //                                DataClass row = new DataClass();
        //                                foreach (KeyValuePair<string, object> itms in input)
        //                                {
        //                                    // Create a new dynamic ExpandoObject
        //                                    if (itms.Key.ToString() == prm.ddlData)
        //                                        row.XData = itms.Value.ToString();
        //                                    else if (itms.Key.ToString() == prm.ddlValue)
        //                                        row.YData1 = (itms.Value == null) ? 0 : Convert.ToDouble((itms.Value.ToString()));
        //                                }
        //                                // Add the object to the dynamic output list
        //                                dtr.Add(row);
        //                            }
        //                            prm.ControlData = dtr;
        //                        }
        //                    }

        //                }
        //            }
        //        }

        //    }
        //}
    }
}
