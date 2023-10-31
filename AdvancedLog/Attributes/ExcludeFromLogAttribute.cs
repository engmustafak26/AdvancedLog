using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedLog.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromLogAttribute : System.Attribute
    {
        public string Property { get; private set; }

        public ExcludeFromLogAttribute(string property)
        {
            Property = property;
        }
    }
}
