using ManagementDAL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAL.DAL.EF.User
{
    public sealed class UserDAL:BaseEntities,IUser
    {
        public ManagementDataModel.Models.User.User GetUser(Guid guid)
        {
            return ManagementContext.Users.FirstOrDefault(x => x.Guid == guid);
        }


        public ManagementDataModel.Models.User.User GetUser(string account)
        {
            return ManagementContext.Users.FirstOrDefault(x => x.Account == account);
        }
    }
}
