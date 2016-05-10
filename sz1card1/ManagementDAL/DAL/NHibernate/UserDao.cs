using ManagementDAL.IDAL;
using ManagementDataModel.Models.User;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAL.DAL.NHibernate
{
    public class UserDao : IUser
    {
        private ISessionFactory sessionFactory;

        public UserDao()
        {
            var cfg = new Configuration().Configure("Config/MySql.cfg.xml");
            sessionFactory = cfg.BuildSessionFactory();
        }

        public ManagementDataModel.Models.User.User GetUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        public ManagementDataModel.Models.User.User GetUser(string account)
        {
            throw new NotImplementedException();
        }

        public ManagementDataModel.Models.User.User Get(string account)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Get<User>(account);
            }
        }
    }
}
