using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.BussinessService.Models
{
    public class Start
    {
        public string BusinessId { get; set; }
        public string Creator { get; set; }
        public string CategoryCode { get; set; }
        public IList<string> Users { get; set; }
        public IList<string> Roles { get; set; }
        public string Message { get; set; }

        public bool Parallel { get; set; } = false;

        public IList<WorkflowSubprocess> Children { get; set; } = new List<WorkflowSubprocess>();
    }
}
