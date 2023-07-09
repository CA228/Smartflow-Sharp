using System;
using System.Collections.Generic;
using Smartflow.Core.Chain;
using Smartflow.Core.Internals;
using Smartflow.Core.Mail;

namespace Smartflow.Core.Handlers
{
    public class WorkflowTaskMailHandler : IHandler
    {
        private readonly IMailService mailService = WorkflowGlobalServiceProvider.Resolve<IMailService>();
        private readonly IWorkflowTaskAuthService authService = WorkflowGlobalServiceProvider.Resolve<IWorkflowTaskAuthService>();
        private readonly IWorkflowConversion conversionService= WorkflowGlobalServiceProvider.Resolve<IWorkflowConversion>();
        
        private IList<string> Sender { get; set; }
        private string Comment { get; set; }

        public WorkflowTaskMailHandler(string categoryCode,long taskId)
        {
            IList<WorkflowTaskAuth> auths=authService.GetTaskAuthListByTaskId(taskId);
            Sender = conversionService?.GetReceiveAddress(auths);
            string categoryName = conversionService?.GetCategoryName(categoryCode);
            if (String.IsNullOrEmpty(categoryName)) {
                Comment = ResourceManage.NOTIFICATION_TASK_CONTENT_DEFAULT;
            }
            else{
               Comment = String.Format(ResourceManage.NOTIFICATION_TASK_CONTENT, categoryName);
            }
        }

        public void Done() => mailService.Notification(ResourceManage.NOTIFICATION_TASK_TITLE, Sender, Comment);
    }
}
