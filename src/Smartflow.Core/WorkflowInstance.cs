using NHibernate;
using Smartflow.Common;
using System;
using System.Linq;

namespace Smartflow.Core
{
    public class WorkflowInstance
    {
        public virtual string Id
        {
            get;
            set;
        }

        public virtual long TemplateId { get; set; }

        public virtual string Creator
        {
            get;
            set;
        }

        public virtual int CategoryId
        {
            get;
            set;
        }

        public virtual string BusinessId
        {
            get;
            set;
        }
       
        public virtual DateTime CreateTime
        {
            get;
            set;
        }

        public static WorkflowInstance GetWorkflowInstance(string id)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                    .Query<WorkflowInstance>()
                    .Where(e => e.Id == id)
                    .FirstOrDefault();
        }
    }
}
