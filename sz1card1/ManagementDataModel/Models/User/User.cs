using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDataModel.Models.User
{
    [Serializable]
    public class User
    {
        public virtual System.Guid Guid { get; set; }
        public virtual string Account { get; set; }
        public virtual string Password { get; set; }
        public virtual string TrueName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Mobile { get; set; }
        public virtual System.DateTime AddTime { get; set; }
        public virtual bool IsLocked { get; set; }
        public virtual string Meno { get; set; }
        public virtual int UserWeight { get; set; }
        public virtual int UserWeightUsed { get; set; }
        public virtual string Tel { get; set; }
        public virtual string LoginTicket { get; set; }

        public virtual System.Guid UserGroup { get; set; }
    }
}
