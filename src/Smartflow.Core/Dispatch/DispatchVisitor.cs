using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Core.Dispatch
{
    public class DispatchVisitor : IDispatchVisitor
    {
        protected WorkflowInstance Instance;

        protected IList<Node> Nodes { get; }

        protected Node Destination { get; }

        protected WorkflowTask Task { get; }

        public string Props { get; }

        public DispatchVisitor(IList<Node> nodes, WorkflowInstance instance, string props, Node destination, WorkflowTask task)
        {
            this.Nodes = nodes;
            this.Instance = instance;
            this.Destination = destination;
            this.Task = task;
            this.Props = props;
        }

        public void Visit(DecisionGateway decision)
        {
            if (Destination.NodeType != WorkflowNodeCategory.Decision) return;
            Transition transition = decision.NodeService.GetTransition(Props, Destination);
            Node destination = Nodes.Where(c => c.Id == transition.Destination).FirstOrDefault();
            WorkflowTask afterTask = decision.CreateTask(destination, transition.Id, string.Empty, false, Task.Id);
            decision.DispatchBranchTask(Props, destination, afterTask);
        }

        public void Visit(ForkGateway fork)
        {
            if (Destination.NodeType != WorkflowNodeCategory.Fork) return;
            ISet<Transition> transitions = Destination.Transitions;
            foreach (Transition transition in transitions)
            {
                Node destination = Nodes.Where(c => c.Id == transition.Destination).FirstOrDefault();
                WorkflowTask afterTask = fork.CreateTask(destination, transition.Id, string.Empty, true, Task.Id);
                fork.DispatchBranchTask(Props, destination, afterTask);
            }
        }

        public void Visit(JoinGateway join)
        {
            if (Destination.NodeType != WorkflowNodeCategory.Join) return;
            IList<Transition> previous = join.NodeService.GetPreviousTransitions(Nodes, Destination);
            int taskCount = join.TaskService.GetTaskListByInstanceId(Instance.Id).Where(c => previous.Where(s => s.Id == c.LineCode).Count() > 0 && c.Status == 1).Count();
            if (previous.Count == taskCount)
            {
                Transition transition = Destination.Transitions.FirstOrDefault();
                Node destination = Nodes.Where(c => c.Id == transition.Destination).FirstOrDefault();
                WorkflowTask afterTask = join.CreateTask(destination, transition.Id, string.Empty, true, Task.Id);
                join.DispatchBranchTask(Props, destination, afterTask);
            }
        }
    }
}
