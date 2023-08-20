using NHibernate;
using Smartflow.Abstraction.DTOs.Output;
using Smartflow.Common;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Core
{
    public interface IWorkflowTemplateService
    {
        void Persist(object template);

        void Persist(long templateId,object template);

        IList<WorkflowTemplate> GetWorkflowTemplateList(TemplateQueryOption queryOption,out int total);

        IList<WorkflowTemplate> GetWorkflowTemplateList();

        WorkflowTemplate GetWorkflowTemplateByCategoryId(int categoryId);

        IList<WorkflowTemplate> GetWorkflowTemplateListByCategoryId(int categoryId);

        WorkflowTemplate GetWorkflowTemplateById(long id);

        decimal GetWorkflowTemplateVersionByCategoryId(int categoryId);
    }
}
