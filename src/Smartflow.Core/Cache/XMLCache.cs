using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Smartflow.Core.Elements;
using Smartflow.Core.Internals;

namespace Smartflow.Core.Cache
{
    internal class XMLCache : ICache
    {
        private readonly static IDictionary<long, IList<Node>> cache = new ConcurrentDictionary<long, IList<Node>>();
        private readonly static IWorkflowTemplateService templateService = WorkflowGlobalServiceProvider.Resolve<IWorkflowTemplateService>();

        static XMLCache()
        {
            IList<WorkflowTemplate> templates = templateService.GetWorkflowTemplateList();
            foreach (WorkflowTemplate template in templates)
            {
                if (!cache.ContainsKey(template.Id))
                {
                    Workflow workflow = XMLResolve.Parse(template.Source);
                    cache.TryAdd(template.Id, workflow.Nodes);
                }
            }
        }

        private static void Load(long templateId)
        {
            if (!cache.ContainsKey(templateId))
            {
                WorkflowTemplate template = templateService.GetWorkflowTemplateById(templateId);
                Workflow workflow = XMLResolve.Parse(template.Source);
                cache.TryAdd(templateId, workflow.Nodes);
            }
        }

        public void Publish(long templateId)
        {
            WorkflowTemplate template = templateService.GetWorkflowTemplateById(templateId);
            if (cache.ContainsKey(templateId))
            {
                Workflow workflow = XMLResolve.Parse(template.Source);
                cache[templateId] = workflow.Nodes;
            }
        }

        public Node GetNodeById(long templateId, string nodeId)
        {
            Load(templateId);
            IList<Node> nodes = cache[templateId];
            return nodes.Where(n => n.Id == nodeId).FirstOrDefault();
        }

        public IList<Node> GetNodesByTemplateId(long templateId)
        {
            Load(templateId);
            return cache.ContainsKey(templateId) ? cache[templateId] : null;
        }

        public IDictionary<long, IList<Node>> GetAllTemplateNodes()
        {
            return cache;
        }
    }
}
