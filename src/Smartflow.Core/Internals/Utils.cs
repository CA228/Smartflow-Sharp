using System;
using System.Linq;
using System.Xml.Linq;

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

        public static bool CheckAttributes(XElement ele,string attrName)
        {
            return ele.Attributes().Where(c => c.Name == attrName).Count() > 0;
        }
    }
}
