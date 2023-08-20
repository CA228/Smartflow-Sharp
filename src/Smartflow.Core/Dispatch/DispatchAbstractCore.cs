using Smartflow.Core.Cache;
using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using Smartflow.Core.Handler;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Core.Dispatch
{
    public abstract class DispatchAbstractCore : AbstractWorkflow
    {
        protected WorkflowInstance Instance { get;}

        protected IList<Node> Nodes { get;  }

        public DispatchAbstractCore(WorkflowInstance instance)
        {
            this.Instance = instance;
            this.Nodes= CacheFactory.Instance.GetNodesByTemplateId(Instance.TemplateId);
        }

        public WorkflowTask CreateTask(Node to,string lineCode,string publisher, bool parallel, long parentId, IList<WorkflowSubprocess> children = null, IList<string> users = null, IList<string> roles = null)
        {
            int close = to.NodeType == WorkflowNodeCategory.End || to.NodeType == WorkflowNodeCategory.Decision||to.NodeType == WorkflowNodeCategory.Fork ? 1 : 0;
            
            WorkflowTask task = TaskService.CreateTask(to, lineCode, Instance.Id, publisher, parentId, false, close);
           
            if (children != null)
            {
                foreach (WorkflowSubprocess subprocess in children)
                {
                    this.CreateSubprocess(task.Id, lineCode, publisher, subprocess, parallel);
                }
            }
            ChainFactory.Chain()
                .Add(new WorkflowTaskActorHandler(task.Id, to, users, roles))
                .Add(new WorkflowTaskMailHandler(Instance.CategoryId, task.Id))
                .Done();

            return task;
        }

        protected void CreateSubprocess(long taskId,string lineCode, string publisher, WorkflowSubprocess subprocess, bool parallel)
        {
            Node node = CacheFactory.Instance.GetNodeById(subprocess.TemplateId, subprocess.Code);
            WorkflowTask afterTask = TaskService.CreateTask(node, lineCode, Instance.Id, publisher, taskId, parallel, 0);
            ChainFactory.Chain()
                            .Add(new WorkflowTaskActorHandler(afterTask.Id, node, subprocess.Users, subprocess.Roles))
                            .Add(new WorkflowTaskMailHandler(Instance.CategoryId, afterTask.Id))
                            .Done();
        }

        public void Dispatch(Transition transition,string publisher, string props,long taskId, bool parallel, IList<WorkflowSubprocess> children = null, IList<string> users = null, IList<string> roles = null)
        {
            Node destination = Nodes.Where(c => c.Id == transition.Destination).FirstOrDefault();
            if (destination.NodeType == WorkflowNodeCategory.Collaboration)
            {
                CollaborationTask.CreateInstance(Instance, destination, transition.Id, taskId, publisher, users, roles).Dispatch();
            }
            else
            {
                WorkflowTask afterTask = this.CreateTask(destination, transition.Id, publisher, parallel, taskId,children,users,roles);
                this.DispatchBranchTask(props, destination, afterTask);
            }
        }

        public void DispatchBranchTask(string props,Node destination,WorkflowTask afterTask)
        {
            IList<IGateway> gatewaies = new List<IGateway>
            {
                new DecisionGateway(Instance),
                new ForkGateway(Instance),
                new JoinGateway(Instance)
            };

            foreach (IGateway gateway in gatewaies)
            {
                gateway.Accept(new DispatchVisitor(Nodes,Instance,props,destination,afterTask));
            }
        }
    }
}
