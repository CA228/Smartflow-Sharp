using Smartflow.Core.Cache;
using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using Smartflow.Core.Handler;
using System.Linq;

namespace Smartflow.Core.Dispatch
{
    public class DispatchCore : DispatchAbstractCore,IDispatch
    {
        protected WorkflowTask Task { get; }

        protected WorkflowContext Context { get; }

        public DispatchCore(WorkflowInstance instance, WorkflowTask task, WorkflowContext context) : base(instance)
        {
            this.Context = context;
            this.Task = task;
        }

        public  void Dispatch()
        {
            Node current = Nodes.Where(e => e.Id == Task.Code).FirstOrDefault();
            Transition transition = current.Transitions.Where(c => c.Id == Context.LineId).FirstOrDefault();
            Node to = Nodes.Where(n => n.Id == transition.Destination).FirstOrDefault();
            
            ChainFactory.Chain()
                  .Add(new WorkflowRecordHandler(Instance.Id, Context.Submiter, current, transition.Name, Context.Message))
                  .Add(new WorkflowRecordMailHandler(Instance.Creator, Context.Message))
                  .Done();

            if (to.NodeType == WorkflowNodeCategory.End) return;
            
            WorkflowTask afterTask=base.CreateTask(to, transition.Id, Context.Submiter, Context.Parallel, Task.Id, Context.Children, Context.Users, Context.Roles);

            base.DispatchBranchTask(Context.Props,to, afterTask);
        }
    }
}
