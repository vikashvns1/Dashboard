using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text;

namespace DashBoardModel
{
    public class Dashboard
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
        public int IsFilterOn { get; set; }
        public string Xml { get; set; }
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
        public string MultiAxixName { get; set; }
       
        public List<DataClass> ChartData { get; set; }
        public List<ExpandoObject> TableData { get; set; }
        public DataTable ColumnDataTable { get; set; }
        public List<Param> Filter { get; set; }
        public string GroupKey { get; set; }
        public int CommanSeries { get; set; }
        //
        
    }
}

