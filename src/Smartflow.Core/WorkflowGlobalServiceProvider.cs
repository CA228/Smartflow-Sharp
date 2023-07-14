using System;
using System.Collections.Generic;
using System.Linq;
using Smartflow.Core.Mail;

namespace Smartflow.Core
{
    public static class WorkflowGlobalServiceProvider
    {
        private static readonly IList<Type> globalServiceCollection = new List<Type>();

        static WorkflowGlobalServiceProvider()
        {
            globalServiceCollection.Add(typeof(WorkflowService));
            globalServiceCollection.Add(typeof(WorkflowNodeService));
            globalServiceCollection.Add(typeof(WorkflowTaskService));
            globalServiceCollection.Add(typeof(WorkflowTransitionService));
            globalServiceCollection.Add(typeof(WorkflowTemplateService));
            globalServiceCollection.Add(typeof(WorkflowRecordService)); 
            globalServiceCollection.Add(typeof(WorkflowTaskAuthService));
            globalServiceCollection.Add(typeof(MailService));
        }

        public static void RegisterGlobalService(Type registerType)
        {
            if (globalServiceCollection.Contains(registerType))
            {
                globalServiceCollection.Remove(registerType);
            }
            globalServiceCollection.Add(registerType);
        }

        public static T Resolve<T>()
        {
            Type map = globalServiceCollection
                      .Where(e => typeof(T).IsAssignableFrom(e))
                      .FirstOrDefault();

            return (map == null) ? default : (T)Smartflow.Core.Internals.Utils.CreateInstance(map);
        }
    }
}
