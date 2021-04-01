using DashBoardModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SynGeniee
{
    public class DynamicAPIService : IDynamicAPIService
    {
        private readonly HttpClient httpClient;

        public DynamicAPIService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetAppConfiguration(string connection, string spName, string CommondName, int userId, string ProviderName)
        {
            string response = await httpClient.GetStringAsync("api/Values/Get/" + connection + "," + spName + "," + CommondName + "," + userId + "," + ProviderName);
            return response;
        }

        public async Task<string> InsertData(ExpandoObject exObj, string connection, string spName, string CommondName)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(exObj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/Post/" + CommondName + "," + connection + "," + spName, content).Result.Content.ReadAsStringAsync();
            return response;
        }

        public async Task<string> GetDatabyParam(List<Param> exObj, string spName, string CommondName, string connection)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(exObj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/GetDatabyParam/" + spName + "," + CommondName + ", " + connection, content).Result.Content.ReadAsStringAsync();
            return response;
        }

        public async Task<string> GetDatabyParam(string connection, string ProviderName, Param exObj)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(exObj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/FilterDataByParam/" + connection + "," + ProviderName, content).Result.Content.ReadAsStringAsync();
            return response;
        }

        public async Task<CompanyProfile> GetComponyData(string connection, string spName, string UserId, string Password)
        {
            string response = await httpClient.GetStringAsync("api/Values/GetUserInfo/" + connection + "," + spName + "," + UserId + "," + Password);
            CompanyProfile _userInfo = JsonConvert.DeserializeObject<CompanyProfile>(response);
            return _userInfo;
        }

        public async Task<string> GetDateByDateRange(string connection, string ProviderName, RangeDateParam obj)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/GetDateRageData/" + connection + "," + ProviderName, content).Result.Content.ReadAsStringAsync();
            return response;

        }
        public async Task<IEnumerable<GetRequestConfiguration>> GetChartsConfiguration(string connection, string spName, int userId)
        {
            string response = await httpClient.GetStringAsync("api/Values/GetChartsConfiguration/" + connection + "," + spName + "," + userId);
            IEnumerable<GetRequestConfiguration> _ChartConfiguration = JsonConvert.DeserializeObject<IEnumerable<GetRequestConfiguration>>(response);
            return _ChartConfiguration;
        }

        public async Task<List<StoreProcedures>> GetAllChartsData(List<StoreProcedures> obj, string connection, string ProviderName)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/GetAllChartsData/" + connection + "," + ProviderName, content).Result.Content.ReadAsStringAsync();
            List<StoreProcedures> _ChartsData = JsonConvert.DeserializeObject<List<StoreProcedures>>(response);
            return _ChartsData;
        }

        public async Task<List<CompanyProfile>> GetAllAdminData(string connection, string SpName)
        {
            string response = await httpClient.GetStringAsync("api/Values/GetAllAdminData/" + connection + "," + SpName);
            List<CompanyProfile> _userInfo = JsonConvert.DeserializeObject<List<CompanyProfile>>(response);
            return _userInfo;
        }

        public async Task<ChartsFeaturesList> GetChartsFeaturesData(string connection)
        {
            string response = await httpClient.GetStringAsync("api/Values/GetChartFeature/" + connection);
            ChartsFeaturesList _userInfo = JsonConvert.DeserializeObject<ChartsFeaturesList>(response);
            return _userInfo;
        }

        public async Task<int> InsertUpdateChartConfigData(string connection, PostRequestConfiguration obj)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/InsertUpdateChartConfiguration/" + connection, content).Result.Content.ReadAsStringAsync();
            int i = JsonConvert.DeserializeObject<int>(response);
            return i;
        }

        public async Task<int> InsertEditCustomer(string connection, CompanyProfile obj)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/AddEditCustomer/" + connection, content).Result.Content.ReadAsStringAsync();
            int i = JsonConvert.DeserializeObject<int>(response);
            return i;
        }

        public async Task<string> GetReportsData(string connectionString, string ProviderName, Param Obj)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(Obj), Encoding.UTF8, "application/json");
            string response = await httpClient.PostAsync("api/Values/GetReports/" + connectionString + "," + ProviderName, content).Result.Content.ReadAsStringAsync();
            return response;
        }

        public async Task<string> GetMenuData(string connectionString, int Id)
        {
            string response = await httpClient.GetStringAsync("api/Values/GetMenu/" + connectionString + "," + Id);
            return response;
        }

        public async Task<string> GetNotificationData(string connectionString, int Id, int type)
        {
            string response = await httpClient.GetStringAsync("api/Values/GetNotification/" + connectionString + "," + Id + "," + type);
            return response;
        }

        public async Task<string> GetDBConnections(string connectionString, int Id)
        {
            string response = await httpClient.GetStringAsync("api/Values/GetDbConnection/" + connectionString + "," + Id);
            return response;
        }

        public async Task<int> DeleteUser(string connection, int Id, string Sp)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync("api/Values/DeleteUser/" + connection + "," + Id + "," + Sp).Result.Content.ReadAsStringAsync();
            int i = JsonConvert.DeserializeObject<int>(response);
            return i;
        }

        public async Task<int> AddEditUserDB(string connection, DbConnection obj)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/AddEditUserDB/" + connection, content).Result.Content.ReadAsStringAsync();
            int i = JsonConvert.DeserializeObject<int>(response);
            return i;
        }

        public async Task<int> AddEditUserMenu(string connection, MenuItem obj)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Values/AddEditUserMenu/" + connection, content).Result.Content.ReadAsStringAsync();
            int i = JsonConvert.DeserializeObject<int>(response);
            return i;
        }

        public async Task<IEnumerable<PostRequestConfiguration>> GetAdminConfigurationDataByUserId(string connection, string spName, int userId)
        {
            string response = await httpClient.GetStringAsync("api/Values/GetAdminConfigurationDataByUserId/" + connection + "," + spName + "," + userId);
            IEnumerable<PostRequestConfiguration> _ChartConfiguration = JsonConvert.DeserializeObject<IEnumerable<PostRequestConfiguration>>(response);
            return _ChartConfiguration;
        }
    }
}
