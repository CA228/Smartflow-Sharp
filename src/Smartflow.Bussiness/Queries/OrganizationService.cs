using System;
using System.Collections.Generic;
using System.Linq;
using Smartflow.Bussiness.Interfaces;
using NHibernate;
using Smartflow.Bussiness.Models;
using Smartflow.Common;

namespace Smartflow.Bussiness.Queries
{
    public class OrganizationService : IOrganizationService
    {
        public IList<Organization> Query(string id)
        {
            using ISession session = DbFactory.OpenBussinessSession();
           return session
                       .Query<Organization>()
                       .Where(e => e.ParentId == id).ToList();
        }

        public IList<Organization> Query(string id, string searchKey)
        {
            using ISession session = DbFactory.OpenBussinessSession();
            IQueryable<Organization> query= session.Query<Organization>();
            return string.IsNullOrEmpty(searchKey) ? query.ToList() :
                   query.Where(u => u.Name.Contains(searchKey)).ToList();
        }

        public void Load(string id, IList<Organization> all)
        {
            IList<Organization> orgs = this.Query(id);
            foreach (Organization org in orgs)
            {
                Load(org.Id, all);
                all.Add(org);
            }
        }
    }
}
