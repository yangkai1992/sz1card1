using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDataModel.User
{
    [Serializable]
    public class User
    {
        private Guid guid = Guid.Empty;
        private string account = string.Empty;
        private string password = string.Empty;
        private string trueName = string.Empty;
        private string email = null;
        private string mobile = null;
        private DateTime addTime = DateTime.Now;
        private Guid userGroupGuid = Guid.Empty;
        private bool isLocked = false;
        private string meno = null;
        private int userWeight = 0;
        private int userWeightUsed = 0;
        private string tel = null;

        public Guid Guid
        {
            get { return guid; }
        }
    }
}
