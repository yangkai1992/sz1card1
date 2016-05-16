using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace sz1card1.Common.JQuery
{
    [Serializable]
    public class AjaxEventArgs<T> : EventArgs where T: class
    {
        private T request;
        private object response;

        /// <summary>
        /// 异步请求参数
        /// </summary>
        public T Request
        {
            get
            {
                return request;
            }
            internal set
            {
                request = value;
            }
        }

        /// <summary>
        /// 异步响应参数
        /// </summary>
        public object Response
        {
            get
            {
                return response;
            }
            set
            {
                response = value;
            }
        }
    }
}
