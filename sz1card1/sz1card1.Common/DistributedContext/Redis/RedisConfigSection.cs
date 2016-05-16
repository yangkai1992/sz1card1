using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace sz1card1.Common.Distributed
{
    /// <summary>
    /// Redis连接配置信息
    /// </summary>
    public class RedisConfigSection : ConfigurationSection
    {
        public static RedisConfig GetConfig()
        {
            RedisConfigSection section = (RedisConfigSection)ConfigurationManager.GetSection("redisConfig");
            var collection = section.RedisConfig;
            if (collection.Count == 0)
            {
                return null;
            }
            return collection[0];
        }

        public static RedisConfig GetConfig(string addName)
        {
            RedisConfigSection section = (RedisConfigSection)ConfigurationManager.GetSection("redisConfig");
            var collection = section.RedisConfig;
            if (collection.Count == 0)
            {
                return null;
            }
            return collection[addName];
        }


        [ConfigurationProperty("", IsDefaultCollection = true)]
        public RedisConfigCollection RedisConfig
        {
            get
            {
                return (RedisConfigCollection)base[""];
            }
        }
    }

    public class RedisConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RedisConfig();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RedisConfig)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        protected override string ElementName
        {
            get
            {
                return "add";
            }
        }

        public RedisConfig this[int index]
        {
            get
            {
                return (RedisConfig)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public RedisConfig this[string name]
        {
            get
            {
                return (RedisConfig)BaseGet(name);
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }

    public class RedisConfig : ConfigurationElement
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
            set
            {
                base["name"] = value;
            }
        }

        /// <summary>
        /// 负责写入的Redis链接地址，一般为一个服务器，我们称为主服务器
        /// </summary>
        [ConfigurationProperty("readWriteHosts", IsRequired = true)]
        public string ReadWriteHosts
        {
            get
            {
                return (string)base["readWriteHosts"];
            }
            set
            {
                base["readWriteHosts"] = value;
            }
        }


        /// <summary>
        /// 负责读的Redis链接地址，它一般由多个服务器组件，一般称为从服务器(slave)，各个服务器之间用逗号分开
        /// </summary>
        [ConfigurationProperty("readOnlyHosts", IsRequired = true)]
        public string ReadOnlyHosts
        {
            get
            {
                return (string)base["readOnlyHosts"];
            }
            set
            {
                base["readOnlyHosts"] = value;
            }
        }


        /// <summary>
        /// 最大写链接数
        /// </summary>
        [ConfigurationProperty("maxWritePoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxWritePoolSize
        {
            get
            {
                int _maxWritePoolSize = (int)base["maxWritePoolSize"];
                return _maxWritePoolSize > 0 ? _maxWritePoolSize : 5;
            }
            set
            {
                base["maxWritePoolSize"] = value;
            }
        }


        /// <summary>
        /// 最大读链接数
        /// </summary>
        [ConfigurationProperty("maxReadPoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxReadPoolSize
        {
            get
            {
                int _maxReadPoolSize = (int)base["maxReadPoolSize"];
                return _maxReadPoolSize > 0 ? _maxReadPoolSize : 5;
            }
            set
            {
                base["maxReadPoolSize"] = value;
            }
        }


        /// <summary>
        /// 自动重启
        /// </summary>
        [ConfigurationProperty("autoStart", IsRequired = false, DefaultValue = true)]
        public bool AutoStart
        {
            get
            {
                return (bool)base["autoStart"];
            }
            set
            {
                base["autoStart"] = value;
            }
        }



        /// <summary>
        /// 本地缓存到期时间(超时时间)，单位:秒
        /// </summary>
        [ConfigurationProperty("localCacheTime", IsRequired = false, DefaultValue = 36000)]
        public int LocalCacheTime
        {
            get
            {
                return (int)base["localCacheTime"];
            }
            set
            {
                base["localCacheTime"] = value;
            }
        }


        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        [ConfigurationProperty("recordeLog", IsRequired = false, DefaultValue = false)]
        public bool RecordeLog
        {
            get
            {
                return (bool)base["recordeLog"];
            }
            set
            {
                base["recordeLog"] = value;
            }
        }

        /// <summary>
        /// 数据库id
        /// </summary>
        [ConfigurationProperty("initalDb", IsRequired = false, DefaultValue = 0)]
        public int InitalDb
        {
            get
            {
                return (int)base["initalDb"];
            }
            set
            {
                base["initalDb"] = value;
            }
        }

                /// <summary>
        /// 数据库id
        /// </summary>
        [ConfigurationProperty("poolTimeOutSeconds", IsRequired = false, DefaultValue = 10)]
        public int PoolTimeOutSeconds
        {
            get
            {
                return (int)base["poolTimeOutSeconds"];
            }
            set
            {
                base["poolTimeOutSeconds"] = value;
            
            }
        }
    }
}
