using System;

namespace Smartflow.Core
{
    public class WorkflowTask
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual long ParentId { get; set; }

        public virtual string InstanceId { get; set; }

        public virtual int Status { get; set; }

        /// <summary>
        /// 0:串行 1：并行
        /// </summary>
        public virtual int Parallel { get; set; }

        public virtual string Type { get; set; }
        
        public virtual string Creator { get; set; }
        
        public virtual DateTime CreateTime { get; set; }
    }
}
