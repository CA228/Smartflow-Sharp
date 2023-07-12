using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartflow.API.DTOs;
using Smartflow.Bussiness.Interfaces;
using Smartflow.Bussiness.Models;
using Smartflow.Core;
using Smartflow.Core.Elements;

namespace Smartflow.API.Controllers
{
    /// <summary>
    /// 工作流核心API
    /// </summary>
    [ApiController]
    public class SMFController : ControllerBase
    {
        private readonly IWorkflowReportService _workflowReportService;
        public SMFController(IWorkflowReportService workflowReportService)
        {
            _workflowReportService = workflowReportService;
        }

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="start">启动参数</param>
        /// <returns>任务</returns>
        [Route("api/smf/start"), HttpPost]
        public WorkflowStartTask Start(WorkflowStartInfo start)
        {
            return WorkflowEngine.Instance.Start(start);
        }

        /// <summary>
        /// 根据用户获取任务列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>任务清单</returns>
        [Route("api/smf/task/{userId}/list"), HttpGet]
        public IList<WorkflowTask> GetUserTaskListByUserId(string userId)
        {
            return WorkflowEngine.Instance.AbstractCoreService.TaskService.GetUserTaskListByUserId(userId);
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="context">提交实体</param>
        [Route("api/smf/submit"), HttpPost]
        public void Submit(WorkflowContext context)
        {
            WorkflowEngine.Instance.Submit(context);
        }

        /// <summary>
        /// 根据用户统计任务汇总
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>汇总</returns>
        [Route("api/smf/report/{userId}/list"), HttpGet]
        public WorkflowSummaryInfo GetReportByUserId(string userId)
        {
            IDictionary<long, IList<Node>> nodeMap = WorkflowEngine.Instance.AbstractCoreService.NodeService.GetAllTemplateNodes();
            IList<StatisticsInstance> statisticsInstances = _workflowReportService.GetStatisticsInstanceByUserId(userId);
            return new WorkflowSummaryInfo
            {
                Dict = nodeMap,
                Instances = statisticsInstances
            };
        }
    }
}