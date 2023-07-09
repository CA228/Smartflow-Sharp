using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartflow.Abstraction.DTO
{
    public class RecordDto
    {
        public virtual string NID
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Comment
        {
            get;
            set;
        }

        public virtual string InstanceID
        {
            get;
            set;
        }

        public virtual DateTime CreateTime
        {
            get;
            set;
        }

        public virtual string Url
        {
            get;
            set;
        }

        public virtual string AuditUserID
        {
            get;
            set;
        }

        public virtual string AuditUserName
        {
            get;
            set;
        }

        public virtual string NodeName
        {
            get;
            set;
        }

        public virtual string NodeID
        {
            get;
            set;
        }
    }
}
