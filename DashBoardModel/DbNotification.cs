using System;
using System.Collections.Generic;
using System.Text;

namespace DashBoardModel
{
    public class DbNotification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Msg { get; set; }
        public int Status { get; set; }
        public DateTime date { get; set; }
        public string Name { get; set; }
    }
}
