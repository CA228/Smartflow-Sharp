using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;

namespace Smartflow.Core
{
    public interface IWorkflowNodeService :  IWorkflowParse
    {
        Transition GetTransition(string json,Node n);

        IDictionary<long, IList<Node>> GetAllTemplateNodes();
    }
}
