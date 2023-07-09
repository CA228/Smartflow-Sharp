using System;

namespace Smartflow.Core.Cache
{
    public sealed class CacheFactory
    {
        public static ICache Instance
        {
            get
            {
                return WorkflowGlobalServiceProvider.Resolve<ICache>() ?? new XMLCache();
            }
        }
    }
}
