using DashBoardModel;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DashBoardAPI.BAL
{
    public class ExcelData
    {
        public static IEnumerable<object> GetReport(string connectionString, Param prm)
        {
            var returnObject = new List<dynamic>();
            var fileName = "./Users.xlsx"; 
            // For .net core, the next line requires the NuGet package, 
            // System.Text.Encoding.CodePages
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {                
                var retObject = new List<dynamic>();
                using (var dataReader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (dataReader.Read())
                    {
                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        {
                            dataRow.Add(
                                dataReader.GetName(iFiled),
                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                            );
                        }
                        retObject.Add((ExpandoObject)dataRow);
                    }
                }
                return retObject;
            }
        }
    }
}
