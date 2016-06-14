using ManagementBLL.User;
using ManagementDataModel.Models.User;
using System;
using System.Data;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //UserDao userDao = new UserDao();
            //User user = userDao.Get("yangkai");
            User user = UserBLL.GetUser("admin");

           // DataSet dataset = UserBLL.GetUserList(1, 2, "sd");

            string s = "sdf";
            Console.ReadKey();
        }
    }
}
