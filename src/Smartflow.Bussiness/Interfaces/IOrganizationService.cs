using Smartflow.Bussiness.Models;
using System.Collections.Generic;


namespace Smartflow.Bussiness.Interfaces
{
    public interface IOrganizationService
    {
        IList<Organization> Query(string id);

        IList<Organization> Query(string id, string searchKey);

        void Load(string id, IList<Organization> all);
    }
}
