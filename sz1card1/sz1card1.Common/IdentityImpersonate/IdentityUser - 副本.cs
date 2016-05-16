using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Principal;
using sz1card1.Common.Redis;

namespace sz1card1.Common.IdentityImpersonate
{
    /// <summary>
    /// 身份标识用户
    /// </summary>
    [Serializable, DataContract]
    public class IdentityUser
    {
        private string name;
        private string password;

        /// <summary>
        /// 默认为Administrator账号
        /// </summary>
        public IdentityUser()
        {
            this.name = "tech-admin";
            this.password = "201card113@admin".ToDes();
        }

        /// <summary>
        /// 身份标识用户
        /// </summary>
        /// <param name="name">域账号名称</param>
        /// <param name="password">域账号密码(Des加密)</param>
        public IdentityUser(string name, string password)
        {
            this.name = name;
            this.password = password;
        }

        /// <summary>
        /// 域账号名称
        /// </summary>
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// 域账号密码(Des加密)
        /// </summary>
        [DataMember]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public static IdentityUser GetIdentityUser()
        {
            string memCachedKey = typeof(IdentityUser).ToString().ToDes();
            return RedisSession.Current[memCachedKey] as IdentityUser;
        }

        public static void SaveIdentityUser(IdentityUser identityUser)
        {
            string memCachedKey = typeof(IdentityUser).ToString().ToDes();
            RedisSession.Current[memCachedKey] = identityUser;
        }

        public string ToXml()
        {
            XmlSerializer ser = new XmlSerializer(this.GetType());
            StringBuilder result = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(result))
            {
                ser.Serialize(writer, this);
            }
            return result.ToString();
        }

        public static IdentityUser FromXml(string xml)
        {
            IdentityUser identityUser = null;
            XmlSerializer ser = new XmlSerializer(typeof(IdentityUser));
            using (StringReader reader = new StringReader(xml))
            {
                identityUser = ser.Deserialize(reader) as IdentityUser;
            }
            return identityUser;
        }
    }
}
