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
        public IList<Organization> Query(String id)
        {
            using ISession session = DbFactory.OpenBussinessSession();
            return session
                       .Query<Organization>()
                       .Where(e=>e.ParentId==id)
                       .ToList();
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
