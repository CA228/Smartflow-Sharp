using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Smartflow;
using Smartflow.Core.Elements;

namespace Smartflow.Core.Internals
{
    internal class Utils
    {
        public static WorkflowNodeCategory Convert(string category)
        {
            return (WorkflowNodeCategory)Enum.Parse(typeof(WorkflowNodeCategory), category, true);
        }

        public static Object CreateInstance(Type createType)
        {
            return System.Activator.CreateInstance(createType);
        }

        public static Object CreateInstance(string typeName)
        {
            return Assembly.GetExecutingAssembly().CreateInstance(typeName);
        }
    }
}
