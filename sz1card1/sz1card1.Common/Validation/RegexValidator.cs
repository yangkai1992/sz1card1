using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace sz1card1.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RegexValidator : Attribute, IValidator
    {
        /// <summary>
        /// 正则表达式
        /// </summary>
        public string Pattern
        {
            get;
            set;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
            get;
            set;
        }

        public RegexValidator(string pattern)
        {
            this.Pattern = pattern;
        }

        /// <summary>
        /// 执行验证
        /// </summary>
        /// <param name="obj">待验证的值</param>
        public void Validate(object obj)
        {
            if (obj.ToString().Length > 0)
            {
                Regex reg = new Regex(Pattern);
                if (!reg.Match(obj.ToString()).Success)
                {
                    throw new ValidationException(ErrorMessage);
                }
            }
        }
    }
}
