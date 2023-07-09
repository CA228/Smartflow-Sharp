using NHibernate;
using Smartflow.Common;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Core
{
    public interface IWorkflowTemplateService
    {
        void Persist(object template);

        void Persist(long templateId,object template);

        IList<WorkflowTemplate> GetWorkflowTemplateList();

        WorkflowTemplate GetWorkflowTemplateByCategoryCode(string categoryCode);

        IList<WorkflowTemplate> GetWorkflowTemplateListByCategoryCode(string categoryCode);

        WorkflowTemplate GetWorkflowTemplateById(long id);

        decimal GetWorkflowTemplateVersionByCategoryCode(string categoryCode);

     
    }
}
