using AdvancedLog.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedLog.Implementation
{
    public class ExecutionContext : IExecutionContext
    {
        private string _requestCorrelationId;

        public string UserAgent { get; set; }
        public string UserName { get; set; }
        public virtual string RequestCorrelationId
        {
            get
            {

                if (string.IsNullOrWhiteSpace(this._requestCorrelationId))
                {
                    _requestCorrelationId = GenerateULID();
                }
                return _requestCorrelationId;

            }
            set
            {
                _requestCorrelationId = value;
            }
        }

        public virtual string GenerateULID()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffffff");
        }
    }
}
