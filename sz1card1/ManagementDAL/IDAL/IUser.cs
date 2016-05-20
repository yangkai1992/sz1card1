using ManagementDataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAL.IDAL
{
    public interface IUser
    {
        User GetUser(Guid guid);
        User GetUser(string account);
        List<ManagementDataModel.Models.User> GetUserList(int pageIndex, int pageSize, string orderBy);
    }
}
