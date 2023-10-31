using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedLog.Abstraction
{
    public interface IExecutionContext
    {
        string UserName { get; set; }
        string UserAgent { get; set; }
        abstract string RequestCorrelationId { get; set; }

       abstract string GenerateULID();
    }
}
