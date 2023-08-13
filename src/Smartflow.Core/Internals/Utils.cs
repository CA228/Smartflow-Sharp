using Smartflow.Core.Elements;
using System;
using System.Collections.Generic;
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

        public static IList<WorkflowTaskActor> GetTaskActorList(Node to,IList<string> users=null, IList<string> roles=null, IList<string> organizations=null)
        {
            IList<WorkflowTaskActor> actors = new List<WorkflowTaskActor>();

            if (to.Groups != null)
            {
                foreach (Group g in to.Groups)
                {
                    actors.Add(new WorkflowTaskActor
                    {
                        Id = g.Id,
                        Type = 0
                    });
                }
            }

            if (to.Actors != null)
            {
                foreach (Actor a in to.Actors)
                {
                    actors.Add(new WorkflowTaskActor
                    {
                        Id = a.Id,
                        Type = 1
                    });
                }
            }

            if (roles != null)
            {
                foreach (string role in roles)
                {
                    actors.Add(new WorkflowTaskActor
                    {
                        Id = role,
                        Type = 0
                    });
                }
            }

            if (users != null)
            {
                foreach (string user in users)
                {
                    actors.Add(new WorkflowTaskActor
                    {
                        Id = user,
                        Type = 1
                    });
                }
            }

            if (organizations != null)
            {
                foreach (string organizationCode in organizations)
                {
                    actors.Add(new WorkflowTaskActor
                    {
                        Id = organizationCode,
                        Type = 2
                    });
                }
            }

            if (to.Organizations != null)
            {
                foreach (Organization o in to.Organizations)
                {
                    actors.Add(new WorkflowTaskActor
                    {
                        Id = o.Id,
                        Type = 2
                    });
                }
            }

            return actors;
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
