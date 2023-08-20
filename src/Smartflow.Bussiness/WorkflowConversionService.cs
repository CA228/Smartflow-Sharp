using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Smartflow.Bussiness.Models;
using Smartflow.Common;
using Smartflow.Core;

namespace Smartflow.Bussiness
{
    public class WorkflowConversionService : IWorkflowConversion
    {
        public IList<string> GetActorList(IList<WorkflowTaskActor> actors)
        {
            List<string> all = new List<string>();
            IList<string> roleGroupId = actors.Where(s => s.Type == 0).Select(c => c.Id).ToList();
            IList<string> organizationGroupId = actors.Where(s => s.Type == 2).Select(c => c.Id).ToList();
            using ISession session = DbFactory.OpenBussinessSession();
            IList<string> roleMailGroup = new List<string>();
            if (roleGroupId.Count > 0)
            {
                roleMailGroup = session.GetNamedQuery("queryMailByRoleIds")
                                                 .SetParameterList("rIds", roleGroupId)
                                                 .List<User>()
                                                 .Select(c => c.Id)
                                                 .ToList();
            }
            IList<string> organizationMailGroup = new List<string>();
            if (roleGroupId.Count > 0)
            {
                organizationMailGroup = session.GetNamedQuery("queryMailByOrganizationCodes")
                                                           .SetParameterList("organizationCodes", organizationGroupId)
                                                           .List<User>()
                                                           .Select(c => c.Id)
                                                           .ToList();
            }

            if (roleMailGroup.Count > 0)
            {
                all.AddRange(roleMailGroup);
            }
            if (organizationMailGroup.Count > 0)
            {
                all.AddRange(organizationMailGroup);
            }

            return all.Distinct().ToList();
        }

        public string GetCategoryName(int categoryId)
        {
            using ISession session = DbFactory.OpenSession();
            Category category = session.Query<Category>()
                                      .Where(c => c.Id.Equals(categoryId))
                                      .FirstOrDefault();
            return category?.Name;
        }

        public IList<string> GetReceiveAddress(IList<WorkflowTaskAuth> auths)
        {
            List<string> all = new List<string>();
            IList<string> userGroupId = auths.Where(a => a.Type == 1).Select(c => c.AuthCode).ToList();
            IList<string> roleGroupId = auths.Where(s => s.Type == 0).Select(c => c.AuthCode).ToList();
            IList<string> organizationGroupId = auths.Where(s => s.Type == 2).Select(c => c.AuthCode).ToList();

            using ISession session = DbFactory.OpenBussinessSession();
            IList<string> userMailGroup = this.GetReceiveAddress(userGroupId);

            IList<string> roleMailGroup = new List<string>();
            if (roleGroupId.Count > 0)
            {
                roleMailGroup = session.GetNamedQuery("queryMailByRoleIds")
                                                 .SetParameterList("rIds", roleGroupId)
                                                 .List<User>()
                                                 .Select(c => c.Mail)
                                                 .ToList();
            }
            IList<string> organizationMailGroup = new List<string>();
            if (roleGroupId.Count > 0)
            {
                organizationMailGroup = session.GetNamedQuery("queryMailByOrganizationCodes")
                                                           .SetParameterList("organizationCodes", organizationGroupId)
                                                           .List<User>()
                                                           .Select(c => c.Mail)
                                                           .ToList();
            }

            if (userMailGroup != null && userMailGroup.Count > 0)
            {
                all.AddRange(userMailGroup);
            }
            if (roleMailGroup.Count > 0)
            {
                all.AddRange(roleMailGroup);
            }
            if (organizationMailGroup.Count > 0)
            {
                all.AddRange(organizationMailGroup);
            }
            return all.Distinct().ToList();
        }

        public IList<string> GetReceiveAddress(IList<string> users)
        {
            if (users.Count > 0)
            {
                using ISession session = DbFactory.OpenSession();
                IList<User> ul = session.CreateCriteria<User>()
                                       .Add(Restrictions.IsNotNull("Mail"))
                                       .Add(Restrictions.In("Id", users.ToArray()))
                                       .List<User>();

                return ul.Select(e => e.Mail).ToArray();
            }
            return null;
        }
    }
}

