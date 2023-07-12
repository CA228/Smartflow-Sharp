using Smartflow.Core.Elements;
using System.Collections.Generic;

namespace Smartflow.Core
{
    public interface IWorkflowTaskService 
    {
        WorkflowTask Persist(WorkflowTask task);

        WorkflowTask GetTaskById(long id);

        IList<WorkflowTask> GetTaskListByInstanceId(string instanceId);

        IList<WorkflowTask> GetUserTaskListByUserId(string userId);

        WorkflowTask CreateTask(Node to,string lineCode, string instanceId, string publisher, long parentId, bool parallel, int status = 0);
    }
}
