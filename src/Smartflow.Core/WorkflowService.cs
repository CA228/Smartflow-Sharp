using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using Smartflow.Common;
using Smartflow.Core.Cache;
using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using Smartflow.Core.Handlers;

namespace Smartflow.Core
{
    public class WorkflowService : AbstractWorkflow, IWorkflowService
    {
        public WorkflowStartTask Start(WorkflowStart start)
        {
            string instanceId = Guid.NewGuid().ToString();
            WorkflowTemplate workflowTemplate = TemplateService.GetWorkflowTemplateByCategoryCode(start.CategoryCode);
            IList<Node> nodes = CacheFactory.Instance.GetNodesByTemplateId(workflowTemplate.Id);
            var startNode = nodes.Where(n => n.NodeType == WorkflowNodeCategory.Start).FirstOrDefault();
            ISet<Transition> transitions = startNode.Transitions;
            Transition transition = transitions.FirstOrDefault();

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

            TaskService.CreateTask(instance, nodes, startNode.Id, start.Props, transition.Id, start.Creator, start.Parallel,0, start.Children, start.Users, start.Roles);
            
            ChainFactory.Chain()
                .Add(new WorkflowRecordHandler(instance.Id, start.Creator, startNode, transition.Name, start.Message))
                .Add(new WorkflowRecordMailHandler(instance.Creator, start.Message))
                .Done();

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
