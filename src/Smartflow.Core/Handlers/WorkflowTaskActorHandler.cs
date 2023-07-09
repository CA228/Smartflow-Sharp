using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Core.Handlers
{
    public class WorkflowTaskActorHandler : IHandler
    {
        private readonly IWorkflowTaskAuthService authService = WorkflowGlobalServiceProvider.Resolve<IWorkflowTaskAuthService>();
        private readonly IWorkflowTaskAssignService assginService = WorkflowGlobalServiceProvider.Resolve<IWorkflowTaskAssignService>();

        private Node To { get; set; }

        private long TaskId { get; set; }

        private IList<string> Users { get; set; } = null;

        private IList<string> Roles { get; set; } = null;

        public WorkflowTaskActorHandler(long taskId, Node to, IList<string> users, IList<string> roles)
        {
            this.TaskId = taskId;
            this.To = to;
            this.Users = users;
            this.Roles = roles;
        }

        public void Done()
        {
            IList<WorkflowTaskAuth> auths = GetTaskAuths(To, TaskId, Users, Roles);

            assginService?.Assign(To, auths);

            if (auths.Count > 0)
            {
                authService.CreateBatchTaskAuth(auths);
            }
        }

        protected IList<WorkflowTaskAuth> GetTaskAuths(Node to, long taskId, IList<string> users, IList<string> roles)
        {
            IList<WorkflowTaskAuth> auths = new List<WorkflowTaskAuth>();

            if (to.Groups != null)
            {
                foreach (Group g in to.Groups)
                {
                    auths.Add(new WorkflowTaskAuth
                    {
                        AuthCode = g.Id,
                        TaskId = taskId,
                        Type = 0
                    });
                }
            }

            if (to.Actors != null)
            {
                foreach (Actor a in to.Actors)
                {
                    auths.Add(new WorkflowTaskAuth
                    {
                        AuthCode = a.Id,
                        TaskId = taskId,
                        Type = 1
                    });
                }
            }

            if (roles != null)
            {
                foreach (string role in roles)
                {
                    auths.Add(new WorkflowTaskAuth
                    {
                        AuthCode = role,
                        TaskId = taskId,
                        Type = 0
                    });
                }
            }

            if (users != null)
            {
                foreach (string user in users)
                {
                    auths.Add(new WorkflowTaskAuth
                    {
                        AuthCode = user,
                        TaskId = taskId,
                        Type = 1
                    });
                }
            }

            if (to.Organizations != null)
            {
                foreach (Organization o in to.Organizations)
                {
                    auths.Add(new WorkflowTaskAuth
                    {
                        AuthCode = o.Id,
                        TaskId = taskId,
                        Type = 2
                    });
                }
            }

            return auths;
        }
    }
}
