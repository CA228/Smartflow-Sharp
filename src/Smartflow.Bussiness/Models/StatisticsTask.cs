using System;

namespace Smartflow.Bussiness.Models
{
    public class StatisticsTask
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual long ParentId { get; set; }

        public virtual string InstanceId { get; set; }

        public virtual string CategoryName { get; set; }

        public virtual string TemplateName { get; set; }

        public virtual DateTime CreateTime { get; set; }

        public virtual string Actor { get; set; }
    }
}
