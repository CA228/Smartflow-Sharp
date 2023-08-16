using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using Smartflow.Bussiness.Models;
using Smartflow.Common;
using Smartflow.Core;

namespace Smartflow.Bussiness
{
    public class WorkflowBridgeService : AbstractBridgeService
    {
        public List<Role> GetRoles(string searchKey)
        {
            using ISession session = DbFactory.OpenBussinessSession();
            IQueryable<Role> query = session.Query<Role>();
            return string.IsNullOrEmpty(searchKey)?query.ToList():
                   query.Where(u => u.Name.Contains(searchKey)).ToList();
        }

        public IList<User> GetUsers(string searchKey)
        {
            using ISession session = DbFactory.OpenBussinessSession();
            return session.GetNamedQuery("queryUserBySearchKey")
                .SetParameter("searchKey", String.Format("%{0}%", searchKey)).List<User>();
        }
    }
}

