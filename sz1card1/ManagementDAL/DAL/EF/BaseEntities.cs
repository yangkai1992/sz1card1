using ManagementDataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAL.DAL.EF
{
    public class BaseEntities
    {
        protected static ManagementEntities ManagementContext
        {
            get
            {
                if (CallContext.GetData("DataContext") == null)
                {
                    ManagementEntities dataContext = new ManagementEntities();
                    CallContext.SetData("DataContext", dataContext);
                }
                ManagementEntities context = (ManagementEntities)CallContext.GetData("DataContext");
                return context;
            }
        }
    }
}
