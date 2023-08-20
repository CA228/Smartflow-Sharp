using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartflow.Core.Dispatch
{
    public class DispatchVisitor : IDispatchVisitor
    {
        protected WorkflowInstance Instance;

        protected IList<Node> Nodes { get; }

        protected Node Destination { get; }

        protected WorkflowTask Task { get; }

        protected string Props { get; }

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
            if (CheckGateway(WorkflowNodeCategory.Decision)) return;
            Transition transition = decision.NodeService.GetTransition(Props, Destination);
            decision.DispatchTask(transition,String.Empty,Props,Task.Id,false);
        }

        public void Visit(ForkGateway fork)
        {
            if (CheckGateway(WorkflowNodeCategory.Fork)) return;
            ISet<Transition> transitions = Destination.Transitions;
            foreach (Transition transition in transitions)
            {
                fork.DispatchTask(transition, String.Empty, Props, Task.Id, true);
            }
        }

        public void Visit(JoinGateway join)
        {
            if (CheckGateway(WorkflowNodeCategory.Join)) return;
            IList<Transition> previous = join.NodeService.GetPreviousTransitions(Nodes, Destination);
            int taskCount = join.TaskService.GetTaskListByInstanceId(Instance.Id).Where(c => previous.Where(s => s.Id == c.LineCode).Count() > 0 && c.Status == 1).Count();
            if (previous.Count != taskCount) return;
            Transition transition = Destination.Transitions.FirstOrDefault();
            join.DispatchTask(transition, String.Empty, Props, Task.Id, true);
        }

        protected bool CheckGateway(WorkflowNodeCategory nodeCategory)
        {
            return Destination.NodeType != nodeCategory;
        }
    }
}
