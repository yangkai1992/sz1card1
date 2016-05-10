using ManagementDAL.DAL.NHibernate;
using ManagementDataModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UserDao userDao = new UserDao();
            User user = userDao.Get(new Guid("D637501B-8D79-490D-8E23-6919BAED0587"));
            Console.ReadKey();
        }
    }
}
