using System;
using System.Xml.Linq;
using Smartflow.Core.Elements;

namespace Smartflow.Core
{
    public class WorkflowActorService : IWorkflowParse
    {
        public Element Parse(XElement element)
        {
            return new Actor
            {
                Name = element.Attribute("name").Value,
                Id = element.Attribute("id").Value
            };
        }
    }
}
