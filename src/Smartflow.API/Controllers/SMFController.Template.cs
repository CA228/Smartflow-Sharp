using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartflow.Core;
using Smartflow.API.Code;
using Smartflow.API.Input;
using System.Linq;

namespace Smartflow.API.Controllers
{
    /// <summary>
    /// 流程模板操作API
    /// </summary>
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly WorkflowTemplateService _workflowTemplateService;
        public TemplateController(WorkflowTemplateService workflowTemplateService)
        {
            _workflowTemplateService = workflowTemplateService;
        }

        /// <summary>
        /// 获取流程模板清单
        /// </summary>
        /// <returns></returns>
        [Route("api/wf/template/list"), HttpPost]
        public ResultData GetWorkflowTemplateList()
        {
            IList<WorkflowTemplate> templates = _workflowTemplateService.GetWorkflowTemplateList();
            return CommonMethods.Response(templates, templates.Count);
        }

        /// <summary>
        /// 通过流程模块Id获取流程模板信息
        /// </summary>
        /// <param name="id">流程Id</param>
        /// <returns>流程模块</returns>
        [Route("api/wf/template/{id}/info"), HttpGet]
        public WorkflowTemplate GetWorkflowTemplateById(long id)
        {
            return _workflowTemplateService.GetWorkflowTemplateById(id);
        }

        /// <summary>
        /// 保存/编辑流程模板
        /// </summary>
        /// <param name="body">流程模板实体</param>
        [Route("api/wf/template/persist"), HttpPost]
        public void Persist(WorkflowTemplateInput body)
        {
            WorkflowTemplate template;
            if (body.Id != 0)
            {
                template = _workflowTemplateService.GetWorkflowTemplateById(body.Id);
            }
            else
            {
                decimal version = _workflowTemplateService.GetWorkflowTemplateVersionByCategoryCode(body.CategoryCode);
                template = new WorkflowTemplate
                {
                    CreateTime = DateTime.Now,
                    Version = version + 0.1m,
                    Status = 0,
                    CategoryCode = body.CategoryCode,
                    CategoryName = body.CategoryName
                };
            }
            template.UpdateTime = DateTime.Now;
            template.Name = body.Name;
            template.Memo = body.Memo;
            template.Source = Uri.UnescapeDataString(body.Source);
            _workflowTemplateService.Persist(template.Id, template);
        }

        /// <summary>
        /// 流程模板另存为
        /// </summary>
        /// <param name="body">流程模板实体</param>
        [Route("api/wf/template/saveas"), HttpPost]
        public void SaveAs(WorkflowTemplateInput body)
        {
            WorkflowTemplate template = new WorkflowTemplate
            {
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            decimal version = _workflowTemplateService.GetWorkflowTemplateVersionByCategoryCode(body.CategoryCode);
            template.Name = body.Name;
            template.Version = (version + 0.1m);
            template.CategoryCode = body.CategoryCode;
            template.CategoryName = body.CategoryName;
            template.Status = 0;
            template.Memo = body.Memo;
            template.Source = Uri.UnescapeDataString(body.Source);
            _workflowTemplateService.Persist(template);
        }

        /// <summary>
        /// 通过流程Id和状态，修改流程模板使用状态
        /// </summary>
        /// <param name="id">流程模板Id</param>
        /// <param name="status">状态</param>
        [Route("api/wf/template/{id}/change/{status}"), HttpPost]
        public void ChangeStatus(long id, int status)
        {
            WorkflowTemplate template = _workflowTemplateService.GetWorkflowTemplateById(id);
            template.Status = status;
            template.UpdateTime = DateTime.Now;
            _workflowTemplateService.Persist(template.Id, template);

            if (template.Status == 1)
            {
                IList<WorkflowTemplate> wfList = _workflowTemplateService.GetWorkflowTemplateListByCategoryCode(template.CategoryCode);
                foreach (WorkflowTemplate entry in wfList.Where(e => e.Id != template.Id && e.Status == 1).ToList())
                {
                    entry.Status = 0;
                    entry.UpdateTime = DateTime.Now;
                    _workflowTemplateService.Persist(entry.Id, entry);
                }
            }
        }
    }
}