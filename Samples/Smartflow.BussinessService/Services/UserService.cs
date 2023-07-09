using Smartflow.BussinessService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow.BussinessService.Services
{
    public class UserService
    {
        public User Get(string id)
        {
            string executeSql = @"SELECT T.*,[dbo].[F_GetRoleGroup](T.ID) RoleGroup,[dbo].[F_GetOrganizationName](T.OrganizationCode) OrganizationName FROM T_SYS_USER T WHERE UID=@UID";
            return DBUtils.CreateConnection().Query<User>(executeSql, new { UID = id }).FirstOrDefault();
        }

        public IList<User> GetUserList()
        {
            string executeSql = @"SELECT T.*,[dbo].[F_GetRoleGroup](T.ID) RoleGroup,[dbo].[F_GetOrganizationName](T.OrganizationCode) OrganizationName FROM T_SYS_USER T";
            return DBUtils.CreateConnection().Query<User>(executeSql).ToList();
        }
    }
}
