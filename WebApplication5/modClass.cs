using DuckDB.NET.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public class modClass
    {
        public static String conString = "";
        public static String conStringMysql = "";
        public static String conStringDuckdb = "";
        public static String conStringParquet = "";

        public static IConfiguration configuration;

        public static IEnumerable<Dictionary<string, object>> Serialize(DuckDBDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));
            while (reader.Read())
                results.Add(SerializeRow(cols, reader));
            return results;
        }
        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                DuckDBDataReader reader)
        {
            {
                var result = new Dictionary<string, object>();
                foreach (var col in cols)
                    result.Add(col, reader[col]);
                return result;
            }

        }
    }
}
