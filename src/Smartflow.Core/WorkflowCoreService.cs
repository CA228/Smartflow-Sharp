using Smartflow.Core;
using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Smartflow.Core
{
    public class WorkflowCoreService : IWorkflowBasicCoreService, IWorkflowCoreService
    {
        public AbstractWorkflow AbsWorkflowService => WorkflowGlobalServiceProvider.Resolve<AbstractWorkflow>();

        public void Go(WorkflowContext context)
        {
            new WorkflowJumpService(this).Go(context);
        }
    }
}
