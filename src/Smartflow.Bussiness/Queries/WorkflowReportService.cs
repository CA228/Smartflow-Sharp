using NHibernate;
using Smartflow.Bussiness.Interfaces;
using Smartflow.Bussiness.Models;
using Smartflow.Common;
using System.Collections.Generic;

namespace Smartflow.Bussiness.Queries
{
    public class WorkflowReportService : IWorkflowReportService
    {
        public IList<StatisticsInstance> GetStatisticsInstanceByUserId(string userId)
        {
            using ISession session = DbFactory.OpenSession();
            return session.GetNamedQuery("queryStatisticsInstanceByUserId")
                            .SetParameter("userId", userId)
                            .List<StatisticsInstance>();
        }
    }
}
