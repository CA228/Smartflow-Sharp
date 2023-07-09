using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core
{
    public class WorkflowProcessRecord
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Comment { get; set; }

        public virtual string InstanceId { get; set; }

        public virtual string LineName { get; set; }

        public virtual string CreatorName { get; set; }

        public virtual DateTime CreateTime { get; set; }
    }
}
