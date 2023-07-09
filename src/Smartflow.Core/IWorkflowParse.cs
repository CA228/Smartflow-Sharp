using System;
using System.Xml.Linq;

namespace Smartflow.Core
{
    public interface IWorkflowParse
    {
        Smartflow.Core.Elements.Element Parse(XElement element);
    }
}
