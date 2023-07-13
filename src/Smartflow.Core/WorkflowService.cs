﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using Smartflow.Common;
using Smartflow.Core.Cache;
using Smartflow.Core.Dispatch;
using Smartflow.Core.Elements;

namespace Smartflow.Core
{
    public class WorkflowService : AbstractWorkflow, IWorkflowService
    {
        public WorkflowStartTask Start(WorkflowStartInfo start)
        {
            string instanceId = Guid.NewGuid().ToString();
            WorkflowTemplate workflowTemplate = TemplateService.GetWorkflowTemplateByCategoryCode(start.CategoryCode);
            IList<Node> nodes = CacheFactory.Instance.GetNodesByTemplateId(workflowTemplate.Id);
            var startNode = nodes.Where(n => n.NodeType == WorkflowNodeCategory.Start).FirstOrDefault();

            WorkflowInstance instance = new WorkflowInstance
            {
                BusinessId = start.BusinessId,
                CategoryCode = start.CategoryCode,
                CreateTime = DateTime.Now,
                Creator = start.Creator,
                Id = instanceId,
                TemplateId = workflowTemplate.Id
            };

            CreateInstance(instance);
            
            DispatchTaskStrategy.CreatetDispatchTaskStrategy(new StartDispatch(instance, startNode, start)).Dispatch();
            
            return new WorkflowStartTask
            {
                InstanceId = instance.Id,
                Code = startNode.Id,
                TemplateId = workflowTemplate.Id
            };
        }

        protected void CreateInstance(WorkflowInstance instance)
        {
            using ISession session = DbFactory.OpenSession();
            session.Save(instance);
            session.Flush();
        }
    }
}
