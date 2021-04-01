//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using DashBoardModel;
//using Microsoft.Extensions.Configuration;
//using TableDependency.SqlClient;
//using TableDependency.SqlClient.Base.EventArgs;

//namespace SynGeniee
//{
//    public class DatabaseNotification : IDatabaseNotification, IDisposable
//    {
//        public event DBNotificationDelegate DBNotificationChanged;

//        private const string TableName = "Notification";
//        private SqlTableDependency<DbNotification> _notifier;
//        private IConfiguration _configuration;

//        public DatabaseNotification(IConfiguration configuration)
//        {
//            _configuration = configuration;
//            _notifier = new SqlTableDependency<DbNotification>(_configuration.GetConnectionString("DashBoardConnection"), TableName);
//            _notifier.OnChanged += this.TableDependency_Changed;
//            _notifier.Start();
//        }

//        private void TableDependency_Changed(object sender, RecordChangedEventArgs<DbNotification> e)
//        {
//            if (this.DBNotificationChanged != null)
//            {
//                this.DBNotificationChanged(this, new DBNotificationChangeEventArgs(e.Entity));
//            }
//        }

//        public void Dispose()
//        {
//            _notifier.Stop();
//            _notifier.Dispose();
//        }
//    }
//}
