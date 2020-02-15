using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace NLESG.Common.Utils
{
    public static class Utils
    {
        #region DataTable Functions
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Props = Props.OrderBy(p => p.Name).ToArray();
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                try
                {


                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #endregion DataTable Functions     
    }
}
