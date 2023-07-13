using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using System;

namespace Smartflow.Core.Handler
{
    public class WorkflowRecordHandler : IHandler
    {
        private readonly IWorkflowRecordService recordService = WorkflowGlobalServiceProvider.Resolve<IWorkflowRecordService>();

        private string InstanceId
        {
            get;
            set;
        }

        private string Creator
        {
            get;
            set;
        }

        private Node From
        {
            get;
            set;
        }

        private string Line
        {
            get; set;
        }

        private string Comment
        {
            get; set;
        }

        public WorkflowRecordHandler(string instanceId,string creator,Node from, string line, string comment)
        {
            this.InstanceId = instanceId;
            this.Creator = creator;
            this.From = from;
            this.Line = line;
            this.Comment = comment;
        }

        public void Done()
        {
            recordService.Persist(new WorkflowRecord
            {
                Code = From.Id,
                Name = From.Name,
                InstanceId = InstanceId,
                LineName = Line,
                Comment = Comment,
                Creator = Creator,
                CreateTime = DateTime.Now
            });
        }
    }
}
