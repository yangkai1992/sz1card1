using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringLengthValidator : Attribute,IValidator
    {
        private int minLength = 0;
        private int maxLength = int.MaxValue;

        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLengh
        {
            get
            {
                return minLength;
            }
            set
            {
                minLength = value;
            }
        }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                maxLength = value;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 执行验证
        /// </summary>
        /// <param name="obj">待验证的值</param>
        public void Validate(object obj)
        {
            if (obj.ToString().Length > 0)
            {
                if (obj.ToString().Length < minLength && obj.ToString().Length > maxLength)
                {
                    throw new ValidationException(ErrorMessage);
                }
            }
        }
    }
}
