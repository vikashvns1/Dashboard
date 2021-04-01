using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DashBoardModel;
using Microsoft.AspNetCore.SignalR;
using Npgsql;
using NpgsqlTypes;

namespace DashBoardAPI.BAL
{
    public class PostgreSQLClass
    {
        public static async Task<OutputData> GetDataSpMasterByParam(string connectionString, string storeProcedureName, string commondName, List<Param> exObj)
        {
            ////string conn = "Server=127.0.0.1;Port=5432;Database=dvdrental;User Id=PostgreSQL;Password=abc123;";
            OutputData _outPut = new OutputData();
            var retObject = new List<ExpandoObject>();
            try
            {
                var returnObject = new List<dynamic>();
                using (NpgsqlConnection sql = new NpgsqlConnection(connectionString))
                {
                    CommandType cmdType;
                    if (commondName == "IsQueraable")
                    {
                        cmdType = CommandType.Text;
                    }
                    else
                    {
                        cmdType = CommandType.StoredProcedure;
                    }
                    using (NpgsqlCommand cmd = new NpgsqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = cmdType;
                        if (exObj != null)
                        {
                            foreach (Param itm in exObj)
                            {
                                if (!string.IsNullOrEmpty(itm.Value))
                                    cmd.Parameters.AddWithValue("@" + itm.Label, itm.Value);
                            }
                        }                    

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

                        _outPut.Msg = (string)cmd.Parameters["@Msg"].Value.ToString();
                        _outPut.DynamicData = retObject;
                        return _outPut;
                    }
                }
            }
            catch (Exception e)
            {
                string y = e.Message;
                return _outPut;
            }
        }
        public static async Task<List<StoreProcedures>> GetChartsData(string connectionString, List<StoreProcedures> spList)
        {
            try
            {
                var returnObject = new List<dynamic>();
                using (NpgsqlConnection sql = new NpgsqlConnection(connectionString))
                {
                    foreach (StoreProcedures sp in spList)
                    {
                        var retObject = new List<ExpandoObject>();
                        CommandType cmdType;
                        if (sp.IsSp == 0)
                            cmdType = CommandType.Text;
                        else
                            cmdType = CommandType.StoredProcedure;

                        using (NpgsqlCommand cmd = new NpgsqlCommand(sp.SpOrQuery, sql))
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
        public static async Task<OutputData> GetFilterDataByParam(string connectionString, Param exObj)
        {
            OutputData _outPut = new OutputData();
            var retObject = new List<ExpandoObject>();
            try
            {
                var returnObject = new List<dynamic>();
                using (NpgsqlConnection sql = new NpgsqlConnection(connectionString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(exObj.QueryOrSp, sql))
                    {
                        cmd.CommandType = exObj.IsSp == 0 ? CommandType.Text : CommandType.StoredProcedure;
                        if (exObj != null)
                        {
                            
                            if (!string.IsNullOrEmpty(exObj.Value1))
                                cmd.Parameters.AddWithValue("@" + exObj.Label1, exObj.Value1);
                            if (!string.IsNullOrEmpty(exObj.Value))
                                cmd.Parameters.AddWithValue("@" + exObj.Label, exObj.Value);
                        }

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
                        _outPut.DynamicData = retObject;
                        return _outPut;
                    }
                }
            }
            catch (Exception e)
            {
                string y = e.Message;
                return _outPut;
            }
        }
        public static async Task<IEnumerable<object>> ExecuteSpGetData(string connectionString, string storeProcedureName, string commondName, int UserID)
        {
            var returnObject = new List<dynamic>();
            using (NpgsqlConnection sql = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(storeProcedureName, sql))
                {
                    cmd.CommandType = commondName == "0" ? CommandType.Text : CommandType.StoredProcedure;

                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@StatementType", SqlDbType.NVarChar) { Value = commondName });

                    if (UserID != 0)
                        cmd.Parameters.Add(new NpgsqlParameter("@CompanyId", SqlDbType.Int) { Value = UserID });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
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
        public static async Task<IEnumerable<object>> GetReport(string connectionString, Param prm)
        {
            var returnObject = new List<dynamic>();
            using (NpgsqlConnection sql = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(prm.QueryOrSp, sql))
                {
                    if (prm.IsSp == 1)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        cmd.CommandType = CommandType.Text;
                    }

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
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
        public static async Task<IEnumerable<object>> GetDataByDateRange(string connectionString, RangeDateParam PramObj)
        {
            var retObject = new List<dynamic>();
            try
            {
                using (NpgsqlConnection sql = new NpgsqlConnection(connectionString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(PramObj.QueryOrSp, sql))
                    {
                        string StartDate = DateTime.Parse(PramObj.StartDate).ToString("yyyy-MM-dd");
                        string EndDate = DateTime.Parse(PramObj.EndDate).ToString("yyyy-MM-dd");

                        cmd.CommandType = PramObj.IsSp == 0 ? CommandType.Text : CommandType.StoredProcedure;
                        cmd.Parameters.Add(new NpgsqlParameter("@startdate", NpgsqlDbType.Varchar) { Value = StartDate});

                        cmd.Parameters.Add(new NpgsqlParameter("@enddate", NpgsqlDbType.Varchar) { Value = EndDate });

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
            catch (Exception ex)
            {
                string str = ex.Message;
                return retObject;
            }
        }
    }
}
