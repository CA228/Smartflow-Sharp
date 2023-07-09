using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core
{
    public interface IWorkflowTaskAssignService
    {
        void Assign(Node node, IList<WorkflowTaskAuth> auth);
    }
}
