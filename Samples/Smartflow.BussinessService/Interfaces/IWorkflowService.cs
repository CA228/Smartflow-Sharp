using Refit;
using Smartflow.BussinessService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smartflow.BussinessService.Interfaces
{
    public interface IWorkflowService
    {
        [Post("/api/smf/start")]
        Task<WorkflowStartTask> StartAsync([Body(BodySerializationMethod.Serialized)]Start process);

        [Get("/api/smf/task/{userId}/list")]
        Task<IList<WorkflowTask>> GetUserTaskListByUserIdAsync(string userId);

        [Post("/api/smf/submit")]
        Task SubmitAsync([Body(BodySerializationMethod.Serialized)] WorkflowContext context);
    }
}
