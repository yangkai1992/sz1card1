using Common.SQLHelper;
using ManagementDAL.IDAL;
using ManagementDataModel.Models.User;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ManagementDAL.DAL.NHibernate
{
    public class UserDao : IUser
    {
        private ISessionFactory sessionFactory;

        public UserDao()
        {
            var cfg = new Configuration().Configure("Config/MSSQL.cfg.xml");
            sessionFactory = cfg.BuildSessionFactory();
        }

        public User GetUser(Guid guid)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.QueryOver<User>().Where(x => x.Guid == new Guid("D637501B-8D79-490D-8E23-6919BAED0587")).SingleOrDefault();
            }
        }

        public User GetUser(string account)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Get<User>(account);
            }
        }

        public User Get(string account)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Get<User>(account);
            }
        }


        public DataSet GetUserList(int pageIndex, int pageSize, string orderBy, Dictionary<string, object> parameters, out int total)
        {
            if(string.IsNullOrEmpty(orderBy))
            {
                orderBy="guid";
            }
            using (ISession session = sessionFactory.OpenSession())
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                string table = "select * from [user]";
                string where = setWhereParameters(cmd, parameters);
                string sql = SQLHelper.CreatPageSelectSQL(pageIndex, pageSize, table, where, orderBy);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = session.Connection;
                DataSet dataset = new DataSet();
                dataset.Load(cmd.ExecuteReader(), LoadOption.Upsert, new string[] { "User" });
                string getCountSQL = SQLHelper.GetTotalCountSQL(table, where);
                cmd.CommandText = getCountSQL;
                total = (int)cmd.ExecuteScalar();
                return dataset;
            }
        }

        private string setWhereParameters(IDbCommand cmd, Dictionary<string, object> parameters)
        {
            string where = "where 1=1";
            if (parameters == null)
                return where;
            if (parameters.ContainsKey("account"))
            {
                where += " AND Account=@Account";
                IDbDataParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = "@Account";
                parameter.Value = parameters["account"];
                parameter.DbType = DbType.String;
                cmd.Parameters.Add(parameter);
            }
            return where;
        }

    }
}
