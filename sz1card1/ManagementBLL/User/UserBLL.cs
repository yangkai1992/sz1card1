using ManagementDAL.IDAL;
using System.Collections.Generic;
using ManagementDAL.DAL.NHibernate;
using System;
using System.Data;

namespace ManagementBLL.User
{
    public sealed class UserBLL
    {
        private static IUser iuser = null;
        private static IUser IUser
        {
            get
            {
                if (iuser == null)
                {
                    iuser = new UserDao();
                }
                return iuser;
            }
        }

        public static ManagementDataModel.Models.User.User GetUser(string account)
        {
            return IUser.GetUser(account);
        }

        public static ManagementDataModel.Models.User.User GetUser(Guid guid)
        {
            return IUser.GetUser(guid);
        }


        public static DataSet GetUserList(int pageIndex, int pageSize, string orderBy, Dictionary<string, object> parameters, out int total)
        {
            return IUser.GetUserList(pageIndex, pageSize, orderBy, parameters, out total);
        }
    }
}
