using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Smartflow.Core.Elements;

namespace Smartflow.Core
{
    public interface IWorkflowService 
    {
        WorkflowStartTask Start(WorkflowStartInfo start);
        
        void Submit(WorkflowContext context);
    }
}
