namespace DataAccessHandler
{
    public class DatabaseHandlerFactory
    {
        private string connectionStringSettings;

        public DatabaseHandlerFactory(string connectionStringName)
        {
            connectionStringSettings = connectionStringName;
        }

        public IDatabaseHandler CreateDatabase(string providerName)
        {
            IDatabaseHandler database = null;

            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    database = new SqlDataAccess(connectionStringSettings);
                    break;
                case "system.data.oracleclient":
                    database = new OracleDataAccess(connectionStringSettings);
                    break;
                case "system.data.oleDb":
                    database = new OledbDataAccess(connectionStringSettings);
                    break;
                case "system.data.odbc":
                    database = new OdbcDataAccess(connectionStringSettings);
                    break;
            }

            return database;
        }

        //public string GetProviderName()
        //{
        //    return connectionStringSettings.ProviderName;
        //}
    }
}
