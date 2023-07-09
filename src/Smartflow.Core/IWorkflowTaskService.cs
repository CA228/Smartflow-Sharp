using Smartflow.Core.Elements;
using System.Collections.Generic;

namespace Smartflow.Core
{
    public interface IWorkflowTaskService 
    {
        WorkflowTask Persist(WorkflowTask task);

        WorkflowTask GetTaskById(long id);
        
        IList<WorkflowTask> GetTaskListByInstanceId(string code,string instanceId);

        IList<WorkflowTask> GetUserTaskListByUserId(string userId);

        void CreateSubprocess(WorkflowInstance instance, long taskId,string publisher, WorkflowSubprocess subprocess, bool parallel);

        void CreateTask(WorkflowInstance instance, IList<Node> nodes, string nodeCode,string props,string transitionId,string publisher,bool parallel,long parentId, IList<WorkflowSubprocess> children, IList<string> users, IList<string> roles);
    }
}
