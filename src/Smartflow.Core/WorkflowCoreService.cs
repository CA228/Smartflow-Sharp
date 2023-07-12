using Smartflow.Core;
using Smartflow.Core.Dispatch;

namespace Smartflow.Core
{
    public class WorkflowCoreService : AbstractWorkflow,IWorkflowCoreService
    {
        public void Go(WorkflowContext context)
        {
            WorkflowInstance instance = WorkflowInstance.GetWorkflowInstance(context.Id);
            WorkflowTask task = this.TaskService.GetTaskById(context.TaskId);
            task.Status = 1;
            TaskService.Persist(task);


            DispatchTaskStrategy.CreatetDispatchTaskStrategy(new DispatchCore(instance, task, context)).Dispatch();
        }
    }
}
