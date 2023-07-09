using System;
using System.Collections.Generic;
using Smartflow.Core.Chain;
using Smartflow.Core.Internals;
using Smartflow.Core.Mail;

namespace Smartflow.Core.Handlers
{
    public class WorkflowRecordMailHandler : IHandler
    {
        private readonly IMailService mailService = WorkflowGlobalServiceProvider.Resolve<IMailService>();
        private readonly IWorkflowConversion conversionService = WorkflowGlobalServiceProvider.Resolve<IWorkflowConversion>();

        private IList<string> Sender { get; set; }
        private string Comment { get; set; }

        public WorkflowRecordMailHandler(string start, string comment)
        {
            this.Sender = conversionService?.GetReceiveAddress(new List<string>() { start });
            this.Comment = comment;
        }

        public void Done() => mailService.Notification(ResourceManage.NOTIFICATION_RECORD_TITLE, Sender, Comment);
    }
}
