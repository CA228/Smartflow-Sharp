using Smartflow.Abstraction.DTOs.Output;
using Smartflow.Bussiness.Models;
using Smartflow.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Bussiness.Interfaces
{
    public interface IWorkflowReportService
    {
        IList<StatisticsInstance> GetStatisticsInstanceByUserId(string userId);
        

        IList<StatisticsTask> GetPendingTaskList(TaskQueryOption queryOption, out int total);
    }
}
