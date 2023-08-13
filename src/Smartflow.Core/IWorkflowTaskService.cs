using Smartflow.Core.Elements;
using System.Collections.Generic;
using System;

namespace Smartflow.Core
{
    public interface IWorkflowTaskService 
    {
        WorkflowTask Persist(WorkflowTask task);

        WorkflowTask GetTaskById(long id);

        IList<WorkflowTask> GetTaskListByInstanceId(string instanceId);

        Boolean CheckTaskCompleted(string instanceId, string nodeCode);

        IList<WorkflowTask> GetUserTaskListByUserId(string userId);

        WorkflowTask CreateTask(Node to,string lineCode, string instanceId, string publisher, long parentId, bool parallel, int status = 0);
    }
}
