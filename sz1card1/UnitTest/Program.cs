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
            User user = userDao.Get(new Guid("9E16018F-9774-4D83-A693-1D1C7F6C1E59"));
            Console.ReadKey();
        }
    }
}
