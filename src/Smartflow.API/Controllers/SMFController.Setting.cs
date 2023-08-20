using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Smartflow.Bussiness.Interfaces;
using Smartflow.Bussiness.Models;
using Smartflow.Common;
using Smartflow.Core;
using Smartflow.Bussiness;

namespace Smartflow.API.Controllers
{
    /// <summary>
    /// 配置信息API
    /// </summary>
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly WorkflowBridgeService _workflowBridgeService;
        private readonly IOrganizationService _organizationService;
        private readonly IQuery<IList<Category>> _categoryService;
        public SettingController(WorkflowBridgeService workflowBridgeService, IOrganizationService organizationService, IQuery<IList<Category>> categoryService)
        {
            _workflowBridgeService = workflowBridgeService;
            _organizationService = organizationService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// 获取角色清单
        /// </summary>
        /// <returns>角色列表</returns>
        [Route("api/setting/group/list"), HttpGet]
        public IEnumerable<Role> GetRoles(string searchKey)
        {
            return _workflowBridgeService.GetRoles(searchKey);
        }

        /// <summary>
        /// 获取参与者清单
        /// </summary>
        /// <returns>参与者列表</returns>
        [Route("api/setting/user/list"), HttpGet]
        public IEnumerable<User> GetUsers(string searchKey)
        {
            return _workflowBridgeService.GetUsers(searchKey);
        }

        /// <summary>
        /// 通过实例Id，获取过程记录
        /// </summary>
        /// <param name="instanceId">实例Id</param>
        /// <returns>过程记录列表</returns>
        [Route("api/setting/record/{instanceId}/list"), HttpGet]
        public IEnumerable<WorkflowProcessRecord> GetRecordByInstanceId(string instanceId)
        {
            return WorkflowEngine.Instance.AbstractCoreService.RecordService
                .GetRecordListByInstanceId(instanceId);
        }

        /// <summary>
        /// 获取单位清单
        /// </summary>
        /// <returns>单位列表</returns>
        [Route("api/setting/organization/list"), HttpGet]
        public IEnumerable<Bussiness.Models.Organization> GetOrganization(string searchKey)
        {
            return _organizationService.Query("0", searchKey);
        }

        /// <summary>
        /// 获取类别清单
        /// </summary>
        /// <returns>类别列表</returns>
        [Route("api/setting/category/list"), HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return _categoryService.Query();
        }

        /// <summary>
        /// 通过类别代码，获取类别实体
        /// </summary>
        /// <param name="id">类别代码</param>
        /// <returns>类别信息</returns>
        [Route("api/setting/category/{id}/info"), HttpGet]
        public Category GetCategory(int id)
        {
            return _categoryService.Query().FirstOrDefault(cate => cate.Id == id);
        }
    }
}