using ManagementDAL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAL.DAL.EF.User
{
    public sealed class UserDAL:ManagementContext,IUser
    {
        public ManagementDataModel.Models.User GetUser(Guid guid)
        {
            return DataContext.Users.FirstOrDefault(x => x.Guid == guid);
        }

        public ManagementDataModel.Models.User GetUser(string account)
        {
            return DataContext.Users.FirstOrDefault(x => x.Account == account);
        }
    }
}
