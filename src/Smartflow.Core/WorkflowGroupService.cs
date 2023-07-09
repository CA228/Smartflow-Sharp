using Smartflow.Core.Elements;
using System.Xml.Linq;

namespace Smartflow.Core
{
    public class WorkflowGroupService : IWorkflowParse
    {
        public Element Parse(XElement element)
        {
            return new Group
            {
                Name = element.Attribute("name").Value,
                Id = element.Attribute("id").Value
            };
        }
    }
}
