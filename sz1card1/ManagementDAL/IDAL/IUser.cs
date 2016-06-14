using ManagementDataModel.Models.User;
using System;
using System.Collections.Generic;
using System.Data;

namespace ManagementDAL.IDAL
{
    public interface IUser
    {
        User GetUser(Guid guid);
        User GetUser(string account);
        DataSet GetUserList(int pageIndex, int pageSize, string orderBy, Dictionary<string, object> parameters, out int total);
    }
}
