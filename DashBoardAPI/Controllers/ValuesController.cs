using DashBoardAPI.BAL;
using DashBoardModel;
using Microsoft.AspNetCore.Mvc;
using Npgsql.PostgresTypes;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DashBoardAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        BAL.ExecuteSp exSp = new BAL.ExecuteSp();
        // GET: api/<ValuesController>
        [HttpGet("{connectionString},{StoreProcedureName},{CommondName},{UserId},{ProviderName}")]
        public async Task<IEnumerable<object>> Get(string connectionString, string StoreProcedureName, string CommondName, int UserId,string ProviderName)
        {
            IEnumerable<object> output=null;
            switch (ProviderName.ToLower())
            {
                case "sqlserver":
                    output= await exSp.ExecuteSpGetData(connectionString, StoreProcedureName, CommondName, UserId);
                    break;
                case "postgresql":
                    output= await PostgreSQLClass.ExecuteSpGetData(connectionString, StoreProcedureName, CommondName, UserId);
                    break;
                    //case "mysql":
                    // reponse = await MySqlClass.GetChartsData(connectionString, value);
                    //break;
                    case "oracle":
                    break;                    
            }

            return output;
        }

        [HttpPost("{connectionString},{ProviderName}")]
        public async Task<IEnumerable<object>> GetReports(string connectionString,string ProviderName,  Param prm)
        {
            IEnumerable<object> output = null;
            switch (ProviderName.ToLower())
            {
                case "sqlserver":
                    output = await exSp.GetReport(connectionString, prm);
                    break;
                case "postgresql":
                    output = await PostgreSQLClass.GetReport(connectionString, prm);
                    break;                
                //case "mysql":
                // reponse = await MySqlClass.GetChartsData(connectionString, value);
                //break;
                case "oracle":
                    break;
            }

            return output;
        }

        [HttpPost("{connectionString},{ProviderName}")]
        public IEnumerable<object> GetExcelReports(string connectionString, string ProviderName, Param prm)
        {
            IEnumerable<object> output = null;            
            output =  ExcelData.GetReport(connectionString, prm);
            return output;
        }
        [HttpGet("{connectionString},{StoreProcedureName},{UserId}")]
        public async Task<IEnumerable<GetRequestConfiguration>> GetChartsConfiguration(string connectionString, string StoreProcedureName, int UserId)
        {
            return await exSp.GeChartConfigurationtData(connectionString, StoreProcedureName, UserId);
        }

        [HttpGet("{connectionString},{Id}")]
        public async Task<IEnumerable<object>> GetMenu(string connectionString, int Id)
        {
            return await exSp.GetMenuData(connectionString, Id);
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("{connectionString},{StoreProcedureName},{UserId},{Password}")]
        public CompanyProfile GetUserInfo(string connectionString, string StoreProcedureName, string UserId, string Password)
        {
            CompanyProfile _userInfo = new CompanyProfile();
            _userInfo = exSp.GetCompanyData(connectionString, StoreProcedureName, UserId, Password);
            return _userInfo;
        }

        // POST api/<ValuesController>
        [HttpPost("{CommondName},{connectionString},{StoreProcedureName}")]
        public async Task<OutputData> Post(string CommondName, string connectionString, string StoreProcedureName, [FromBody] ExpandoObject value)
        {
            return await exSp.ExecuteSpMasterData(connectionString, StoreProcedureName, CommondName, value);
        }

        [HttpPost("{StoreProcedureName},{CommondName},{connectionString}")]
        public async Task<OutputData> GetDatabyParam(string StoreProcedureName, string CommondName, string connectionString, [FromBody] List<Param> value )
        {
            var reponse = await exSp.GetDataSpMasterByParam(connectionString, StoreProcedureName, CommondName, value);
            return reponse;
        }

        [HttpPost("{connectionString},{ProviderName}")]
        public async Task<OutputData> FilterDataByParam(string connectionString, string ProviderName, [FromBody] Param value)
        {
            OutputData outData= new OutputData();
            //var reponse = await exSp.GetFilterDataByParam(connectionString, value);
            switch (ProviderName.ToLower())
            {
                case "sqlserver":
                    outData = await exSp.GetFilterDataByParam(connectionString, value);
                    break;
                case "postgresql":
                    outData = await PostgreSQLClass.GetFilterDataByParam(connectionString, value);
                    break;
                //case "mysql":
                   // reponse = await MySqlClass.GetChartsData(connectionString, value);
                    //break;
                case "oracle":
                    break;
            }
            
            return outData;
        }

        [HttpPost("{connectionString},{ProviderName}")]
        public async Task<List<StoreProcedures>> GetAllChartsData(string connectionString,string ProviderName, [FromBody] List<StoreProcedures> value)
        {
            List<StoreProcedures> reponse = new List<StoreProcedures>();
            switch (ProviderName.ToLower())
            {
                case "sqlserver":
                    reponse = await exSp.GetChartsData(connectionString, value);
                    break;
                case "postgresql":
                    reponse = await PostgreSQLClass.GetChartsData(connectionString, value);
                    break;
                case "mysql":
                    reponse = await MySqlClass.GetChartsData(connectionString, value);
                    break;
                case "oracle":
                    break;
            }
             
            //var reponse = await PostgreSQLClass.GetChartsData(connectionString, value);
            //var reponse = await MySqlClass.GetChartsData(connectionString, value);

            return reponse;
        }

        [HttpGet("{connectionString},{SpName}")]
        public async Task<List<CompanyProfile>> GetAllAdminData(string connectionString, string SpName)
        {
            var reponse = await exSp.GetAdminData(connectionString, SpName);
            return reponse;
        }

        [HttpGet("{connectionString}")]
        public async Task<ChartsFeaturesList> GetChartFeature(string connectionString)
        {
            ChartsFeaturesList reponse = await exSp.GetChartsFeatureData(connectionString);
            return reponse;
        }

        [HttpPost("{connectionString}")]
        public async Task<int> InsertUpdateChartConfiguration(string connectionString, [FromBody] PostRequestConfiguration value)
        {
            var reponse = await exSp.UpdateInsertChartConfigurationtData(connectionString, "InsertUpdateChartConfiguration", value);
            return reponse;
        }

        [HttpPost("{connectionString}")]
        public async Task<int> AddEditCustomer(string connectionString, [FromBody] CompanyProfile value)
        {
            var reponse = await exSp.AddEditCustomer(connectionString, "InsertUpdateUser", value);
            return reponse;
        }

        [HttpPost("{connectionString},{ProviderName}")]
        public async Task<IEnumerable<object>> GetDateRageData(string connectionString,string ProviderName, [FromBody] RangeDateParam Value)
        {
            IEnumerable<object> output = null;
            switch (ProviderName.ToLower())
            {
                case "sqlserver":
                    output = await exSp.GetDataByDateRange(connectionString, Value);
                    break;
                case "postgresql":
                    output = await PostgreSQLClass.GetDataByDateRange(connectionString,Value);
                    break;
                //case "mysql":
                // reponse = await MySqlClass.GetChartsData(connectionString, value);
                //break;
                case "oracle":
                    break;
            }
            return output;
        }

        [HttpGet("{connectionString},{Id},{type}")]
        public async Task<IEnumerable<object>> GetNotification(string connectionString, int Id ,int type)
        {
            return await exSp.GetNotificationData(connectionString, Id, type);
        }

        [HttpGet("{connectionString},{Id}")]
        public async Task<IEnumerable<object>> GetDbConnection(string connectionString, int Id)
        {
            return await exSp.GetDataBaseConnection(connectionString, Id);
        }

        [HttpGet("{connectionString},{Id},{Sp}")]
        public async Task<int> DeleteUser(string connectionString, int Id,string Sp)
        {
            return await exSp.Deleteuser(connectionString,Sp,Id);
        }

        [HttpPost("{connectionString}")]
        public async Task<int> AddEditUserDB(string connectionString, [FromBody] DbConnection value)
        {
            var reponse = await exSp.AddEditUserDB(connectionString, "InsertUpdateDbConnection", value);
            return reponse;
        }

        [HttpPost("{connectionString}")]
        public async Task<int> AddEditUserMenu(string connectionString, [FromBody] MenuItem value)
        {
            var reponse = await exSp.AddEditUserMenu(connectionString, "InsertUpdateUserMenu", value);
            return reponse;
        }

        [HttpGet("{connectionString},{StoreProcedureName},{UserId}")]
        public async Task<IEnumerable<PostRequestConfiguration>> GetAdminConfigurationDataByUserId(string connectionString, string StoreProcedureName, int UserId)
        {
            return await exSp.GetAdminConfigurationDataByUserId(connectionString, StoreProcedureName, UserId);
        }
    }
}
