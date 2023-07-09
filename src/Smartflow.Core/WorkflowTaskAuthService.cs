using System;
using System.Collections.Generic;
using NHibernate;
using Smartflow.Common;
using System.Linq;

namespace Smartflow.Core
{
    public class WorkflowTaskAuthService: IWorkflowTaskAuthService
    {
        public IList<WorkflowTaskAuth> GetTaskAuthListByTaskId(long taskId)
        {
            using ISession session = DbFactory.OpenSession();
            return session.Query<WorkflowTaskAuth>()
                          .Where(c => c.TaskId == taskId)
                          .ToList();
        }

        public void CreateBatchTaskAuth(IList<WorkflowTaskAuth> auths)
        {
            using ISession session = DbFactory.OpenSession();
            foreach (WorkflowTaskAuth auth in auths)
            {
                session.Save(auth);
            }
            session.Flush();
        }

    }
}
