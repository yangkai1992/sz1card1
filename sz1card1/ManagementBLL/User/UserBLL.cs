using ManagementDAL.DAL.EF.User;
using ManagementDAL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagementDataModel.Models;

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
                    iuser = new UserDAL();
                }
                return iuser;
            }
        }

        public static ManagementDataModel.Models.User GetUser(string account)
        {
            return IUser.GetUser(account);
        }

        public static List<ManagementDataModel.Models.User> GetUserList(int pageIndex, int pageSize, string orderBy)
        {
            return IUser.GetUserList(pageIndex, pageSize, orderBy);
        }
    }
}
