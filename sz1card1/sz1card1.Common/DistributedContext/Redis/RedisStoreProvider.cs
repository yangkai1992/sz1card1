using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sz1card1.Common.Distributed
{
    public class RedisStoreProvider
    {
        internal static readonly string DataKey_Timeout = "_timeout";

        internal static readonly string DataKey_TypeSuffix = "_type";

        private static Dictionary<string, PooledRedisClientManager> _pooledRedisClientManagers;

        private static Dictionary<int, Type> _dicType;

        private static readonly object _syncRoot = new object();

        private static readonly JsonConverter[] _jsonConverters = new JsonConverter[] { new DictionaryConverter(),new GuidConverter() };

        private string _name;

        private PooledRedisClientManager _prcm
        {
            get
            {
                if (!_pooledRedisClientManagers.ContainsKey(_name))
                {
                    lock (_syncRoot)
                    {
                        if (!_pooledRedisClientManagers.ContainsKey(_name))
                        {
                            var redisConfig = RedisConfigSection.GetConfig(_name);
                            string[] readWriteHosts = redisConfig.ReadWriteHosts.Split(new char[] { ',' });
                            string[] readOnlyHosts = redisConfig.ReadOnlyHosts.Split(new char[] { ',' });

                            // 支持读写分离，均衡负载  
                            var pool = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
                            {
                                MaxWritePoolSize = redisConfig.MaxWritePoolSize, // “写”链接池链接数  
                                MaxReadPoolSize = redisConfig.MaxReadPoolSize, // “读”链接池链接数  
                                AutoStart = redisConfig.AutoStart,
                            }, redisConfig.InitalDb, null, redisConfig.PoolTimeOutSeconds);
                            _pooledRedisClientManagers[_name] = pool;
                        }
                    }
                }
                return _pooledRedisClientManagers[_name];
            }
        }

        static RedisStoreProvider()
        {
            _pooledRedisClientManagers=new Dictionary<string,PooledRedisClientManager>();
            //ServiceStack.Logging.LogManager.LogFactory = new Log4NetFactory();
            //ServiceStack.Licensing.RegisterLicense("1001-e1JlZjoxMDAxLE5hbWU6VGVzdCBCdXNpbmVzcyxUeXBlOkJ1c2luZXNzLEhhc2g6UHVNTVRPclhvT2ZIbjQ5MG5LZE1mUTd5RUMzQnBucTFEbTE3TDczVEF4QUNMT1FhNXJMOWkzVjFGL2ZkVTE3Q2pDNENqTkQyUktRWmhvUVBhYTBiekJGUUZ3ZE5aZHFDYm9hL3lydGlwUHI5K1JsaTBYbzNsUC85cjVJNHE5QVhldDN6QkE4aTlvdldrdTgyTk1relY2eis2dFFqTThYN2lmc0JveHgycFdjPSxFeHBpcnk6MjAxMy0wMS0wMX0=");

            _dicType = new Dictionary<int, Type>();
            _dicType.Add(0, typeof(string));
            _dicType.Add(1, typeof(int));
            _dicType.Add(2, typeof(DateTime));
            _dicType.Add(3, typeof(bool));
            _dicType.Add(4, typeof(double));
            _dicType.Add(5, typeof(Decimal));
            _dicType.Add(6, typeof(float));
            _dicType.Add(7, typeof(long));
            _dicType.Add(8, typeof(byte[]));
            _dicType.Add(10, typeof(string[]));
            _dicType.Add(11, typeof(int[]));
            _dicType.Add(12, typeof(System.Data.Linq.Binary));
            _dicType.Add(13, typeof(Enum.LoginTypes));
            _dicType.Add(14, typeof(IdentityImpersonate.IdentityUser));
            _dicType.Add(15, typeof(sz1card1.Common.SessionUser));
        }

        public static Dictionary<string, Dictionary<string, string>> GetPoolsStats()
        {
            return _pooledRedisClientManagers.ToDictionary(k => k.Key, v => v.Value.GetStats());
        }

        public RedisStoreProvider(string name)
        {
            _name = name;
        }

        public IRedisClient RedisClient
        {
            get
            {
                return _prcm.GetClient();
            }
        }

        public void SetHashTimeout(string key, int timeout)
        {
            using (var redisClient = _prcm.GetClient())
            {
                redisClient.SetEntryInHash(key, DataKey_Timeout, timeout.ToString());
            }
        }

        public void ResetHashExpire(string key)
        {
            using (var redisClient = _prcm.GetClient())
            {
                string value = redisClient.GetValueFromHash(key, DataKey_Timeout);
                if (value == null) return;
                var timeout = int.Parse(value);
                redisClient.ExpireEntryIn(key, new TimeSpan(0, timeout, 0));
            }
        }

        public void SetHashExpire(string key, int timeout)
        {
            using (var redisClient = _prcm.GetClient())
            {
                redisClient.ExpireEntryIn(key, new TimeSpan(0, timeout, 0));
            }
        }

        public TimeSpan? GetExpireTime(string key)
        {
            using (var redisClient = _prcm.GetReadOnlyClient())
            {
                return redisClient.GetTimeToLive(key);
            }
        }

        public string GetItem(string key)
        {
            using (var redisClient = _prcm.GetReadOnlyClient())
            {
                return redisClient.Get<string>(key);
            }
        }

        public void SetItem(string key, string data)
        {
            using (var redisClient = _prcm.GetClient())
            {
                redisClient.Set(key, data);
            }
        }

        public void RemoveItem(string key)
        {
            using (var redisClient = _prcm.GetClient())
            {
                redisClient.Remove(key);
            }
        }

        public bool ExistsItem(string key)
        {
            using (var redisClient = _prcm.GetClient())
            {
                return redisClient.ContainsKey(key);
            }
        }

        public object HashGetItem(string key, string dataKey)
        {
            string valueString = null;
            string typeString = null;

            using (var redisClient = _prcm.GetReadOnlyClient())
            {
                valueString = redisClient.GetValueFromHash(key, dataKey);
                if (valueString == null) return null;
                typeString = redisClient.GetValueFromHash(key, getTypeDataKey(dataKey));
            }

            if (typeString == null)
            {
                return valueString;
            }
            else
            {
                var typeNo = 0;
                if (int.TryParse(typeString, out typeNo))
                {
                    return jsonDeserializeObject(valueString, _dicType[typeNo]);
                }
                else
                {
                    var type = JsonConvert.DeserializeObject<Type>(typeString);
                    return jsonDeserializeObject(valueString, type);
                }
            }
        }

        public string HashGetString(string key, string dataKey)
        {
            using (var redisClient = _prcm.GetReadOnlyClient())
            {
                return redisClient.GetValueFromHash(key, dataKey);
            }
        }

        public Dictionary<string, object> HashGetAllItem(string key)
        {
            var dic = new Dictionary<string, object>();

            using (var redisClient = _prcm.GetReadOnlyClient())
            {
                var dicHash = redisClient.GetAllEntriesFromHash(key);
                dicHash.Remove(DataKey_Timeout);
                foreach (var item in dicHash)
                {
                    if (!item.Key.EndsWith(DataKey_TypeSuffix))
                    {
                        string valueString = item.Value;
                        if (valueString == null)
                        {
                            dic.Add(item.Key, null);
                            continue;
                        }

                        string typeString = null;
                        if (dicHash.ContainsKey(item.Key + DataKey_TypeSuffix))
                        {
                            typeString = dicHash[item.Key + DataKey_TypeSuffix];
                        }
                        if (typeString == null)
                        {
                            dic.Add(item.Key, valueString);
                        }
                        else
                        {
                            var typeNo = 0;
                            if (int.TryParse(typeString, out typeNo))
                            {
                                dic.Add(item.Key, jsonDeserializeObject(valueString, _dicType[typeNo]));
                            }
                            else
                            {
                                var type = JsonConvert.DeserializeObject<Type>(typeString);
                                dic.Add(item.Key, jsonDeserializeObject(valueString, type));
                            }
                        }
                    }
                }
            }
            return dic;
        }

        public void HashSetItem(string key, string dataKey, object data)
        {
            if (dataKey.EndsWith(DataKey_TypeSuffix)) throw new ArgumentException("dataKey非法");

            using (var redisClient = _prcm.GetClient())
            {
                if (data == null)
                {
                    redisClient.RemoveEntryFromHash(key, dataKey);
                    redisClient.RemoveEntryFromHash(key, getTypeDataKey(dataKey));
                }
                else
                {
                    var type = data.GetType();
                    //字符串类型特殊处理
                    if (type == _dicType[0])
                    {
                        //设置值
                        redisClient.SetEntryInHash(key, dataKey, (string)data);
                    }
                    else
                    {
                        //设置值
                        redisClient.SetEntryInHash(key, dataKey, JsonConvert.SerializeObject(data));
                    }
                    var item = _dicType.FirstOrDefault(p => p.Value == type);
                    if (!item.Equals(default(KeyValuePair<int, Type>)))
                    {
                        //设置类型
                        redisClient.SetEntryInHash(key, getTypeDataKey(dataKey), item.Key.ToString());
                    }
                    else
                    {
                        //设置类型
                        redisClient.SetEntryInHash(key, getTypeDataKey(dataKey), JsonConvert.SerializeObject(data.GetType()));
                    }
                }
            }
        }

        public void HashRemove(string key, string dataKey)
        {
            using (var redisClient = _prcm.GetClient())
            {
                redisClient.RemoveEntryFromHash(key, dataKey);
            }
        }

        public List<string> HashGetKeys(string key)
        {
            using (var redisClient = _prcm.GetReadOnlyClient())
            {
                var list = redisClient.GetHashKeys(key);
                list.RemoveAll(p => p == DataKey_Timeout);
                return list;
            }
        }

        public List<string> GetKeys(string pattern)
        {
            using (var redisClient = _prcm.GetReadOnlyClient())
            {
                if (string.IsNullOrEmpty(pattern))
                {
                    return redisClient.GetAllKeys();
                }
                return redisClient.SearchKeys(pattern);
            }
        }

        private string getTypeDataKey(string dataKey)
        {
            return dataKey + DataKey_TypeSuffix;
        }

        private object jsonDeserializeObject(string value,Type type)
        {
            if (type==typeof(string))
            {
                return value;
            }
            return JsonConvert.DeserializeObject(value, type, _jsonConverters);
        }

        //public void Dispose()
        //{
        //    _prcm.Dispose();
        //}

        class DictionaryConverter : JsonConverter
        {
            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var genericArguments = objectType.GetGenericArguments();
                var keyType = genericArguments[0];
                var valType = genericArguments[1];
                var dictionaryType = typeof(Dictionary<,>).MakeGenericType(genericArguments);
                var dictionary = (object)null;
                dictionary = Activator.CreateInstance(dictionaryType);
                var jObject = JObject.Load(reader);
                foreach (var item in jObject)
                {
                    var key = (object)null;
                    if (keyType == typeof(Guid))
                    {
                        key = Guid.Parse(item.Key);
                    }
                    else
                    {
                        key = JsonConvert.DeserializeObject(item.Key, keyType);
                    }
                    var value = JsonConvert.DeserializeObject(item.Value.ToString(), valType);
                    dictionaryType.GetMethod("Add").Invoke(dictionary, new object[] { key, value });
                }
                return dictionary;
            }

            public override bool CanConvert(Type objectType)
            {
                if (!objectType.IsValueType && objectType.IsGenericType)
                    return (objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>));

                return false;
            }
        }

        class GuidConverter : JsonConverter
        {
            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return Guid.Parse(reader.Value.ToString());
            }

            public override bool CanConvert(Type objectType)
            {
                var r = (objectType == typeof(Guid));
                return r;
            }
        }
    }
}
