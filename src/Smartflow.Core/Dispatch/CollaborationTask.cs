using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using Smartflow.Core.Handler;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Core.Dispatch
{
    public class CollaborationTask : AbstractWorkflow, IDispatch
    {
        private readonly IWorkflowConversion conversionService = WorkflowGlobalServiceProvider.Resolve<IWorkflowConversion>();

        protected WorkflowInstance Instance { get; }

        protected long TaskId { get; }

        protected Node To { get; }

        protected string TransitionId { get; }

        /// <summary>
        /// 提交者
        /// </summary>
        public string Submiter
        {
            get;
            set;
        }

        /// <summary>
        /// 参与者
        /// </summary>
        public IList<string> Users
        {
            get;
            set;
        }

        /// <summary>
        /// 参与角色
        /// </summary>
        public IList<string> Roles
        {
            get;
            set;
        }

        protected CollaborationTask(WorkflowInstance instance, Node to, string transitionId, long taskId, string submiter,IList<string> users, IList<string> roles)
        {
            this.Instance = instance;
            this.TaskId = taskId;
            this.To = to;
            this.TransitionId = transitionId;
            this.Submiter = submiter;
            this.Roles = roles;
            this.Users = users;
        }

        public void Dispatch()
        {
            IList<WorkflowTaskActor> taskActors = Internals.Utils.GetTaskActorList(To, Users, Roles);
            List<string> actors = conversionService.GetActorList(taskActors.Where(c => c.Type != 1).ToList()).ToList();
            if (taskActors.Count(c => c.Type == 1) > 0)
            {
                actors.AddRange(taskActors.Where(c => c.Type == 1).Select(c => c.Id).ToList());
            }
            foreach (string actor in actors)
            {
               var afterTask=TaskService.CreateTask(To, TransitionId, Instance.Id, Submiter, TaskId, true,0);
               ChainFactory.Chain()
                     .Add(new WorkflowCollaborationTaskActorHandler(afterTask.Id, actor))
                     .Add(new WorkflowTaskMailHandler(Instance.CategoryCode, afterTask.Id))
                     .Done();
            }
        }

        public static IDispatch CreateInstance(WorkflowInstance instance,Node to,string transitionId, long taskId, string submiter, IList<string> users, IList<string> roles)
        {
            return new CollaborationTask(instance, to, transitionId, taskId, submiter, users, roles);
        }
    }
}
