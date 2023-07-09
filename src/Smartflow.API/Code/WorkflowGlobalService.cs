using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartflow.Bussiness;
using Smartflow.Core;

namespace Smartflow.API.Code
{
    public class WorkflowGlobalService
    {
        public static void RegisterService()
        {
            WorkflowGlobalServiceProvider.RegisterGlobalService(typeof(WorkflowBridgeService));
            WorkflowGlobalServiceProvider.RegisterGlobalService(typeof(WorkflowConversionService));
        }
    }
}
