using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Smartflow.BussinessService.Models;
using Dapper;
using Smartflow.BussinessService.Interfaces;

namespace Smartflow.BussinessService.Services
{
    public class VacationService: IVacationService
    {
        private readonly string SQL_COMMAND_INSERT = @"INSERT INTO T_VACATION([NID],[UID],[Name],[Day],[Reason],[CreateTime],[VacationType],[InstanceID],[TemplateId]) VALUES(@NID,@UID,@Name,@Day,@Reason,@CreateTime,@VacationType,@InstanceID,@TemplateId)";
        private readonly string SQL_COMMAND_SELECT = @"SELECT * FROM T_VACATION WHERE 1=1  AND NID=@NID  ";
        private readonly string SQL_COMMAND_SELECT_PENDING = @"SELECT * FROM T_VACATION WHERE InstanceId IN @InstanceIds";

        public void Persist(Vacation model)
        {
            DBUtils.CreateConnection().Execute(SQL_COMMAND_INSERT, model);
        }
   
        public Vacation GetVacationById(string id)
        {
            return DBUtils.CreateConnection().Query<Vacation>(SQL_COMMAND_SELECT, new { NID = id }).FirstOrDefault();
        }

        public IList<Vacation> GetPendingTaskList(IList<String> instanceIds)
        {
            return DBUtils.CreateConnection().Query<Vacation>(SQL_COMMAND_SELECT_PENDING,new { InstanceIds = instanceIds }).ToList();
        }
    }
}
