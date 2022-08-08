using Gazel;
using Gazel.DataAccess;
using Inventiv.Todo.Module.TaskManagement.Service;
using System.Collections.Generic;
using System.Linq;

namespace Inventiv.Todo.Module.TaskManagement
{
    public class Task : ITaskService, ITaskInfo
    {
        private readonly IRepository<Task> repository;

        protected Task() { }
        public Task(IRepository<Task> repository)
        {
            this.repository = repository;
        }

        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual bool Completed { get; protected set; }

        protected internal virtual Task With(string name)
        {
            Name = name;
            Completed = false;

            repository.Insert(this);

            return this;
        }

        public virtual void Complete()
        {
            Completed = true;
        }
    }

    public class Tasks : Query<Task>, ITasksService
    {
        public Tasks(IModuleContext context) : base(context) { }

        public List<Task> ByCompleted(bool completed)
        {
            return By(t => t.Completed == completed);
        }

        #region Service Mappings

        ITaskInfo ITasksService.GetTask(int taskId) =>
            SingleById(taskId);

        List<ITaskInfo> ITasksService.GetTasks(bool completed) =>
            ByCompleted(completed).Cast<ITaskInfo>().ToList();

        #endregion
    }
}