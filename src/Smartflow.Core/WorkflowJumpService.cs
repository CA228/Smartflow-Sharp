using Smartflow.Core.Cache;
using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using Smartflow.Core.Handlers;
using Smartflow.Core.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Core
{
    public class WorkflowJumpService
    {
        protected AbstractWorkflow AbsWorkflowService
        {
            get;
        }

        protected IWorkflowBasicCoreService CoreService
        {
            get;
            set;
        }

        public WorkflowJumpService(IWorkflowBasicCoreService coreService)
        {
            this.CoreService = coreService;
            this.AbsWorkflowService = coreService.AbsWorkflowService;
        }

        public void Go(WorkflowContext context)
        {
            WorkflowInstance instance = WorkflowInstance.GetWorkflowInstance(context.Id);
            WorkflowTask workflowTask = AbsWorkflowService.TaskService.GetTaskById(context.TaskId);
            workflowTask.Status = 1;
            AbsWorkflowService.TaskService.Persist(workflowTask);
            IList<Node> nodes = CacheFactory.Instance.GetNodesByTemplateId(instance.TemplateId);
            Node current = nodes.Where(e => e.Id == workflowTask.Code).FirstOrDefault();
            Transition transition = current.Transitions.Where(c => c.Id == context.LineId).FirstOrDefault();
            AbsWorkflowService.TaskService.CreateTask(instance, nodes,workflowTask.Code,context.Props, context.LineId, context.Submiter,context.Parallel, workflowTask.Id, context.Children, context.Users, context.Roles);
            
            ChainFactory.Chain()
                  .Add(new WorkflowRecordHandler(instance.Id, context.Submiter, current, transition.Name, context.Message))
                  .Add(new WorkflowRecordMailHandler(instance.Creator, context.Message))
                  .Done();
        }
    }
}
