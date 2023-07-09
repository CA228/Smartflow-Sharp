using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smartflow.BussinessService.Interfaces;
using Smartflow.BussinessService.Models;
using Smartflow.Samples.Models;
using System.Linq;

namespace Smartflow.Samples.Controllers
{
    [ApiController]
    public class VacationController : ControllerBase
    {
        private readonly IVacationService _vacationService;
        private readonly IWorkflowService _workflowService;
        public VacationController(IWorkflowService workflowService, IVacationService vacationService)
        {
            _workflowService = workflowService;
            _vacationService = vacationService;
        }

        [Route("api/vacation/persist"), HttpPost]
        public async Task PersistAsync(VacationInput input)
        {
            Vacation vacation = new Vacation
            {
                CreateTime = DateTime.Now,
                NID = Guid.NewGuid().ToString(),
                VacationType = input.VacationType,
                Day = input.Day,
                Name = input.Name,
                UID = input.UID,
                Reason = input.Reason
            };
            WorkflowStartTask startTask = await _workflowService.StartAsync(new Start
            {
                BusinessId = vacation.NID,
                Creator = vacation.UID,
                CategoryCode = "001001",
                Message = "启动流程"
            });
            vacation.InstanceId = startTask.InstanceId;
            vacation.TemplateId = startTask.TemplateId;
            _vacationService.Persist(vacation);
        }

        [Route("api/vacation/{id}/info"), HttpGet]
        public Vacation Get(string id)
        {
            return _vacationService.GetVacationById(id);
        }

        [Route("api/vacation/{userId}/{id}/accept"), HttpPost]
        public async Task Accept(string userId, string id, RequestContent requestContent)
        {
            var info = _vacationService.GetVacationById(id);
            WorkflowContext workflowContext = new WorkflowContext
            {
                Id = requestContent.InstanceId,
                LineId = requestContent.LineId,
                TaskId = requestContent.TaskId,
                Message = requestContent.Message,
                Submiter = userId,
                Props = Newtonsoft.Json.JsonConvert.SerializeObject(info)
            };
            await _workflowService.SubmitAsync(workflowContext);
        }

        [Route("api/vacation/{userId}/{id}/reject"), HttpPost]
        public async Task Reject(string userId, string id, RequestContent requestContent)
        {
            var info = _vacationService.GetVacationById(id);
            WorkflowContext workflowContext = new WorkflowContext
            {
                Id = requestContent.InstanceId,
                LineId = requestContent.LineId,
                TaskId = requestContent.TaskId,
                Message = requestContent.Message,
                Submiter = userId,
                Props = Newtonsoft.Json.JsonConvert.SerializeObject(info)
            };
            await _workflowService.SubmitAsync(workflowContext);
        }

        [Route("api/vacation/{userId}/{id}/cancel"), HttpPost]
        public async Task Cancel(string userId, string id, RequestContent requestContent)
        {
            var info = _vacationService.GetVacationById(id);
            WorkflowContext workflowContext = new WorkflowContext
            {
                Id = requestContent.InstanceId,
                LineId = requestContent.LineId,
                TaskId = requestContent.TaskId,
                Message = requestContent.Message,
                Submiter = userId,
                Props = Newtonsoft.Json.JsonConvert.SerializeObject(info)
            };
            await _workflowService.SubmitAsync(workflowContext);
        }

        [Route("api/pending/{userId}/vacation/list"), HttpGet]
        public async Task<IList<Pending<Vacation>>> GetPendingTaskList(string userId)
        {
            IList<WorkflowTask> workflowTasks = await _workflowService.GetUserTaskListByUserIdAsync(userId);
            IList<string> instanceIds = workflowTasks.GroupBy(e => e.InstanceId).Select(c => c.Key).ToList();
            IList<Pending<Vacation>> tasks = new List<Pending<Vacation>>();
            IList<Vacation> vacations = _vacationService.GetPendingTaskList(instanceIds);
            foreach (WorkflowTask workflowTask in workflowTasks)
            {
                Vacation record = vacations.Where(c => c.InstanceId.Equals(workflowTask.InstanceId)).FirstOrDefault();
                if (record != null)
                {
                    tasks.Add(new Pending<Vacation>()
                    {
                        Task = workflowTask,
                        Data = record
                    });
                }
            }
            return tasks;
        }
    }
}
