using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace DashBoard
{
    public static class ExtensionMethods
    {

        /// <summary>
        /// Create a datatable from a list of <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="data">The list can be created from a dictionary with Dictionary.Values.ToList()</param>
        /// <param name="tableName">Name of the data table</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IEnumerable<dynamic> data, string tableName)
        {
            return data.ToList().ToDataTable(tableName);
        }
        /// <summary>
        /// Create a datatable from a list of ExpandoObjects
        /// </summary>
        /// <param name="list">The list can be created from a dictionary with Dictionary.Values.ToList()</param>
        /// <param name="tableName">Name of the data table</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this List<ExpandoObject> list, string tableName)
        {

            if (list == null || list.Count == 0)
            {
                return null;
            }

            //build columns
            var props = (IDictionary<string, object>)list[0];
            var t = new DataTable(tableName);
            foreach (var prop in props)
            {
                t.Columns.Add(new DataColumn(prop.Key, prop.Value.GetType()));
            }
            //add rows
            foreach (var row in list)
            {
                var data = t.NewRow();
                foreach (var prop in (IDictionary<string, object>)row)
                {
                    data[prop.Key] = prop.Value;
                }
                t.Rows.Add(data);
            }
            return t;
        }

        public static DataTable ToDt(this IEnumerable<ExpandoObject> items)
        {
            var data = items.ToArray();
            if (data.Count() == 0) return null;
            var dt =new  DataTable();
            foreach(var d in data)
            {
                foreach (var key in ((IDictionary<string, object>)d).Keys)
                {
                    if(!dt.Columns.Contains(key))
                    {
                        dt.Columns.Add(key);
                    }
                }
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }
    }
}
