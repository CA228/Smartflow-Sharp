using NHibernate;
using Smartflow.Abstraction.DTOs.Output;
using Smartflow.Common;
using Smartflow.Core.Cache;
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

        public IList<WorkflowTemplate> GetWorkflowTemplateList(TemplateQueryOption queryOption,out int total)
        {
            using ISession session = DbFactory.OpenSession();
            var q = session.Query<WorkflowTemplate>();
            if (!string.IsNullOrEmpty(queryOption.CategoryId))
            {
                q=q.Where(e => e.CategoryId == int.Parse(queryOption.CategoryId));
            }
            total = ((int)q.LongCount());

            int index = queryOption.Index - 1;
            return q.Skip(index * queryOption.Size).Take(queryOption.Size)
                        .OrderBy(e => e.CategoryId)
                       .OrderByDescending(e => e.Version).ToList();
        }

        public IList<WorkflowTemplate> GetWorkflowTemplateList()
        {
            using ISession session = DbFactory.OpenSession();
            return session.Query<WorkflowTemplate>()
                           .OrderBy(e => e.CategoryId)
                           .OrderByDescending(e => e.Version).ToList();
        }

        public WorkflowTemplate GetWorkflowTemplateByCategoryId(int categoryId)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                       .Query<WorkflowTemplate>()
                       .Where(e => e.CategoryId == categoryId && e.Status==1)
                       .OrderByDescending(e => e.Version)
                       .FirstOrDefault();
        }

        public IList<WorkflowTemplate> GetWorkflowTemplateListByCategoryId(int categoryId)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                       .Query<WorkflowTemplate>()
                       .Where(e => e.CategoryId == categoryId)
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

        public decimal GetWorkflowTemplateVersionByCategoryId(int categoryId)
        {
            using ISession session = DbFactory.OpenSession();
            string hql = "select max(t.Version) from WorkflowTemplate as t where t.CategoryId=:CategoryId ";
            IQuery query =session.CreateQuery(hql).SetParameter("CategoryId", categoryId, NHibernateUtil.Int32);
            return query.FutureValue<decimal>().Value;
        }
    }
}
