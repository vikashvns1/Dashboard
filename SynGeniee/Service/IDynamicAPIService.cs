using DashBoardModel;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace SynGeniee
{
    public interface IDynamicAPIService
    {
        Task<string> GetAppConfiguration(string connection, string spName, string action, int userId, string ProviderName);
        Task<string> GetReportsData(string connectionString,string ProviderName, Param Obj);
        Task<string> GetMenuData(string connectionString, int Id);
        Task<string> InsertData(ExpandoObject obj, string connection, string spName, string action);
        Task<string> GetDatabyParam(List<Param> obj, string spName, string action, string connection);
        Task<CompanyProfile> GetComponyData(string connection, string spName, string UserId, string Password);
        Task<IEnumerable<GetRequestConfiguration>> GetChartsConfiguration(string connection, string spName, int userId);
        Task<List<StoreProcedures>> GetAllChartsData(List<StoreProcedures> obj, string connection, string ProviderName);
        Task<List<CompanyProfile>> GetAllAdminData(string connection, string SpName);
        Task<ChartsFeaturesList> GetChartsFeaturesData(string connection);
        Task<int> InsertUpdateChartConfigData(string connection, PostRequestConfiguration obj);
        Task<int> InsertEditCustomer(string connection, CompanyProfile obj);
        Task<string> GetDatabyParam(string connection,string ProviderName, Param Obj);
        Task<string> GetDateByDateRange(string connection, string ProviderName,RangeDateParam obj);
        Task<string> GetNotificationData(string connectionString, int Id, int type);
        Task<string> GetDBConnections(string connectionString, int Id);
        Task<int> DeleteUser(string connection, int obj,string Sp);
        Task<int> AddEditUserDB(string connection, DbConnection obj);
        Task<int> AddEditUserMenu(string connection, MenuItem obj); //GetAdminConfigurationDataByUserId

        Task<IEnumerable<PostRequestConfiguration>> GetAdminConfigurationDataByUserId(string connection, string spName, int userId);

    }
}
