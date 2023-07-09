using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core
{
    public interface IWorkflowCoreService
    {
        void Go(WorkflowContext context);
    }
}
