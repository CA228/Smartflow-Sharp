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
        public List<Role> GetRoles()
        {
            using ISession session = DbFactory.OpenBussinessSession();
            return session
                    .Query<Role>().ToList();
        }
    }
}

