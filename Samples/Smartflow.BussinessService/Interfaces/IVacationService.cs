using Smartflow.BussinessService.Models;
using System;
using System.Collections.Generic;

namespace Smartflow.BussinessService.Interfaces
{
    public interface IVacationService
    {
        void Persist(Vacation model);

        Vacation GetVacationById(string id);

        IList<Vacation> GetPendingTaskList(IList<String> instanceIds);
    }
}
