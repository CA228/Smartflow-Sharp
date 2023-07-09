using NHibernate;
using Smartflow.Common;
using System.Collections.Generic;

namespace Smartflow.Core
{
    public class WorkflowRecordService: IWorkflowRecordService
    {
        public IList<WorkflowProcessRecord> GetRecordListByInstanceId(string instanceId)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                  .GetNamedQuery("queryProcessRecordListByInstanceId")
                  .SetParameter("instanceId", instanceId).List<WorkflowProcessRecord>();
        }

        public void Persist(object record)
        {
            using ISession session = DbFactory.OpenSession();
            session.Save(record);
            session.Flush();
        }
    }
}
