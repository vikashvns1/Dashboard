using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SynGeniee
{
    public class ExcelConnection
    {
        public static DataTable GetReport(string connectionString)
        {
            DataTable dataTable = new DataTable();
            var returnObject = new List<dynamic>();
            try
            {
                var fileName = "./ExcelData/SapData.xlsx";
                // For .net core, the next line requires the NuGet package, 
                // System.Text.Encoding.CodePages
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    var retObject = new List<dynamic>();
                    using (var dataReader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        var dataSet = dataReader.AsDataSet(conf);

                        // Now you can get data from each sheet by its index or its "name"
                        dataTable = dataSet.Tables[0];
                        //while (dataReader.Read())
                        //{
                        //    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        //    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        //    {
                        //        dataRow.Add(
                        //            dataReader.GetName(iFiled).ToString(),
                        //            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                        //        );
                        //    }
                        //    retObject.Add((ExpandoObject)dataRow);
                        //}
                    }
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                var e = ex.Message.ToString();
                return dataTable;
            }
        }

        public static DataTable ConvertXSLXtoDataTable(string connString,string Qry)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand(Qry, oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            finally
            {
                oledbConn.Close();
            }
            return dt;
        }
    }
}
