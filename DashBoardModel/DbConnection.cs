using System;
using System.Collections.Generic;
using System.Text;

namespace DashBoardModel
{
    public class DbConnection
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string ConnectionName { get; set; }
    }
}
