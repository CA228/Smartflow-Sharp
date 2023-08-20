using NHibernate;
using Smartflow.Abstraction.DTOs.Output;
using Smartflow.Bussiness.Interfaces;
using Smartflow.Bussiness.Models;
using Smartflow.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Bussiness.Queries
{
    public class WorkflowReportService : IWorkflowReportService
    {
        public IList<StatisticsTask> GetPendingTaskList(TaskQueryOption queryOption, out int total)
        {
            using ISession session = DbFactory.OpenSession();
            var q= session.Query<StatisticsTask>();
            if (!string.IsNullOrEmpty(queryOption.CategoryId))
            {
                q=q.Where(e => e.CategoryId==int.Parse(queryOption.CategoryId));
            }
            total=((int)q.LongCount());
            int index = queryOption.Index - 1;
            return q.Skip(index * queryOption.Size).Take(queryOption.Size).ToList();
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
