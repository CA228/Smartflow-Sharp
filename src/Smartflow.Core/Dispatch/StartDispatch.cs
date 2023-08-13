using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using Smartflow.Core.Handler;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Core.Dispatch
{
    public class StartDispatch : DispatchAbstractCore, IDispatch
    {
        private WorkflowStartInput Start { get; }

        private Node Destination { get; }

        protected StartDispatch(WorkflowInstance instance, Node destination, WorkflowStartInput start) : base(instance)
        {
            this.Start = start;
            this.Destination = destination;
        }

        public void Dispatch()
        {
            ISet<Transition> transitions = Destination.Transitions;
            Transition transition = transitions.FirstOrDefault();
            Node destination = Nodes.Where(n => n.Id == transition.Destination).FirstOrDefault();
            ChainFactory.Chain()
              .Add(new WorkflowRecordHandler(Instance.Id, Start.Creator, Destination, transition.Name, Start.Message))
              .Add(new WorkflowRecordMailHandler(Instance.Creator, Start.Message))
              .Done();
            if (destination.NodeType == WorkflowNodeCategory.Collaboration)
            {
                CollaborationTask.CreateInstance(Instance, destination, transition.Id, 0,Start.Creator,Start.Users,Start.Roles).Dispatch();
            }
            else
            {
                WorkflowTask afterTask = this.CreateTask(destination, transition.Id, Start.Creator, Start.Parallel, 0, Start.Children, Start.Users, Start.Roles);
                base.DispatchBranchTask(Start.Props, destination, afterTask);
            }
        }

        public static IDispatch CreateInstance(WorkflowInstance instance, Node destination, WorkflowStartInput start)
        {
            return new StartDispatch(instance, destination, start);
        }
    }
}
