using System;
using System.Collections.Generic;

namespace Smartflow.Core
{
    public class AbstractWorkflow
    {
        public IWorkflowNodeService NodeService => WorkflowGlobalServiceProvider.Resolve<IWorkflowNodeService>();

        public IWorkflowTemplateService TemplateService => WorkflowGlobalServiceProvider.Resolve<IWorkflowTemplateService>();

        public IWorkflowRecordService RecordService => WorkflowGlobalServiceProvider.Resolve<IWorkflowRecordService>();

        public IWorkflowTaskService TaskService => WorkflowGlobalServiceProvider.Resolve<IWorkflowTaskService>();
    }
}
