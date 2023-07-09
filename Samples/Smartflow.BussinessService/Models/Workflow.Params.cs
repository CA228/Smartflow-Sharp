using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.BussinessService.Models
{
    public class WorkflowTask
    {
        public  long Id { get; set; }

        public  string Name { get; set; }

        public  string Code { get; set; }

        public  long ParentId { get; set; }

        public  string InstanceId { get; set; }

        public  int Status { get; set; }
        public  string Type { get; set; }
        public  string Creator { get; set; }
        public  DateTime CreateTime { get; set; }
        
        public string Props
        {
            get;
            set;
        }

    }

    public class WorkflowContext
    {
        public string Id
        {
            get;
            set;
        }

        public long TaskId
        {
            get;
            set;
        }

        public bool Parallel { get; set; } = false;

        public string LineId
        {
            get;
            set;
        }

        public IList<string> Users
        {
            get;
            set;
        }

        public IList<string> Roles
        {
            get;
            set;
        }

        public string Submiter
        {
            get;
            set;
        }

        public string Props
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public IList<WorkflowSubprocess> Children { get; set; } = new List<WorkflowSubprocess>();
    }

    public class WorkflowStartTask
    {
        public string Code { get; set; }

        public string InstanceId { get; set; }

        public long TemplateId { get; set; }
    }

    public class WorkflowSubprocess
    {
        public string InstanceId
        {
            get;
            set;
        }

        public long TemplateId { get; set; }

        public string Code { get; set; }

        public IList<string> Users { get; set; }

        public IList<string> Roles { get; set; }

        public string Props { get; set; }
    }
}
