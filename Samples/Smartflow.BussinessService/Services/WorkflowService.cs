using Refit;
using Smartflow.BussinessService.Interfaces;
using Smartflow.BussinessService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smartflow.BussinessService.Services
{
    public class WorkflowService : IWorkflowService
    {
        public async Task<IList<WorkflowTask>> GetUserTaskListByUserIdAsync(string userId)
        {
            IWorkflowService service = RestServiceExtensions.For<IWorkflowService>();
            return await service.GetUserTaskListByUserIdAsync(userId);
        }

        public async Task<WorkflowStartTask> StartAsync(Start info)
        {
            IWorkflowService service = RestServiceExtensions.For<IWorkflowService>();
            return await service.StartAsync(info);
        }

        public async Task SubmitAsync([Body(BodySerializationMethod.Serialized)] WorkflowContext context)
        {
            IWorkflowService service = RestServiceExtensions.For<IWorkflowService>();
            await service.SubmitAsync(context);
        }
    }
}
