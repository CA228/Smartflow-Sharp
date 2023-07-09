using Smartflow.Bussiness.Models;
using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartflow.API.DTOs
{
    /// <summary>
    /// 汇总
    /// </summary>
    public class WorkflowSummaryInfo
    {
        /// <summary>
        /// 字典
        /// </summary>
        public IDictionary<long, IList<Node>> Dict { get; set; }

        /// <summary>
        /// 统计实例清单
        /// </summary>
        public IList<StatisticsInstance> Instances { get; set; }
    }
}
