using System;
using System.Collections.Generic;
using System.Data;

namespace Smartflow.Core
{
    public interface IWorkflowService 
    {
        WorkflowStartTask Start(WorkflowStartInput input);
        
        void Submit(WorkflowSubmitInput input);
    }
}
