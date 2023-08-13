using System;
using System.Collections.Generic;

namespace Smartflow.Core
{
    public interface IWorkflowConversion
    {
        IList<string> GetReceiveAddress(IList<WorkflowTaskAuth> auths);

        IList<string> GetActorList(IList<WorkflowTaskActor> actors);

        IList<string> GetReceiveAddress(IList<string> users);

        string GetCategoryName(string categoryCode);
    }
}
