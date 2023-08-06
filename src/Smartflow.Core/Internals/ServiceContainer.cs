using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Core.Internals
{
    internal class ServiceContainer
    {
        private static readonly Dictionary<string, Type> innerHandlerMap = new Dictionary<string, Type>();

        static ServiceContainer()
        {
            innerHandlerMap.Add("start", typeof(WorkflowNodeService));
            innerHandlerMap.Add("end", typeof(WorkflowNodeService));
            innerHandlerMap.Add("decision", typeof(WorkflowNodeService));
            innerHandlerMap.Add("node", typeof(WorkflowNodeService));
            innerHandlerMap.Add("group", typeof(WorkflowGroupService));
            innerHandlerMap.Add("transition", typeof(WorkflowTransitionService));
            innerHandlerMap.Add("actor", typeof(WorkflowActorService));
            innerHandlerMap.Add("organization", typeof(WorkflowOrganizationService));
            innerHandlerMap.Add("fork", typeof(WorkflowNodeService));
            innerHandlerMap.Add("join", typeof(WorkflowNodeService));
        }

        public static Object Resolve(string name)
        {
            Type innerType = innerHandlerMap[name];
            return Utils.CreateInstance(innerType);
        }

        public static bool Contains(string name)
        {
            return innerHandlerMap.ContainsKey(name);
        }
    }
}
