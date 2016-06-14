using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SQLHelper
{
    public class SQLHelper
    {
        public static string CreatPageSelectSQL(int pageIndex, int pageSize, string table, string where, string orderBy)
        {
            string sql = string.Format(@"select top {0} * from 
                        (select *,ROW_NUMBER() OVER (ORDER BY {1}) AS ROWNUM FROM ({2}) as b {3}) as a
                        where a.ROWNUM>{4}
                        order by ROWNUM", pageSize.ToString(), orderBy, table, where, (pageIndex * pageSize).ToString());
            return sql;
        }

        public static string GetTotalCountSQL(string table, string where)
        {
            string sql = string.Format(@"select count(*) from ({0} {1}) as a", table, where);
            return sql;
        }
    }
}
