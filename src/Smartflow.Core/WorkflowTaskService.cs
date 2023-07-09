using System;
using System.Collections.Generic;
using System.Linq;
using Smartflow.Core.Elements;
using System.Data;
using NHibernate;
using Smartflow.Common;
using Smartflow.Core.Cache;
using Smartflow.Core.Chain;
using Smartflow.Core.Handlers;

namespace Smartflow.Core
{
    public class WorkflowTaskService : IWorkflowTaskService
    {
        private readonly IWorkflowNodeService nodeService = WorkflowGlobalServiceProvider.Resolve<IWorkflowNodeService>();
     
        public WorkflowTask Persist(WorkflowTask task)
        {
            using ISession session = DbFactory.OpenSession();
            session.SaveOrUpdate(task);
            session.Flush();
            return task;
        }

        public WorkflowTask GetTaskById(long id)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                    .Query<WorkflowTask>()
                    .Where(e => e.Id == id)
                    .FirstOrDefault();
        }

        public void CreateTask(WorkflowInstance instance, IList<Node> nodes, string nodeCode, string props, string transitionId, string publisher, bool parallel,long parentId, IList<WorkflowSubprocess> children = null, IList<string> users = null, IList<string> roles = null)
        {
            Node current = nodes.Where(e => e.Id == nodeCode).FirstOrDefault();
            Transition transition = current.Transitions.Where(c => c.Id == transitionId).FirstOrDefault();
            Node to = nodes.Where(e => e.Id == transition.Destination).FirstOrDefault();
           
            //结束节点，直接终止执行
            if (to.NodeType == WorkflowNodeCategory.End) return;

            WorkflowTask task = CreateTask(to, instance.Id, publisher, parentId, false, to.NodeType == WorkflowNodeCategory.Decision?1:0);

            if (children != null)
            {
                foreach (WorkflowSubprocess subprocess in children)
                {
                    this.CreateSubprocess(instance, task.Id, publisher, subprocess, parallel);
                }
            }

            ChainFactory.Chain()
                .Add(new WorkflowTaskActorHandler(task.Id, to, users, roles))
                .Add(new WorkflowTaskMailHandler(instance.CategoryCode, task.Id))
                .Done();
            
            this.CreateBranchTask(instance, nodes, task, to,publisher, props);
        }

        protected WorkflowTask CreateTask(Node to, string instanceId, string publisher, long parentId, bool parallel,int status=0)
        {
            return this.Persist(new WorkflowTask
            {
                CreateTime = DateTime.Now,
                Name = to.Name,
                Code = to.Id,
                InstanceId = instanceId,
                Type = to.NodeType.ToString(),
                Creator = publisher,
                Parallel = parallel ? 1 : 0,
                ParentId = parentId,
                Status = status
            });
        }

        protected void CreateBranchTask(WorkflowInstance instance, IList<Node> nodes, WorkflowTask afterTask, Node to,string creator, string props)
        {
            if (to.NodeType == WorkflowNodeCategory.Decision)
            {
                Transition selTransition = nodeService.GetTransition(props, to);
                this.CreateTask(instance, nodes, to.Id, props, selTransition.Id, creator, false,afterTask.Id);
            }
        }

        public IList<WorkflowTask> GetUserTaskListByUserId(string userId)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                  .GetNamedQuery("queryUserTaskListByUserId")
                  .SetParameter("userId", userId).List<WorkflowTask>();
        }

        public void CreateSubprocess(WorkflowInstance instance, long taskId, string publisher, WorkflowSubprocess subprocess, bool parallel)
        {
            Node node = CacheFactory.Instance.GetNodeById(subprocess.TemplateId, subprocess.Code);
            WorkflowTask afterTask = this.CreateTask(node, instance.Id, publisher, taskId, parallel,0);
            ChainFactory.Chain()
                            .Add(new WorkflowTaskActorHandler(afterTask.Id, node, subprocess.Users, subprocess.Roles))
                            .Add(new WorkflowTaskMailHandler(instance.CategoryCode, afterTask.Id))
                            .Done();
        }

        public void CreateParallelProcess(WorkflowInstance instance, string publisher, WorkflowSubprocess subprocess)
        {
            IList<WorkflowTask> workflowTasks = GetTaskListByInstanceId(subprocess.Code, subprocess.InstanceId);
            if (workflowTasks.Count > 0)
            {
                Node clone = CacheFactory.Instance.GetNodeById(subprocess.TemplateId, subprocess.Code);
                WorkflowTask task = workflowTasks[0];
                WorkflowTask afterTask = this.CreateTask(clone, task.InstanceId, publisher, task.ParentId, true,0);
                this.ChangeParallel(task.ParentId);
                ChainFactory.Chain()
                             .Add(new WorkflowTaskActorHandler(afterTask.Id, clone, subprocess.Users, subprocess.Roles))
                             .Add(new WorkflowTaskMailHandler(instance.CategoryCode, afterTask.Id))
                             .Done();
            }
        }

        private void ChangeParallel(long parentId)
        {
            using ISession session = DbFactory.OpenSession();
            using var tx = session.BeginTransaction();
            string hql = "update  WorkflowTask c set Parallel=1 where c.ParentId = :ParentId and c.Type='Node'";
            int deletedEntities = session.CreateQuery(hql).SetParameter("ParentId", parentId, NHibernateUtil.Int64).ExecuteUpdate();
            tx.Commit();
        }

        public IList<WorkflowTask> GetTaskListByInstanceId(string code, string instanceId)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                    .Query<WorkflowTask>()
                    .Where(e => e.Code == code && e.InstanceId == instanceId).ToList();
        }
    }
}
