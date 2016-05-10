using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagementDataModel.Models.User;

namespace ManagementDAL.DAL.EF
{
    public class ManagementEntities : DbContext
    {
         public ManagementEntities()
            : base("name=ManagementEntities")
        {
        }

         public virtual DbSet<ManagementDataModel.Models.User.User> Users { get; set; }
    }
}
