using NHibernate;
using Smartflow.Bussiness.Interfaces;
using Smartflow.Bussiness.Models;
using Smartflow.Common;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Bussiness.Queries
{
    public class WorkflowReportService : IWorkflowReportService
    {
        public IList<StatisticsTask> GetPendingTaskList()
        {
            using ISession session = DbFactory.OpenSession();
            return session.Query<StatisticsTask>().OrderByDescending(e => e.CreateTime).ToList();
        }

        public IList<StatisticsInstance> GetStatisticsInstanceByUserId(string userId)
        {
            using ISession session = DbFactory.OpenSession();
            return session.GetNamedQuery("queryStatisticsInstanceByUserId")
                            .SetParameter("userId", userId)
                            .List<StatisticsInstance>();
        }
    }
}
