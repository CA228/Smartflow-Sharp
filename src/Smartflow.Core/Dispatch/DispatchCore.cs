using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using Smartflow.Core.Handler;
using System.Linq;

namespace Smartflow.Core.Dispatch
{
    public class DispatchCore : DispatchAbstractCore, IDispatch
    {
        protected WorkflowTask Task { get; }

        protected WorkflowSubmitInput Input { get; }

        protected DispatchCore(WorkflowInstance instance, WorkflowTask task, WorkflowSubmitInput input) : base(instance)
        {
            this.Input = input;
            this.Task = task;
        }

        public void Dispatch()
        {
            Node current = Nodes.Where(e => e.Id == Task.Code).FirstOrDefault();
            Transition transition = current.Transitions.Where(c => c.Id == Input.LineId).FirstOrDefault();
            ChainFactory.Chain()
                  .Add(new WorkflowRecordHandler(Instance.Id, Input.Submiter, current, transition.Name, Input.Message))
                  .Add(new WorkflowRecordMailHandler(Instance.Creator, Input.Message))
                  .Done();
            if (Input.Close == 1) return;
            if (TaskService.CheckTaskCompleted(Instance.Id, Task.Code)) return;
            Node to = Nodes.Where(n => n.Id == transition.Destination).FirstOrDefault();
            if (to.NodeType == WorkflowNodeCategory.End) return;
            
            base.DispatchTask(transition, Input.Submiter, Input.Props, Task.Id, Input.Parallel, Input.Children, Input.Users, Input.Roles);
        }

        public static IDispatch CreateInstance(WorkflowInstance instance, WorkflowTask task, WorkflowSubmitInput input)
        {
            return new DispatchCore(instance, task, input);
        }
    }
}
