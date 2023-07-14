using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Core
{
    public sealed class WorkflowEngine
    {
        private static readonly WorkflowEngine singleton = new WorkflowEngine();
        private readonly IWorkflowService workflowService = WorkflowGlobalServiceProvider.Resolve<IWorkflowService>();

        public AbstractWorkflow AbstractCoreService
        {
            get
            {
                return WorkflowGlobalServiceProvider.Resolve<AbstractWorkflow>();
            }
        }

        private WorkflowEngine()
        {
        }

        public static WorkflowEngine Instance
        {
            get { return singleton; }
        }

        public WorkflowStartTask Start(WorkflowStartInfo start) => workflowService.Start(start);

        public void Submit(WorkflowContext context) => workflowService.Submit(context);
    }
}
