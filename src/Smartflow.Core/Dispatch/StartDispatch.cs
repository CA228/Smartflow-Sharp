using Smartflow.Core.Chain;
using Smartflow.Core.Elements;
using Smartflow.Core.Handler;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Core.Dispatch
{
    public class StartDispatch : DispatchAbstractCore, IDispatch
    {
        private  WorkflowStartInfo Start { get; }
        
        private Node Destination { get; }

        public StartDispatch(WorkflowInstance instance,Node destination, WorkflowStartInfo start):base(instance)
        {
            this.Start = start;
            this.Destination = destination;
        }
        
        public void Dispatch()
        {
            ISet<Transition> transitions = Destination.Transitions;
            Transition transition = transitions.FirstOrDefault();
            Node destination = Nodes.Where(n => n.Id == transition.Destination).FirstOrDefault();
            WorkflowTask afterTask=this.CreateTask(destination,transition.Id, Start.Creator, Start.Parallel, 0, Start.Children, Start.Users, Start.Roles);
            
            ChainFactory.Chain()
                .Add(new WorkflowRecordHandler(Instance.Id, Start.Creator, Destination, transition.Name, Start.Message))
                .Add(new WorkflowRecordMailHandler(Instance.Creator, Start.Message))
                .Done();

            base.DispatchBranchTask(Start.Props,destination, afterTask);
        }
    }
}
