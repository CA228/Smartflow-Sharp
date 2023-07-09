using Smartflow.Bussiness.Models;
using Smartflow.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Bussiness.Interfaces
{
    public interface IOrganizationService
    {
        IList<Organization> Query(string id);

        void Load(string id, IList<Organization> all);
    }
}
