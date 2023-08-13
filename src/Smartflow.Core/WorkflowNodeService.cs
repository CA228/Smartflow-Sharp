using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using Smartflow.Core.Internals;
using System.Xml.Linq;
using System.Data;
using Smartflow.Core.Cache;

namespace Smartflow.Core
{
    public class WorkflowNodeService : IWorkflowNodeService, IWorkflowParse
    {
        public Element Parse(XElement element)
        {
            Node node = new Node
            {
                Name = element.Attribute("name").Value,
                Id = element.Attribute("id").Value
            };
            string category = element.Attribute("category").Value;
            node.NodeType = Internals.Utils.Convert(category);
            node.Collaboration = Internals.Utils.CheckAttributes(element, "collaboration") ? Convert.ToInt32(element.Attribute("collaboration").Value) :0;
            if (element.HasElements)
            {
                List<Element> nodes = new List<Element>();
                element.Elements().ToList().ForEach(entry =>
                {
                    string nodeName = entry.Name.LocalName;
                    if (ServiceContainer.Contains(nodeName))
                    {
                        IWorkflowParse parseService = (ServiceContainer.Resolve(nodeName) as IWorkflowParse);
                        nodes.Add(parseService.Parse(entry));
                    }
                });
                node.Transitions.AddRange(nodes.Where(transition => (transition is Transition)).Cast<Transition>().OrderBy(e => e.Order));
                node.Groups.AddRange(nodes.Where(group => (group is Group)).Cast<Group>());
                node.Actors.AddRange(nodes.Where(actor => (actor is Actor)).Cast<Actor>());
                node.Organizations.AddRange(nodes.Where(org => (org is Elements.Organization)).Cast<Elements.Organization>());
            }
            return node;
        }

        public Transition GetTransition(string props, Node el)
        {
            try
            {
                DataTable resultSet = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonToDataTableCharacter(props));
                Transition resultSelectTransition = null;
                List<Transition> transitions = el.Transitions.ToList();
                if (resultSet.Rows.Count > 0)
                {
                    foreach (Transition transition in transitions)
                    {
                        if (!string.IsNullOrEmpty(transition.Expression) && resultSet.Select(transition.Expression).Length > 0)
                        {
                            resultSelectTransition = transition;
                            break;
                        }
                    }
                }
                resultSet.Dispose();
                return resultSelectTransition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string JsonToDataTableCharacter(string text)
        {
            return text.StartsWith("[") ? text : string.Format("[{0}]", text);
        }

        public IDictionary<long, IList<Node>> GetAllTemplateNodes()
        {
            return CacheFactory.Instance.GetAllTemplateNodes();
        }

        public IList<Transition> GetPreviousTransitions(IList<Node> nodes,Node el)
        {
            IList<Transition> transitions = new List<Transition>();
            nodes.ToList().ForEach(tr => transitions.AddRange(tr.Transitions));
            return transitions.Where(t => t.Destination == el.Id).ToList();
        }
    }
}