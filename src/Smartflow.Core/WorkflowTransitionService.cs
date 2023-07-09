using Smartflow.Core.Elements;
using System.Linq;
using System.Xml.Linq;
using System;

namespace Smartflow.Core
{
    public class WorkflowTransitionService : IWorkflowTransitionService, IWorkflowParse
    {
        public Element Parse(XElement element)
        {
            Transition entry = new Transition
            {
                Name = element.Attribute("name").Value,
                Destination = element.Attribute("destination").Value,
                Id = element.Attribute("id").Value
            };

            if (element.HasElements)
            {
                XElement expression = element.Elements("expression").FirstOrDefault();
                if (expression != null)
                {
                    entry.Expression = expression.Value;
                }
            }

            XAttribute url = element.Attribute("url");
            entry.Url = url == null ? string.Empty : url.Value;
            return entry;
        }
    }
}
