using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace sz1card1.Common.Validation
{
    public class ValidationException:Exception
    {
        public ValidationException()
        {

        }

        public ValidationException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public ValidationException(string message)
            : base(message)
        {

        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
