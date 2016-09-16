using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Exceptions
{
    /// <summary>
    /// Occurs when there are errors generating the context.
    /// </summary>
    public class SampleItemsContextException : Exception
    {
        public SampleItemsContextException()
        {
        }

        public SampleItemsContextException(string message)
            :base(message)
        {
        }

        public SampleItemsContextException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
