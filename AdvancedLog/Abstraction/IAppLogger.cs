using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedLog.Abstraction
{
    public interface IAppLogger<TSource>
    {
        Task LogInformationAsync(string logCustomType, object flatObject, bool useObjectNameAsContainer = false);
        Task LogErrorAsync(string logCustomType, object flatObject, bool useObjectNameAsContainer = false);
    }
}
