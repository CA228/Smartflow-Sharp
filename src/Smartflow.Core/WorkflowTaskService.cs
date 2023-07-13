using System;
using System.Collections.Generic;
using System.Linq;
using Smartflow.Core.Elements;
using System.Data;
using NHibernate;
using Smartflow.Common;
using Smartflow.Core.Cache;
using Smartflow.Core.Chain;
using Smartflow.Core.Handler;

namespace Smartflow.Core
{
    public class WorkflowTaskService : IWorkflowTaskService
    {
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

        public WorkflowTask CreateTask(Node to,string lineCode, string instanceId, string publisher, long parentId, bool parallel,int status=0)
        {
            return this.Persist(new WorkflowTask
            {
                CreateTime = DateTime.Now,
                Name = to.Name,
                LineCode=lineCode,
                Code = to.Id,
                InstanceId = instanceId,
                Type = to.NodeType.ToString(),
                Creator = publisher,
                Parallel = parallel ? 1 : 0,
                ParentId = parentId,
                Status = status
            });
        }

        public IList<WorkflowTask> GetUserTaskListByUserId(string userId)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                  .GetNamedQuery("queryUserTaskListByUserId")
                  .SetParameter("userId", userId).List<WorkflowTask>();
        }

        public void CreateParallelProcess(WorkflowInstance instance, string publisher, WorkflowSubprocess subprocess)
        {
            IList<WorkflowTask> workflowTasks = GetTaskListByInstanceId(subprocess.InstanceId).Where(c=>c.Code.Equals(subprocess.Code)).ToList();
            if (workflowTasks.Count > 0)
            {
                Node clone = CacheFactory.Instance.GetNodeById(subprocess.TemplateId, subprocess.Code);
                WorkflowTask task = workflowTasks[0];
                WorkflowTask afterTask = this.CreateTask(clone,String.Empty, task.InstanceId, publisher, task.ParentId, true,0);
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

        public IList<WorkflowTask> GetTaskListByInstanceId(string instanceId)
        {
            using ISession session = DbFactory.OpenSession();
            return session
                    .Query<WorkflowTask>()
                    .Where(e => e.InstanceId == instanceId).ToList();
        }
    }
}
