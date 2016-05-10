using ManagementDAL.DAL.NHibernate;
using ManagementDataModel.Models.User;
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
            User user = userDao.Get("yangkai");
            Console.ReadKey();
        }
    }
}
