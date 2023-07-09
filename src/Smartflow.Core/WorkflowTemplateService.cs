using NHibernate;
using Smartflow.Common;
using Smartflow.Core.Cache;
using Smartflow.Core.Elements;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Core
{
    public class WorkflowTemplateService: IWorkflowTemplateService
    {
        public void Persist(object template)
        {
            using ISession session = DbFactory.OpenSession();
            session.SaveOrUpdate(template);
            session.Flush();
        }

        public void Persist(long templateId,object template)
        {
            this.Persist(template);
            CacheFactory.Instance.Publish(templateId);
        }

        public IList<WorkflowTemplate> GetWorkflowTemplateList()
        {
            using ISession session = DbFactory.OpenSession();
            return session
                       .Query<WorkflowTemplate>()
                       .OrderBy(e=>e.CategoryCode)
                       .OrderByDescending(e => e.Version)
                       .ToList();
        }

        public WorkflowTemplate GetWorkflowTemplateByCategoryCode(string categoryCode)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                       .Query<WorkflowTemplate>()
                       .Where(e => e.CategoryCode == categoryCode&&e.Status==1)
                       .OrderByDescending(e => e.Version)
                       .FirstOrDefault();
        }

        public IList<WorkflowTemplate> GetWorkflowTemplateListByCategoryCode(string categoryCode)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                       .Query<WorkflowTemplate>()
                       .Where(e => e.CategoryCode == categoryCode)
                       .OrderByDescending(e => e.Version)
                       .ToList();
        }

        public WorkflowTemplate GetWorkflowTemplateById(long id)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                       .Query<WorkflowTemplate>()
                       .Where(e => e.Id == id)
                       .FirstOrDefault();
        }

        public decimal GetWorkflowTemplateVersionByCategoryCode(string categoryCode)
        {
            using ISession session = DbFactory.OpenSession();
            string hql = "select max(t.Version) from WorkflowTemplate as t where t.CategoryCode=:CategoryCode and t.Status=1";
            IQuery query =session.CreateQuery(hql).SetParameter("CategoryCode", categoryCode, NHibernateUtil.String);
            return query.FutureValue<decimal>().Value;
        }
    }
}
