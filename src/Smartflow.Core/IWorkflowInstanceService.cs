using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core
{
    public interface IWorkflowInstanceService
    {
        WorkflowInstance GetWorkflowInstance(string id);

    }
}
