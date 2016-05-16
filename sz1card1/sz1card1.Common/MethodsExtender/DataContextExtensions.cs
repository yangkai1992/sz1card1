using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace sz1card1.Common
{
    public static class DataContextExtensions
    {
        public static List<T> ExecuteQuery<T>(
            this DataContext dataContext, IQueryable query, bool withNoLock)
        {
            DbCommand command = dataContext.GetCommand(query, withNoLock);

            dataContext.Connection.Open();

            using (DbDataReader reader = command.ExecuteReader())
            {
                return dataContext.Translate<T>(reader).ToList();
            }
        }

        private static Regex s_withNoLockRegex =
            new Regex(@"(] AS \[t\d+\])", RegexOptions.Compiled);

        private static string AddWithNoLock(string cmdText)
        {
            IEnumerable<Match> matches =
                s_withNoLockRegex.Matches(cmdText).Cast<Match>()
                .OrderByDescending(m => m.Index);
            foreach (Match m in matches)
            {
                int splitIndex = m.Index + m.Value.Length;
                cmdText =
                    cmdText.Substring(0, splitIndex) + " WITH (NOLOCK)" +
                    cmdText.Substring(splitIndex);
            }

            return cmdText;
        }

        private static SqlCommand GetCommand(
            this DataContext dataContext, IQueryable query, bool withNoLock)
        {
            SqlCommand command = (SqlCommand)dataContext.GetCommand(query);

            if (withNoLock)
            {
                command.CommandText = AddWithNoLock(command.CommandText);
            }

            return command;
        }

        #region 对同一个表大量数据插入

        /// <summary>
        /// 对同一个表大量数据插入SqlBulkCopy
        /// </summary>
        /// <typeparam name="T">System.Data.Linq.Mapping.Table</typeparam>
        /// <param name="entities"></param>
        public static void BulkInsertAll<T>(this DataContext dataContext, IEnumerable<T> entities)
        {
            entities = entities.ToArray();

            Type t = typeof(T);

            var tableAttribute = (TableAttribute)t.GetCustomAttributes(
                typeof(TableAttribute), false).FirstOrDefault();

            if (tableAttribute == null) throw new Exception("对象未添加System.Data.Linq.Mapping.Table特性");

            var bulkCopy = new SqlBulkCopy(dataContext.Connection.ConnectionString, SqlBulkCopyOptions.KeepIdentity)
            {
                DestinationTableName = tableAttribute.Name,
            };

            var properties = t.GetProperties().Where(p =>
            {
                var attribute = GetColumnAttribute(p);
                return attribute != null && !string.IsNullOrWhiteSpace(attribute.Name) && !attribute.IsDbGenerated;
            }).ToArray();

            var table = new DataTable();

            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;
                if (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                table.Columns.Add(new DataColumn(property.Name, propertyType));
                bulkCopy.ColumnMappings.Add(property.Name, GetColumnName(property));
            }

            foreach (var entity in entities)
            {
                table.Rows.Add(properties.Select(
                  property => GetPropertyValue(
                  property.GetValue(entity, null))).ToArray());
            }

            bulkCopy.WriteToServer(table);

        }

        private static bool EventTypeFilter(System.Reflection.PropertyInfo p)
        {
            var attribute = Attribute.GetCustomAttribute(p,
                typeof(AssociationAttribute)) as AssociationAttribute;

            if (attribute == null) return true;
            if (attribute.IsForeignKey == false) return true;

            return false;
        }

        private static string GetColumnName(System.Reflection.PropertyInfo p)
        {
            var attribute = Attribute.GetCustomAttribute(p,
                typeof(ColumnAttribute)) as ColumnAttribute;
            if (attribute == null) return null;
            return attribute.Name;
        }

        private static ColumnAttribute GetColumnAttribute(System.Reflection.PropertyInfo p)
        {
            return Attribute.GetCustomAttribute(p,typeof(ColumnAttribute)) as ColumnAttribute;
        }

        private static object GetPropertyValue(object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }

        #endregion
    }
}
