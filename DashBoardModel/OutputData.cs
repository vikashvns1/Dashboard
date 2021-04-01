using System.Collections.Generic;
using System.Dynamic;

namespace DashBoardModel
{
    public class OutputData
    {
        public List<ExpandoObject> DynamicData { get; set; }
        public string Msg { get; set; }
        public int ReturnsValue { get; set; }
    }
}
