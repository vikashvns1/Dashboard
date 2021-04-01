using System.Collections.Generic;
using System.Dynamic;

namespace DashBoardModel
{
    public class StoreProcedures
    {
        public int Id { get; set; }
        public string SpOrQuery { get; set; }
        public int IsSp { get; set; }
        public List<ExpandoObject> ChatData { get; set; }
    }
}
