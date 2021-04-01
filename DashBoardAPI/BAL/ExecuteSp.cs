using DashBoardModel;
using DashBoardModel.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading.Tasks;

namespace DashBoardAPI.BAL
{
    public class ExecuteSp
    {
        public async Task<IEnumerable<object>> ExecuteSpGetData(string connectionString, string storeProcedureName, string commondName, int UserID)
        {
            var returnObject = new List<dynamic>();
            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                {
                    cmd.CommandType = commondName == "0" ? CommandType.Text : CommandType.StoredProcedure;

                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@StatementType", SqlDbType.NVarChar) { Value = commondName });

                    if (UserID != 0)
                        cmd.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.Int) { Value = UserID });

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

        public async Task<IEnumerable<object>> GetReport(string connectionString, Param prm)
        {
            var returnObject = new List<dynamic>();
            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(prm.QueryOrSp, sql))
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

        public async Task<OutputData> ExecuteSpMasterData(string connectionString, string storeProcedureName, string commondName, ExpandoObject exObj)
        {
            OutputData _outPut = new OutputData();
            var retObject = new List<ExpandoObject>();
            try
            {
                var returnObject = new List<dynamic>();
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (exObj != null)
                        {
                            foreach (KeyValuePair<string, object> itm in exObj)
                            {
                                if (itm.Key.ToString().ToUpper() != "BlazId".ToUpper())
                                {
                                    if (itm.Value == null)
                                        cmd.Parameters.AddWithValue("@" + itm.Key, itm.Value);
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@" + itm.Key, itm.Value.ToString());
                                    }
                                }
                            }
                        }
                        cmd.Parameters.AddWithValue("@StatementType", commondName);
                        cmd.Parameters.Add("@Msg", SqlDbType.NVarChar, 50);
                        cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

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
        public async Task<OutputData> GetDataSpMasterByParam(string connectionString, string storeProcedureName, string commondName, List<Param> exObj)
        {
            OutputData _outPut = new OutputData();
            var retObject = new List<ExpandoObject>();
            try
            {
                var returnObject = new List<dynamic>();
                using (SqlConnection sql = new SqlConnection(connectionString))
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

                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
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
                        //cmd.Parameters.AddWithValue("@StatementType", commondName);
                        //cmd.Parameters.Add("@Msg", SqlDbType.NVarChar, 50);
                        //cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;
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

                        //_outPut.Msg = (string)cmd.Parameters["@Msg"].Value.ToString();

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
        public async Task<OutputData> GetFilterDataByParam(string connectionString, Param exObj)
        {
            OutputData _outPut = new OutputData();
            var retObject = new List<ExpandoObject>();
            try
            {
                var returnObject = new List<dynamic>();
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(exObj.QueryOrSp, sql))
                    {
                        cmd.CommandType = exObj.IsSp == 0 ? CommandType.Text : CommandType.StoredProcedure;
                        if (exObj != null)
                        {
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
        public CompanyProfile GetCompanyData(string connectionString, string storeProcedureName, string userId, string password)
        {
            CompanyProfile _userInfo = new CompanyProfile();

            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = userId });
                    cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = password });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            _userInfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                            _userInfo.Name = (dataReader["Name"].GetType() != typeof(DBNull)) ? dataReader["Name"].ToString() : null;
                            _userInfo.UserId = dataReader["UserId"].ToString();
                            _userInfo.Password = dataReader["Password"].ToString();
                            _userInfo.EmailId = (dataReader["EmailId"].GetType() != typeof(DBNull)) ? dataReader["EmailId"].ToString() : null;
                            _userInfo.Address = (dataReader["Address"].GetType() != typeof(DBNull)) ? dataReader["Address"].ToString() : null;
                            _userInfo.ContanctNumber = (dataReader["ContanctNumber"].GetType() != typeof(DBNull)) ? dataReader["ContanctNumber"].ToString() : null;
                            _userInfo.ContactPerson = (dataReader["ContactPerson"].GetType() != typeof(DBNull)) ? dataReader["ContactPerson"].ToString() : null;
                            _userInfo.Plan = (dataReader["Plan"].GetType() != typeof(DBNull)) ? dataReader["Plan"].ToString() : null;
                            _userInfo.Paid = (dataReader["Paid"].GetType() != typeof(DBNull)) ? dataReader["Paid"].ToString() : null;
                            _userInfo.Active = Convert.ToInt32(dataReader["Active"].ToString())==1? true:false;
                            _userInfo.ImageName = (dataReader["ImageName"].GetType() != typeof(DBNull)) ? dataReader["ImageName"].ToString() : null;
                            _userInfo.ImageUrl = (dataReader["ImageUrl"].GetType() != typeof(DBNull)) ? dataReader["ImageUrl"].ToString() : null;
                            _userInfo.BgColorCode = (dataReader["BgColorCode"].GetType() != typeof(DBNull)) ? dataReader["BgColorCode"].ToString() : null;
                            _userInfo.ConnectionString = (dataReader["ConectionString"].GetType() != typeof(DBNull)) ? dataReader["ConectionString"].ToString() : null;
                            _userInfo.ProviderName = (dataReader["ProviderName"].GetType() != typeof(DBNull)) ? dataReader["ProviderName"].ToString() : null;
                            //_userInfo.CreateDate = dataReader["CreateDate"].ToString();
                            _userInfo.Optional1 = (dataReader["Option1"].GetType() != typeof(DBNull)) ? dataReader["Option1"].ToString() : null;
                            _userInfo.Optional2 = (dataReader["Option2"].GetType() != typeof(DBNull)) ? dataReader["Option2"].ToString() : null;
                            _userInfo.FromMail = (dataReader["UserEmail"].GetType() != typeof(DBNull)) ? dataReader["UserEmail"].ToString() : null;
                            _userInfo.EmailPassword = (dataReader["UserPassword"].GetType() != typeof(DBNull)) ? dataReader["UserPassword"].ToString() : null; 
                            _userInfo.SmtpServer = (dataReader["SmtpServer"].GetType() != typeof(DBNull)) ? dataReader["SmtpServer"].ToString() : null;   
                            _userInfo.Port = (dataReader["Port"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Port"].ToString()) : 0;
                            _userInfo.ToMail = (dataReader["ToEmails"].GetType() != typeof(DBNull)) ? dataReader["ToEmails"].ToString() : null;
                        }
                    }
                }
            }
            return _userInfo;
        }

        public async Task<IEnumerable<GetRequestConfiguration>> GeChartConfigurationtData(string connectionString, string storeProcedureName, int UserID)
        {
            List<GetRequestConfiguration> _ChartConfigurationList = new List<GetRequestConfiguration>();
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.Int) { Value = UserID });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                GetRequestConfiguration _ChartConfiguration = new GetRequestConfiguration();
                                _ChartConfiguration.Id = (dataReader["Id"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Id"].ToString()) : 0;
                                _ChartConfiguration.QueryOrSp = (dataReader["QueryOrSp"].GetType() != typeof(DBNull)) ? dataReader["QueryOrSp"].ToString().Trim() : null;
                                _ChartConfiguration.IsSp = (dataReader["IsSp"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["IsSp"].ToString().Trim()) : 0;
                                _ChartConfiguration.ChartType = (dataReader["ChartType"].GetType() != typeof(DBNull)) ? dataReader["ChartType"].ToString().Trim() : null;
                                _ChartConfiguration.ChartOrder = (dataReader["ChartOrder"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["ChartOrder"].ToString().Trim()) : 0;
                                _ChartConfiguration.BlockPosition = (dataReader["BlockPosition"].GetType() != typeof(DBNull)) ? dataReader["BlockPosition"].ToString().Trim() : null;
                                _ChartConfiguration.Active = (dataReader["Active"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Active"].ToString().Trim()) : 0;
                                _ChartConfiguration.Title = (dataReader["Title"].GetType() != typeof(DBNull)) ? dataReader["Title"].ToString().Trim() : null;
                                _ChartConfiguration.CardBgColor = (dataReader["CardBgColor"].GetType() != typeof(DBNull)) ? dataReader["CardBgColor"].ToString().Trim() : null;
                                _ChartConfiguration.LegendPosition = (dataReader["LegendPosition"].GetType() != typeof(DBNull)) ? dataReader["LegendPosition"].ToString().Trim() : null;
                                _ChartConfiguration.IsPopUpOpen = (dataReader["IsPopUpOpen"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["IsPopUpOpen"].ToString().Trim()) : 0;
                                _ChartConfiguration.IsFilterOn = (dataReader["IsFilterOn"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["IsFilterOn"].ToString().Trim()) : 0;
                                _ChartConfiguration.Filter = (dataReader["Xml"].GetType() != typeof(DBNull)) ? Comman.ReadXml(dataReader["Xml"].ToString().Trim()) : null;
                                _ChartConfiguration.ColorScheme = (dataReader["ColorScheme"].GetType() != typeof(DBNull)) ? dataReader["ColorScheme"].ToString().Trim() : null;
                                _ChartConfiguration.XAxixValueColumnName = (dataReader["XAxixValueColumnName"].GetType() != typeof(DBNull)) ? dataReader["XAxixValueColumnName"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName1 = (dataReader["YAxixValueColumnName1"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName1"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName2 = (dataReader["YAxixValueColumnName2"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName2"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName3 = (dataReader["YAxixValueColumnName3"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName3"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName4 = (dataReader["YAxixValueColumnName4"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName4"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName5 = (dataReader["YAxixValueColumnName5"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName5"].ToString().Trim() : null;
                                _ChartConfiguration.Label1 = (dataReader["Label1"].GetType() != typeof(DBNull)) ? dataReader["Label1"].ToString().Trim() : null;
                                _ChartConfiguration.Label2 = (dataReader["Label2"].GetType() != typeof(DBNull)) ? dataReader["Label2"].ToString().Trim() : null;
                                _ChartConfiguration.Label3 = (dataReader["Label3"].GetType() != typeof(DBNull)) ? dataReader["Label3"].ToString().Trim() : null;
                                _ChartConfiguration.Label4 = (dataReader["Label4"].GetType() != typeof(DBNull)) ? dataReader["Label4"].ToString().Trim() : null;
                                _ChartConfiguration.Label5 = (dataReader["Label5"].GetType() != typeof(DBNull)) ? dataReader["Label5"].ToString().Trim() : null;
                                _ChartConfiguration.XAxixDisplayName = (dataReader["XAxixDisplayName"].GetType() != typeof(DBNull)) ? dataReader["XAxixDisplayName"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixDisplayName = (dataReader["YAxixDisplayName"].GetType() != typeof(DBNull)) ? dataReader["YAxixDisplayName"].ToString().Trim() : null;
                                _ChartConfiguration.FormateString = (dataReader["FormateString"].GetType() != typeof(DBNull)) ? dataReader["FormateString"].ToString().Trim() : "{value}";
                                _ChartConfiguration.LineType1 = (dataReader["LineType1"].GetType() != typeof(DBNull)) ? dataReader["LineType1"].ToString().Trim() : null;
                                _ChartConfiguration.LineType2 = (dataReader["LineType2"].GetType() != typeof(DBNull)) ? dataReader["LineType2"].ToString().Trim() : null;
                                _ChartConfiguration.LineType3 = (dataReader["LineType3"].GetType() != typeof(DBNull)) ? dataReader["LineType3"].ToString().Trim() : null;
                                _ChartConfiguration.LineType4 = (dataReader["LineType4"].GetType() != typeof(DBNull)) ? dataReader["LineType4"].ToString().Trim() : null;
                                _ChartConfiguration.LineType5 = (dataReader["LineType5"].GetType() != typeof(DBNull)) ? dataReader["LineType5"].ToString().Trim() : null;
                                _ChartConfiguration.MarkerType1 = (dataReader["MarkerType1"].GetType() != typeof(DBNull)) ? dataReader["MarkerType1"].ToString().Trim() : null;
                                _ChartConfiguration.MarkerType2 = (dataReader["MarkerType2"].GetType() != typeof(DBNull)) ? dataReader["MarkerType2"].ToString().Trim() : null;
                                _ChartConfiguration.MarkerType3 = (dataReader["MarkerType3"].GetType() != typeof(DBNull)) ? dataReader["MarkerType3"].ToString().Trim() : null;
                                _ChartConfiguration.MarkerType4 = (dataReader["MarkerType4"].GetType() != typeof(DBNull)) ? dataReader["MarkerType4"].ToString().Trim() : null;
                                _ChartConfiguration.MarkerType5 = (dataReader["MarkerType5"].GetType() != typeof(DBNull)) ? dataReader["MarkerType5"].ToString().Trim() : null;
                                _ChartConfiguration.Xml = (dataReader["Xml"].GetType() != typeof(DBNull)) ? dataReader["Xml"].ToString().Trim() : null;
                                _ChartConfiguration.GroupKey = (dataReader["GroupKey"].GetType() != typeof(DBNull)) ? dataReader["GroupKey"].ToString().Trim() : null;
                                _ChartConfiguration.CommanSeries = (dataReader["CommanSeries"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["CommanSeries"].ToString()) : 0;
                                _ChartConfiguration.LegendOrientation = (dataReader["LegendOrientation"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["LegendOrientation"].ToString()) : 0;
                                _ChartConfiguration.LegendRelativePosition = (dataReader["LegendRelativePosition"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["LegendRelativePosition"].ToString()) : 1;
                                _ChartConfiguration.LegendHorizontalAlignment = (dataReader["LegendHorizontalAlignment"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["LegendHorizontalAlignment"].ToString()) : 0;
                                _ChartConfiguration.LegendVerticalEdge = (dataReader["LegendVerticalEdge"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["LegendVerticalEdge"].ToString()) : 1;
                                _ChartConfiguration.LegendAllowToggleSeries = (dataReader["LegendAllowToggleSeries"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["LegendAllowToggleSeries"].ToString()) : 1;
                                _ChartConfiguration.LegendVisible = (dataReader["LegendVisible"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["LegendVisible"].ToString()) : 1;
                                _ChartConfiguration.DashBoardNo = (dataReader["DashBoardNo"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["DashBoardNo"].ToString()) : 0;
                                _ChartConfiguration.Width = (dataReader["Width"].GetType() != typeof(DBNull)) ? dataReader["Width"].ToString().Trim() : "100%";
                                _ChartConfiguration.Height = (dataReader["Height"].GetType() != typeof(DBNull)) ? dataReader["Height"].ToString().Trim() : "100%";
                                _ChartConfiguration.Y1Color = (dataReader["Y1Color"].GetType() != typeof(DBNull)) ? dataReader["Y1Color"].ToString().Trim() : "Green";
                                _ChartConfiguration.Y2Color = (dataReader["Y2Color"].GetType() != typeof(DBNull)) ? dataReader["Y2Color"].ToString().Trim() : "Blue";
                                _ChartConfiguration.Y3Color = (dataReader["Y3Color"].GetType() != typeof(DBNull)) ? dataReader["Y3Color"].ToString().Trim() : "Red";
                                _ChartConfiguration.Y4Color = (dataReader["Y4Color"].GetType() != typeof(DBNull)) ? dataReader["Y4Color"].ToString().Trim() : "Blue";
                                _ChartConfiguration.Y5Color = (dataReader["Y5Color"].GetType() != typeof(DBNull)) ? dataReader["Y5Color"].ToString().Trim() : "Blue";

                                _ChartConfigurationList.Add(_ChartConfiguration);

                            }

                        }
                    }
                }
                return _ChartConfigurationList;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return _ChartConfigurationList;
            }
        }

        public async Task<List<StoreProcedures>> GetChartsData(string connectionString, List<StoreProcedures> spList)
        {
            try
            {
                var returnObject = new List<dynamic>();
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    foreach (StoreProcedures sp in spList)
                    {
                        var retObject = new List<ExpandoObject>();
                        CommandType cmdType;
                        if (sp.IsSp == 0)
                            cmdType = CommandType.Text;
                        else
                            cmdType = CommandType.StoredProcedure;

                        using (SqlCommand cmd = new SqlCommand(sp.SpOrQuery, sql))
                        {
                            cmd.CommandType = cmdType;
                            // cmd.Parameters.Add("@Msg", SqlDbType.NVarChar, 50);
                            // cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;
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

        public async Task<List<CompanyProfile>> GetAdminData(string connectionString, string storeProcedureName)
        {
            List<CompanyProfile> AdminInfo = new List<CompanyProfile>();
            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = userId });
                    //cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = password });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            CompanyProfile _userInfo = new CompanyProfile();
                            _userInfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                            _userInfo.Name = (dataReader["Name"].GetType() != typeof(DBNull)) ? dataReader["Name"].ToString() : null;
                            _userInfo.UserId = dataReader["UserId"].ToString();
                            _userInfo.Password = dataReader["Password"].ToString();
                            _userInfo.EmailId = (dataReader["EmailId"].GetType() != typeof(DBNull)) ? dataReader["EmailId"].ToString() : null;
                            _userInfo.Address = (dataReader["Address"].GetType() != typeof(DBNull)) ? dataReader["Address"].ToString() : null;
                            _userInfo.ContanctNumber = (dataReader["ContanctNumber"].GetType() != typeof(DBNull)) ? dataReader["ContanctNumber"].ToString() : null;
                            _userInfo.ContactPerson = (dataReader["ContactPerson"].GetType() != typeof(DBNull)) ? dataReader["ContactPerson"].ToString() : null;
                            _userInfo.Plan = (dataReader["Plan"].GetType() != typeof(DBNull)) ? dataReader["Plan"].ToString() : null;
                            _userInfo.Paid = (dataReader["Paid"].GetType() != typeof(DBNull)) ? dataReader["Paid"].ToString() : null;
                            _userInfo.Active = Convert.ToInt32(dataReader["Active"].ToString())==1?true:false;
                            _userInfo.ImageName = (dataReader["ImageName"].GetType() != typeof(DBNull)) ? dataReader["ImageName"].ToString() : null;
                            _userInfo.ImageUrl = (dataReader["ImageUrl"].GetType() != typeof(DBNull)) ? dataReader["ImageUrl"].ToString() : null;
                            _userInfo.BgColorCode = (dataReader["BgColorCode"].GetType() != typeof(DBNull)) ? dataReader["BgColorCode"].ToString() : null;
                            _userInfo.ConnectionString = (dataReader["ConectionString"].GetType() != typeof(DBNull)) ? dataReader["ConectionString"].ToString() : null;
                            _userInfo.ProviderName = (dataReader["ProviderName"].GetType() != typeof(DBNull)) ? dataReader["ProviderName"].ToString() : null;
                            //_userInfo.CreateDate = dataReader["CreateDate"].ToString();
                            _userInfo.Optional1 = (dataReader["Option1"].GetType() != typeof(DBNull)) ? dataReader["Option1"].ToString() : null;
                            _userInfo.Optional2 = (dataReader["Option2"].GetType() != typeof(DBNull)) ? dataReader["Option2"].ToString() : null;
                            _userInfo.FromMail = (dataReader["UserEmail"].GetType() != typeof(DBNull)) ? dataReader["UserEmail"].ToString() : null;
                            _userInfo.EmailPassword = (dataReader["UserPassword"].GetType() != typeof(DBNull)) ? dataReader["UserPassword"].ToString() : null;
                            _userInfo.SmtpServer = (dataReader["SmtpServer"].GetType() != typeof(DBNull)) ? dataReader["SmtpServer"].ToString() : null;
                            _userInfo.Port = (dataReader["Port"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Port"].ToString()) : 0;
                            _userInfo.ToMail = (dataReader["ToEmails"].GetType() != typeof(DBNull)) ? dataReader["ToEmails"].ToString() : null;
                            AdminInfo.Add(_userInfo);
                        }
                    }
                }
            }
            return AdminInfo;
        }

        public async Task<ChartsFeaturesList> GetChartsFeatureData(string connectionString)
        {
            ChartsFeaturesList ChartsFeaturesInfo = new ChartsFeaturesList();
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetChartsType", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        List<ChartTypeModel> _ChartTypeModelList = new List<ChartTypeModel>();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                ChartTypeModel _ChartTypeModelfo = new ChartTypeModel();
                                _ChartTypeModelfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                _ChartTypeModelfo.ChartType = dataReader["ChartType"].ToString();

                                _ChartTypeModelList.Add(_ChartTypeModelfo);
                            }
                        }
                        ChartsFeaturesInfo.ChartTypeListData = _ChartTypeModelList;
                    }

                    using (SqlCommand cmd = new SqlCommand("GetBlockPositionData", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        List<BlockPositionModel> _BlockPositionModelList = new List<BlockPositionModel>();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                BlockPositionModel _BlockPositionModelinfo = new BlockPositionModel();
                                _BlockPositionModelinfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                _BlockPositionModelinfo.BlockPosition = dataReader["BlockPosition"].ToString();

                                _BlockPositionModelList.Add(_BlockPositionModelinfo);
                            }
                        }
                        ChartsFeaturesInfo.BlockPositionListData = _BlockPositionModelList;
                    }

                    using (SqlCommand cmd = new SqlCommand("GetLegendpositonData", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        List<LegendPositionModel> _LegendPositionModelList = new List<LegendPositionModel>();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                LegendPositionModel _LegendPositionModelinfo = new LegendPositionModel();
                                _LegendPositionModelinfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                _LegendPositionModelinfo.LegendPosition = dataReader["LegendPosition"].ToString();

                                _LegendPositionModelList.Add(_LegendPositionModelinfo);
                            }
                        }
                        ChartsFeaturesInfo.LegendPositionListData = _LegendPositionModelList;
                    }

                    using (SqlCommand cmd = new SqlCommand("GetLineTypeData", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        List<LineTypeModel> _LineTypeModelList = new List<LineTypeModel>();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                LineTypeModel _LineTypeModelinfo = new LineTypeModel();
                                _LineTypeModelinfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                _LineTypeModelinfo.LineType = dataReader["LineType"].ToString();

                                _LineTypeModelList.Add(_LineTypeModelinfo);
                            }
                        }
                        ChartsFeaturesInfo.LineTypListData = _LineTypeModelList;
                    }

                    using (SqlCommand cmd = new SqlCommand("GetMarketTypeData", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        List<MarkerTypeModel> _MarkerTypeModelList = new List<MarkerTypeModel>();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                MarkerTypeModel _MarkerTypeModelinfo = new MarkerTypeModel();
                                _MarkerTypeModelinfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                _MarkerTypeModelinfo.MarkerType = dataReader["MarkerType"].ToString();

                                _MarkerTypeModelList.Add(_MarkerTypeModelinfo);
                            }
                        }
                        ChartsFeaturesInfo.MarkerTypeListData = _MarkerTypeModelList;
                    }

                    using (SqlCommand cmd = new SqlCommand("GetColorSchemeData", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        List<ColorSchemeModel> _ColorSchemeModelList = new List<ColorSchemeModel>();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                ColorSchemeModel _ColorSchemeModelinfo = new ColorSchemeModel();
                                _ColorSchemeModelinfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                _ColorSchemeModelinfo.ColorScheme = dataReader["ColorScheme"].ToString();

                                _ColorSchemeModelList.Add(_ColorSchemeModelinfo);
                            }
                        }
                        ChartsFeaturesInfo.ColorSchemeListData = _ColorSchemeModelList;
                    }

                    using (SqlCommand cmd = new SqlCommand("GetCardBgColorData", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        List<CardBgColorModel> _CardBgColorModelList = new List<CardBgColorModel>();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                CardBgColorModel _CardBgColorModelinfo = new CardBgColorModel();
                                _CardBgColorModelinfo.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                _CardBgColorModelinfo.CardBgColor = dataReader["CardBgColor"].ToString();

                                _CardBgColorModelList.Add(_CardBgColorModelinfo);
                            }
                        }
                        ChartsFeaturesInfo.CardBgColorListData = _CardBgColorModelList;
                    }
                }
                return ChartsFeaturesInfo;
            }
            catch (Exception e)
            {
                string y = e.Message;
                return ChartsFeaturesInfo;
            }
        }

        public async Task<int> UpdateInsertChartConfigurationtData(string connectionString, string storeProcedureName, PostRequestConfiguration exObj)
        {
            int _outPut = 0;
            int retval = -2;
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", exObj.Id);
                        cmd.Parameters.AddWithValue("@CompanyId", exObj.CompanyId);//[CompanyId]  
                        cmd.Parameters.AddWithValue("@QueryOrSp", exObj.QueryOrSp);//,[QueryOrSp]
                        cmd.Parameters.AddWithValue("@IsSp", exObj.IsSp);//,[IsSp]
                        cmd.Parameters.AddWithValue("@ChartType", exObj.ChartType);//,[ChartType]
                        cmd.Parameters.AddWithValue("@Position", exObj.BlockPosition);//,[Position]
                        cmd.Parameters.AddWithValue("@ChartOrder", exObj.ChartOrder);//,[ChartOrder]
                        cmd.Parameters.AddWithValue("@Title", exObj.Title);//,[Title]
                        cmd.Parameters.AddWithValue("@XAxixValueColumnName", exObj.XAxixValueColumnName);//,[XAxixValueColumnName]
                        cmd.Parameters.AddWithValue("@YAxixValueColumnName1", exObj.YAxixValueColumnName1);//,[YAxixValueColumnName1]
                        cmd.Parameters.AddWithValue("@YAxixValueColumnName2", exObj.YAxixValueColumnName2);//,[YAxixValueColumnName2]
                        cmd.Parameters.AddWithValue("@YAxixValueColumnName3", exObj.YAxixValueColumnName3);//,[YAxixValueColumnName3]
                        cmd.Parameters.AddWithValue("@YAxixValueColumnName4", exObj.YAxixValueColumnName4);//,[YAxixValueColumnName4]
                        cmd.Parameters.AddWithValue("@YAxixValueColumnName5", exObj.YAxixValueColumnName5);//,[YAxixValueColumnName5]
                        cmd.Parameters.AddWithValue("@Label1", exObj.Label1);//,[Label1]
                        cmd.Parameters.AddWithValue("@Label2", exObj.Label2);//,[Label2]
                        cmd.Parameters.AddWithValue("@Label3", exObj.Label3);//,[Label3]
                        cmd.Parameters.AddWithValue("@Label4", exObj.Label4);//,[Label4]
                        cmd.Parameters.AddWithValue("@Label5", exObj.Label5);//,[Label5]
                        cmd.Parameters.AddWithValue("@XAxixDisplayName", exObj.XAxixDisplayName);//,[XAxixDisplayName]
                        cmd.Parameters.AddWithValue("@YAxixDisplayName", exObj.YAxixDisplayName);//,[YAxixDisplayName]
                        cmd.Parameters.AddWithValue("@FormateString", exObj.FormateString);//,[FormateString]
                        cmd.Parameters.AddWithValue("@CardBgColor", exObj.CardBgColor);//,[CardBgColor]
                        cmd.Parameters.AddWithValue("@IsPopUpOpen", exObj.IsPopUpOpen);//,[IsPopUpOpen]
                        cmd.Parameters.AddWithValue("@LegendPosition", exObj.LegendPosition);//,[LegendPosition]
                        cmd.Parameters.AddWithValue("@Active", exObj.Active);//,[Active]
                        cmd.Parameters.AddWithValue("@IsFilterOn", exObj.IsFilterOn);//,[IsFilterOn]
                        cmd.Parameters.AddWithValue("@Xml", exObj.Xml);//,[Xml]
                        cmd.Parameters.AddWithValue("@LineType1", exObj.LineType1);//,[LineType1]
                        cmd.Parameters.AddWithValue("@LineType2", exObj.LineType2);//,[LineType2]
                        cmd.Parameters.AddWithValue("@LineType3", exObj.LineType3);//,[LineType3]
                        cmd.Parameters.AddWithValue("@LineType4", exObj.LineType4);//,[LineType4]
                        cmd.Parameters.AddWithValue("@LineType5", exObj.LineType5);//,[LineType5]
                        cmd.Parameters.AddWithValue("@MarkerType1", exObj.MarkerType1);//,[MarkerType1]
                        cmd.Parameters.AddWithValue("@MarkerType2", exObj.MarkerType2);//,[MarkerType2]
                        cmd.Parameters.AddWithValue("@MarkerType3", exObj.MarkerType3);//,[MarkerType3]
                        cmd.Parameters.AddWithValue("@MarkerType4", exObj.MarkerType4);//,[MarkerType4]
                        cmd.Parameters.AddWithValue("@MarkerType5", exObj.MarkerType5);//,[MarkerType5]
                        cmd.Parameters.AddWithValue("@LegendOrientation", exObj.LegendOrientation);//,[LegendOrientation]
                        cmd.Parameters.AddWithValue("@LegendAllowToggleSeries", exObj.LegendAllowToggleSeries);//,[LegendAllowToggleSeries]
                        cmd.Parameters.AddWithValue("@LegendVisible", exObj.LegendVisible);//,[LegendVisible]
                        cmd.Parameters.AddWithValue("@DashboardNo", exObj.DashBoardNo);//,[DashboardNo]
                        cmd.Parameters.AddWithValue("@Width", exObj.Width);//,[Width]
                        cmd.Parameters.AddWithValue("@Height", exObj.Height);//,[Height]
                        cmd.Parameters.AddWithValue("@Y1color", exObj.Y1Color);//,[Y1color]
                        cmd.Parameters.AddWithValue("@Y2color", exObj.Y2Color);//,[Y2color]
                        cmd.Parameters.AddWithValue("@Y3color", exObj.Y3Color);//,[Y3color]
                        cmd.Parameters.AddWithValue("@Y4color", exObj.Y4Color);//,[Y4color]
                        cmd.Parameters.AddWithValue("@Y5color", exObj.Y5Color);//,[Y5color]
                        SqlParameter returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        _outPut = await cmd.ExecuteNonQueryAsync();
                        if (_outPut == -1)
                            retval = (int)cmd.Parameters["@return_value"].Value;

                        return retval;
                    }
                }
            }
            catch (Exception e)
            {
                string y = e.Message;
                return retval;
            }
        }

        public async Task<int> AddEditCustomer(string connectionString, string storeProcedureName, CompanyProfile exObj)
        {
            int _outPut = 0;
            int retval = -2;
            try
            {
                var returnObject = new List<dynamic>();
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", exObj.Id);
                        cmd.Parameters.AddWithValue("@Name", exObj.Name);
                        cmd.Parameters.AddWithValue("@UserId", exObj.UserId);
                        cmd.Parameters.AddWithValue("@Password", exObj.Password);
                        cmd.Parameters.AddWithValue("@Active", exObj.Active);

                        //cmd.Parameters.AddWithValue("@EmailId", exObj.EmailId);
                        //cmd.Parameters.AddWithValue("@Address", exObj.Address);
                        //cmd.Parameters.AddWithValue("@ContanctNumber", exObj.ContanctNumber);
                        //cmd.Parameters.AddWithValue("@ContactPerson", exObj.ContactPerson);
                        //cmd.Parameters.AddWithValue("@Plan", exObj.Plan);
                        //cmd.Parameters.AddWithValue("@Paid", exObj.Paid);
                        //cmd.Parameters.AddWithValue("@ImageName", exObj.ImageName);
                        //cmd.Parameters.AddWithValue("@ImageUrl", exObj.ImageUrl);
                        //cmd.Parameters.AddWithValue("@BgColorCode", exObj.BgColorCode);
                        //cmd.Parameters.AddWithValue("@ConectionString", exObj.ConnectionString);
                        SqlParameter returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        _outPut = await cmd.ExecuteNonQueryAsync();
                        if (_outPut == -1)
                            retval = (int)cmd.Parameters["@return_value"].Value;
                        return retval;
                    }
                }
            }
            catch (Exception e)
            {
                string y = e.Message;
                return retval;
            }
        }

        public async Task<IEnumerable<object>> GetDataByDateRange(string connectionString, RangeDateParam PramObj)
        {
            var retObject = new List<dynamic>();
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(PramObj.QueryOrSp, sql))
                    {
                        cmd.CommandType = PramObj.IsSp == 0 ? CommandType.Text : CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@startdate", SqlDbType.NVarChar) { Value = PramObj.StartDate });
                        cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.NVarChar) { Value = PramObj.EndDate });

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

        public async Task<IEnumerable<MenuItem>> GetMenuData(string connectionString, int Id)
        {
            List<MenuItem> MenuList = new List<MenuItem>();
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetMenu", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = Id });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                MenuItem _Menu = new MenuItem();
                                _Menu.Menu_id = Convert.ToInt32(dataReader["Menu_id"].ToString());
                                _Menu.Parentmenu_id = (dataReader["Parentmenu_id"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Parentmenu_id"].ToString()) : 0;
                                _Menu.UserId = Convert.ToInt32(dataReader["UserId"].ToString());
                                _Menu.ComponentId = (dataReader["ComponentId"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["ComponentId"].ToString()) : 0;
                                _Menu.Menu = (dataReader["Menu"].GetType() != typeof(DBNull)) ? dataReader["Menu"].ToString() : null;
                                _Menu.PageName = (dataReader["PageName"].GetType() != typeof(DBNull)) ? dataReader["PageName"].ToString() : null;
                                _Menu.Icon = (dataReader["Icon"].GetType() != typeof(DBNull)) ? dataReader["Icon"].ToString() : null;
                                 int i = (dataReader["Expand"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Expand"].ToString()) : 0;
                                _Menu.Expand = i == 0 ? false : true;
                                _Menu.DashBoardNo = (dataReader["DashBoardNo"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["DashBoardNo"].ToString()) : 0;
                                _Menu.MenuOrder = (dataReader["MenuOrder"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["MenuOrder"].ToString()) : 0;
                                _Menu.ConnectionId = (dataReader["Id"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Id"].ToString()) : 0;
                                _Menu.ConnectionName = (dataReader["ConnectionString"].GetType() != typeof(DBNull)) ? dataReader["ConnectionName"].ToString() : null;
                                _Menu.ConnectionString = (dataReader["ConnectionString"].GetType() != typeof(DBNull)) ? dataReader["ConnectionString"].ToString() : null;
                                _Menu.ProviderName = (dataReader["ProviderName"].GetType() != typeof(DBNull)) ? dataReader["ProviderName"].ToString() : null;
                                _Menu.ImageName = (dataReader["ImageName"].GetType() != typeof(DBNull)) ? dataReader["ImageName"].ToString() : null;
                                _Menu.Bgcolor = (dataReader["Bgcolor"].GetType() != typeof(DBNull)) ? dataReader["Bgcolor"].ToString() : "white";
                                _Menu.ReportTitle = (dataReader["ReportTitle"].GetType() != typeof(DBNull)) ? dataReader["ReportTitle"].ToString() : null;

                                MenuList.Add(_Menu);
                            }
                        }
                    }
                }
                return MenuList;
            }
            catch (Exception e)
            {
                string y = e.Message;
                return MenuList;
            }
        }

        public async Task<IEnumerable<DbNotification>> GetNotificationData(string connectionString, int UserId, int type)
        {
            List<DbNotification> NotificationList = new List<DbNotification>();
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetNotification", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int) { Value = type });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                 DbNotification _Notification = new DbNotification();
                                _Notification.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                _Notification.Status = (dataReader["Status"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Status"].ToString()) : 0;
                                _Notification.UserId = Convert.ToInt32(dataReader["UserId"].ToString());
                                _Notification.Msg = (dataReader["Msg"].GetType() != typeof(DBNull)) ? dataReader["Msg"].ToString() : null;
                                _Notification.Name = (dataReader["Name"].GetType() != typeof(DBNull)) ? dataReader["Name"].ToString() : null;
                                _Notification.date = (dataReader["date"].GetType() != typeof(DBNull)) ?Convert.ToDateTime((dataReader["date"])) : DateTime.Now;
                                 NotificationList.Add(_Notification);
                            }
                        }
                    }
                }
                return NotificationList;
            }
            catch (Exception e)
            {
                string y = e.Message;
                return NotificationList;
            }
        }

        public async Task<IEnumerable<DbConnection>> GetDataBaseConnection(string connectionString, int UserId)
        {
            List<DbConnection> DBCList = new List<DbConnection>();
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetDbConnectionById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                DbConnection DBC = new DbConnection();
                                DBC.Id = Convert.ToInt32(dataReader["Id"].ToString());
                                DBC.UserId = Convert.ToInt32(dataReader["CustomerId"].ToString());
                                DBC.ConnectionString =dataReader["ConnectionString"].ToString();
                                DBC.ProviderName = (dataReader["ProviderName"].GetType() != typeof(DBNull)) ? dataReader["ProviderName"].ToString() : null;
                                DBC.ConnectionName = (dataReader["ConnectionName"].GetType() != typeof(DBNull)) ? dataReader["ConnectionName"].ToString() : null;
                                DBCList.Add(DBC);
                            }
                        }
                    }
                }
                return DBCList;
            }
            catch (Exception e)
            {
                string y = e.Message;
                return DBCList;
            }
        }

        public async Task<int> Deleteuser(string connectionString, string storeProcedureName, int Id)
        {
            int _outPut = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", Id);
                        
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                       return _outPut = await cmd.ExecuteNonQueryAsync();                       
                    }
                }
            }
            catch (Exception e)
            {
                string y = e.Message;
                return _outPut;
            }
        }

        public async Task<int> AddEditUserDB(string connectionString, string storeProcedureName, DbConnection exObj)
        {
            int _outPut = 0;
            int retval = -2;
            try
            {
                var returnObject = new List<dynamic>();
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", exObj.Id);
                        cmd.Parameters.AddWithValue("@ConnectionString", exObj.ConnectionString);
                        cmd.Parameters.AddWithValue("@UserId", exObj.UserId);
                        cmd.Parameters.AddWithValue("@ConnectionName", exObj.ConnectionName);
                        cmd.Parameters.AddWithValue("@ProviderName", exObj.ProviderName);
                       
                        SqlParameter returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        _outPut = await cmd.ExecuteNonQueryAsync();
                        if (_outPut == -1)
                            retval = (int)cmd.Parameters["@return_value"].Value;
                        return retval;
                    }
                }
            }
            catch (Exception e)
            {
                string y = e.Message;
                return retval;
            }
        }

        public async Task<int> AddEditUserMenu(string connectionString, string storeProcedureName, MenuItem exObj)
        {
            int _outPut = 0;
            int retval = -2;
            try
            {
                var returnObject = new List<dynamic>();
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", exObj.Menu_id);
                        cmd.Parameters.AddWithValue("@Parentmenu_id", exObj.Parentmenu_id);
                        cmd.Parameters.AddWithValue("@Menu", exObj.Menu);
                        cmd.Parameters.AddWithValue("@PageName", exObj.PageName);
                        cmd.Parameters.AddWithValue("@UserId", exObj.UserId);
                        cmd.Parameters.AddWithValue("@ComponentId", exObj.ComponentId);
                        //cmd.Parameters.AddWithValue("@Icon", exObj.Icon);
                        cmd.Parameters.AddWithValue("@Expand", exObj.Expand);
                        cmd.Parameters.AddWithValue("@DashboardNo", exObj.DashBoardNo);
                        cmd.Parameters.AddWithValue("@MenuOrder", exObj.MenuOrder);
                        cmd.Parameters.AddWithValue("@DbConnection", exObj.ConnectionId);
                        //cmd.Parameters.AddWithValue("@ImageName", exObj.ImageName);
                        cmd.Parameters.AddWithValue("@Bgcolor", exObj.Bgcolor);

                        SqlParameter returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        _outPut = await cmd.ExecuteNonQueryAsync();
                        if (_outPut == -1)
                            retval = (int)cmd.Parameters["@return_value"].Value;
                        return retval;
                    }
                }
            }
            catch (Exception e)
            {
                string y = e.Message;
                return retval;
            }
        }

        public async Task<IEnumerable<PostRequestConfiguration>> GetAdminConfigurationDataByUserId(string connectionString, string storeProcedureName, int UserID)
        {
            string msg;
            List<PostRequestConfiguration> _ChartConfigurationList = new List<PostRequestConfiguration>();
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storeProcedureName, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompanyId", SqlDbType.Int) { Value = UserID });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                PostRequestConfiguration _ChartConfiguration = new PostRequestConfiguration();
                                 _ChartConfiguration.CompanyId = UserID;
                                _ChartConfiguration.Id = (dataReader["Id"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Id"].ToString()) : 0;
                                _ChartConfiguration.QueryOrSp = (dataReader["QueryOrSp"].GetType() != typeof(DBNull)) ? dataReader["QueryOrSp"].ToString().Trim() : null;
                                _ChartConfiguration.IsSp = (dataReader["IsSp"].GetType() != typeof(DBNull)) ? false : Convert.ToInt32(dataReader["IsSp"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.ChartType = (dataReader["ChartType"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["ChartType"].ToString().Trim()) : 0;
                                _ChartConfiguration.ChartOrder = (dataReader["ChartOrder"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["ChartOrder"].ToString().Trim()) : 0;
                                _ChartConfiguration.BlockPosition = (dataReader["Position"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["Position"].ToString().Trim()) : 0;
                                _ChartConfiguration.Active = (dataReader["Active"].GetType() != typeof(DBNull)) ? false : Convert.ToInt32(dataReader["Active"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.Title = (dataReader["Title"].GetType() != typeof(DBNull)) ? dataReader["Title"].ToString().Trim() : null;
                                _ChartConfiguration.CardBgColor = (dataReader["CardBgColor"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["CardBgColor"].ToString().Trim()) : 0;
                                _ChartConfiguration.IsPopUpOpen = (dataReader["IsPopUpOpen"].GetType() != typeof(DBNull)) ? false : Convert.ToInt32(dataReader["IsPopUpOpen"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.IsFilterOn = (dataReader["IsFilterOn"].GetType() != typeof(DBNull)) ? false : Convert.ToInt32(dataReader["IsFilterOn"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.XAxixValueColumnName = (dataReader["XAxixValueColumnName"].GetType() != typeof(DBNull)) ? dataReader["XAxixValueColumnName"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName1 = (dataReader["YAxixValueColumnName1"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName1"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName2 = (dataReader["YAxixValueColumnName2"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName2"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName3 = (dataReader["YAxixValueColumnName3"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName3"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName4 = (dataReader["YAxixValueColumnName4"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName4"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixValueColumnName5 = (dataReader["YAxixValueColumnName5"].GetType() != typeof(DBNull)) ? dataReader["YAxixValueColumnName5"].ToString().Trim() : null;
                                _ChartConfiguration.Label1 = (dataReader["Label1"].GetType() != typeof(DBNull)) ? dataReader["Label1"].ToString().Trim() : null;
                                _ChartConfiguration.Label2 = (dataReader["Label2"].GetType() != typeof(DBNull)) ? dataReader["Label2"].ToString().Trim() : null;
                                _ChartConfiguration.Label3 = (dataReader["Label3"].GetType() != typeof(DBNull)) ? dataReader["Label3"].ToString().Trim() : null;
                                _ChartConfiguration.Label4 = (dataReader["Label4"].GetType() != typeof(DBNull)) ? dataReader["Label4"].ToString().Trim() : null;
                                _ChartConfiguration.Label5 = (dataReader["Label5"].GetType() != typeof(DBNull)) ? dataReader["Label5"].ToString().Trim() : null;
                                _ChartConfiguration.XAxixDisplayName = (dataReader["XAxixDisplayName"].GetType() != typeof(DBNull)) ? dataReader["XAxixDisplayName"].ToString().Trim() : null;
                                _ChartConfiguration.YAxixDisplayName = (dataReader["YAxixDisplayName"].GetType() != typeof(DBNull)) ? dataReader["YAxixDisplayName"].ToString().Trim() : null;
                                _ChartConfiguration.FormateString = (dataReader["FormateString"].GetType() != typeof(DBNull)) ? dataReader["FormateString"].ToString().Trim() : "{value}";
                                _ChartConfiguration.LineType1 = (dataReader["LineType1"].GetType() != typeof(DBNull)) ? true : Convert.ToInt32(dataReader["LineType1"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.LineType2 = (dataReader["LineType2"].GetType() != typeof(DBNull)) ? true : Convert.ToInt32(dataReader["LineType2"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.LineType3 = (dataReader["LineType3"].GetType() != typeof(DBNull)) ? true : Convert.ToInt32(dataReader["LineType3"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.LineType4 = (dataReader["LineType4"].GetType() != typeof(DBNull)) ? true : Convert.ToInt32(dataReader["LineType4"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.LineType5 = (dataReader["LineType5"].GetType() != typeof(DBNull)) ? true : Convert.ToInt32(dataReader["LineType5"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.MarkerType1 = (dataReader["MarkerType1"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["MarkerType1"].ToString().Trim()) : 1;
                                _ChartConfiguration.MarkerType2 = (dataReader["MarkerType2"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["MarkerType2"].ToString().Trim()) : 1;
                                _ChartConfiguration.MarkerType3 = (dataReader["MarkerType3"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["MarkerType3"].ToString().Trim()) : 1;
                                _ChartConfiguration.MarkerType4 = (dataReader["MarkerType4"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["MarkerType4"].ToString().Trim()) : 1;
                                _ChartConfiguration.MarkerType5 = (dataReader["MarkerType5"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["MarkerType5"].ToString().Trim()) : 1;
                                _ChartConfiguration.Xml = (dataReader["Xml"].GetType() != typeof(DBNull)) ? dataReader["Xml"].ToString().Trim() : null;
                                _ChartConfiguration.LegendAllowToggleSeries = (dataReader["LegendAllowToggleSeries"].GetType() != typeof(DBNull)) ? true : Convert.ToInt32(dataReader["LegendAllowToggleSeries"].ToString()) == 1 ? true : false; 
                                _ChartConfiguration.LegendVisible = (dataReader["LegendVisible"].GetType() != typeof(DBNull)) ? true : Convert.ToInt32(dataReader["LegendVisible"].ToString()) == 1 ? true : false;
                                _ChartConfiguration.LegendPosition = (dataReader["LegendPosition"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["LegendPosition"].ToString().Trim()) : 3;
                                _ChartConfiguration.LegendOrientation = (dataReader["LegendOrientation"].GetType() != typeof(DBNull)) ? true : Convert.ToInt32(dataReader["LegendOrientation"].ToString()) == 1 ? true : false;

                                _ChartConfiguration.DashBoardNo = (dataReader["DashBoardNo"].GetType() != typeof(DBNull)) ? Convert.ToInt32(dataReader["DashBoardNo"].ToString()) : 0;
                                _ChartConfiguration.Width = (dataReader["Width"].GetType() != typeof(DBNull)) ? dataReader["Width"].ToString().Trim() : "100%";
                                _ChartConfiguration.Height = (dataReader["Height"].GetType() != typeof(DBNull)) ? dataReader["Height"].ToString().Trim() : "100%";
                                _ChartConfiguration.Y1Color = (dataReader["Y1Color"].GetType() != typeof(DBNull)) ? dataReader["Y1Color"].ToString().Trim() : "Green";
                                _ChartConfiguration.Y2Color = (dataReader["Y2Color"].GetType() != typeof(DBNull)) ? dataReader["Y2Color"].ToString().Trim() : "Blue";
                                _ChartConfiguration.Y3Color = (dataReader["Y3Color"].GetType() != typeof(DBNull)) ? dataReader["Y3Color"].ToString().Trim() : "Red";
                                _ChartConfiguration.Y4Color = (dataReader["Y4Color"].GetType() != typeof(DBNull)) ? dataReader["Y4Color"].ToString().Trim() : "Blue";
                                _ChartConfiguration.Y5Color = (dataReader["Y5Color"].GetType() != typeof(DBNull)) ? dataReader["Y5Color"].ToString().Trim() : "Blue";

                                _ChartConfigurationList.Add(_ChartConfiguration);

                            }

                        }
                    }
                }
                return _ChartConfigurationList;
            }
            catch (Exception ex)
            {
               
                msg = ex.Message;
                return _ChartConfigurationList;
            }
        }

    }
}
