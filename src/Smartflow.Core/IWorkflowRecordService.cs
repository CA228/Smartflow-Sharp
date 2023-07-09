using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Smartflow.Core.Elements;

namespace Smartflow.Core
{
    public interface IWorkflowRecordService
    {
        void Persist(object record);

        IList<WorkflowProcessRecord> GetRecordListByInstanceId(string instanceId);
    }
}
