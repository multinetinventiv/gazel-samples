using Gazel;
using Gazel.DataAccess;

namespace Inventiv.Todo.Module.TaskManagement
{
    public class Task
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

    public class Tasks : Query<Task>
    {
        public Tasks(IModuleContext context) : base(context) { }
    }
}