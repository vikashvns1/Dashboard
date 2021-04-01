using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DashBoardModel;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DashBoardAPI.BAL
{
    public class MySqlClass
    {
        public static async Task<List<StoreProcedures>> GetChartsData(string connectionString, List<StoreProcedures> spList)
        {
            try
            {
                var returnObject = new List<dynamic>();
                using (MySqlConnection sql = new MySqlConnection(connectionString))
                {
                    foreach (StoreProcedures sp in spList)
                    {
                        var retObject = new List<ExpandoObject>();
                        CommandType cmdType;
                        if (sp.IsSp == 0)
                            cmdType = CommandType.Text;
                        else
                            cmdType = CommandType.StoredProcedure;

                        using (MySqlCommand cmd = new MySqlCommand(sp.SpOrQuery, sql))
                        {
                            cmd.CommandType = cmdType;
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                        }
                        sp.ChatData = retObject;
                    }
                }

                return spList;
            }
            catch (Exception e)
            {
                string y = e.Message;
                return spList;
            }
        }

    }
}
