using System;
using System.Collections.Generic;
using System.Text;

namespace DashBoardModel
{
    public class MenuItem
    {
        public int Menu_id { get; set; }
        public int Parentmenu_id { get; set; }
        public string Menu { get; set; }
        public string PageName { get; set; }
        public int UserId { get; set; }
        public int ComponentId { get; set; }
        public string ReportTitle { get; set; }
        public string Icon { get; set; }
        public bool Expand { get; set; }
        public int DashBoardNo { get; set; }
        public int MenuOrder { get; set; }
        public string ImageName { get; set; }
        public string Bgcolor { get; set; }
        public int ConnectionId { get; set; }
        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
    }
}
