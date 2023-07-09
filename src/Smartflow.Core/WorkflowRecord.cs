using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core
{
    public class WorkflowRecord
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string Comment { get; set; }

        public virtual string LineName { get; set; }

        public virtual string InstanceId { get; set; }

        public virtual string Creator { get; set; }

        public virtual DateTime CreateTime { get; set; }
    }
}
