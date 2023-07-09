using System.Collections.Generic;

namespace Smartflow.Core
{
    public interface IWorkflowTaskAuthService
    {
        void CreateBatchTaskAuth(IList<WorkflowTaskAuth> auths);

        IList<WorkflowTaskAuth> GetTaskAuthListByTaskId(long taskId);
    }
}