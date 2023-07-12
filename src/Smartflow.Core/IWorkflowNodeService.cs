using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;

namespace Smartflow.Core
{
    public interface IWorkflowNodeService :  IWorkflowParse
    {
        Transition GetTransition(string props,Node el);

        IDictionary<long, IList<Node>> GetAllTemplateNodes();

        IList<Transition> GetPreviousTransitions(IList<Node> nodes, Node el);
    }
}
