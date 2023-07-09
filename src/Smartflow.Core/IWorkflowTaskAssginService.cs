using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core
{
    public interface IWorkflowTaskAssginService
    {
        void Assgin(Node node, IList<WorkflowTaskAuth> auth);
    }
}
