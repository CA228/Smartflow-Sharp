using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using System.Collections.Generic;

namespace Smartflow.Core.Handler
{
    public class WorkflowCollaborationTaskActorHandler : IHandler
    {
        private readonly IWorkflowTaskAuthService authService = WorkflowGlobalServiceProvider.Resolve<IWorkflowTaskAuthService>();
  
        private long TaskId { get; set; }

        private string Actor { get; set; }

        public WorkflowCollaborationTaskActorHandler(long taskId, string actor)
        {
            this.TaskId = taskId;
            this.Actor = actor;
        }

        public void Done()
        {
            IList<WorkflowTaskAuth> auths = GetTaskAuths();
            if (auths.Count > 0)
            {
                authService.CreateBatchTaskAuth(auths);
            }
        }

        protected IList<WorkflowTaskAuth> GetTaskAuths()
        {
            return new List<WorkflowTaskAuth>
            {
                new WorkflowTaskAuth
                {
                    AuthCode = Actor,
                    TaskId = TaskId,
                    Type = 1
                }
            };
        }
    }
}
