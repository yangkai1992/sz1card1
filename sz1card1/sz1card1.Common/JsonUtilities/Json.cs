///<summary>
///Copyright (C) 深圳市一卡易科技发展有限公司
///创建标识：2009-05-11 Created by pq
///功能说明：提供Json序列化集合对象的通用类
///注意事项：TotalCount属性并不是返回的数据条数，而是满足条件的所有数据总条数
///</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace sz1card1.Common
{
    [DataContract]
    public class Json<T> where T : class
    {
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember(Name = "success")]
        public bool Success
        {
            get;
            set;
        }
        /// <summary>
        /// 总记录条数
        /// </summary>
        [DataMember(Name = "totalCount")]
        public int TotalCount
        {
            get;
            set;
        }
        /// <summary>
        /// 数据记录
        /// </summary>
        [DataMember(Name = "data")]
        public List<T> Data
        {
            get;
            set;
        }
    }
}
