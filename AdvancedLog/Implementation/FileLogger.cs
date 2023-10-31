using AdvancedLog.Abstraction;
using AdvancedLog.Attributes;
using AdvancedLog.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedLog.Implementation
{
    public class FileLogger<TSource> : IAppLogger<TSource>
    {
        private readonly ILogger<TSource> _logger;
        private readonly IExecutionContext _executionContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileLogger(ILogger<TSource> logger, IExecutionContext executionContext, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _executionContext = executionContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogErrorAsync(string logCustomType, object flatObject, bool useObjectNameAsContainer = false)
        {
            if (flatObject.GetType().IsValueType || flatObject is string)
            {
                flatObject = new { Error = flatObject.ToString() };
            }

            var info =await GetInfoAsync(logCustomType, flatObject, useObjectNameAsContainer, this._executionContext, this._httpContextAccessor);
            this._logger.LogError(info.Format, info.Values);
            
        }

        public async Task LogInformationAsync(string logCustomType, object flatObject, bool useObjectNameAsContainer = false)
        {
            if (flatObject.GetType().IsValueType || flatObject is string)
            {
                flatObject = new { Info = flatObject.ToString() };
            }

            var info = await GetInfoAsync(logCustomType, flatObject, useObjectNameAsContainer, this._executionContext, this._httpContextAccessor);
            this._logger.LogInformation(info.Format, info.Values);
        }

        private static async Task<(string Format, object[] Values)> GetInfoAsync(string logCustomType, object flatObject, bool useObjectNameAsContainer, IExecutionContext executionContext, IHttpContextAccessor httpContextAccessor)
        {

            string objectName = useObjectNameAsContainer ? flatObject.GetType().Name : "@body";
            IList<PropertyInfo> props = new List<PropertyInfo>(flatObject.GetType().GetProperties());

            IList<PropertyInfo> contextProps = executionContext.GetType()
                                                               .GetProperties()
                                                               .Where(prop => !Attribute.IsDefined(prop, typeof(ExcludeFromLogAttribute)))
                                                               .ToList();

            string format = "{@logType}{@uniqueId}"
                             + string.Join(string.Empty, contextProps.Select(x => $"{{@context_{x.Name}}}"))
                             + "@{RequestBody}"
                             + string.Join(string.Empty, props.Select(x => $"{{@{objectName}_{x.Name}}}"));
            string uniqueLogId = executionContext.GenerateULID();
            object[] valueObjects = new object[]
            {
                logCustomType, uniqueLogId,
            }
            .Concat(contextProps.Select(x => x.GetValue(executionContext, null)))
            .Concat( new[] { await httpContextAccessor!.HttpContext.Request.GetRawBodyAsync() })
            .Concat(props.Select(x => x.GetValue(flatObject, null)))
            .ToArray();
            return (format, valueObjects);
        }

    }
}
