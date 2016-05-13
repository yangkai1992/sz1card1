using System.Runtime.Remoting.Messaging;
using ManagementDataModel.Models;

namespace ManagementDAL.DAL.EF
{
    public class ManagementContext 
    {
        protected static ManagementEntities DataContext
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
