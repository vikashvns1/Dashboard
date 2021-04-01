//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using DashBoardModel;

//namespace SynGeniee
//{
//    public delegate void DBNotificationDelegate(object sender, DBNotificationChangeEventArgs args);

//    public class DBNotificationChangeEventArgs : EventArgs
//    {
//        public DbNotification DBNotification { get; }

//        public DBNotificationChangeEventArgs(DbNotification _dbNotification)
//        {
//            this.DBNotification = _dbNotification;
//        }
//    }

//    interface IDatabaseNotification
//    {
//        public event DBNotificationDelegate DBNotificationChanged;
//    }
//}
